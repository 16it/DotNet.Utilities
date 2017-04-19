using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Encryptor;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Result;

namespace YanZhiwei.DotNet.Core.WebApi
{
    public class AuthApi : IAuthApi
    {
        public static readonly int ExpiredMinutes = 10;

        private CheckResult ValidateSignature(string signature, string timestamp, string nonce, string appSecret)
        {
            string[] _arrayParamter = { appSecret, timestamp, nonce };
            Array.Sort(_arrayParamter);
            string _signatureString = string.Join("", _arrayParamter);
            _signatureString = MD5Encryptor.Encrypt(_signatureString);

            if (signature.CompareIgnoreCase(signature) && CheckHelper.IsNumber(timestamp))
            {
                DateTime _timestampMillis = UnixEpochHelper.DateTimeFromUnixTimestampMillis(timestamp.ToDoubleOrDefault(0f));
                double _minutes = DateTime.UtcNow.Subtract(_timestampMillis).TotalMinutes;

                if (_minutes > ExpiredMinutes)
                {
                    return CheckResult.Fail("签名时间戳失效");
                }
            }

            return CheckResult.Success();
        }

        /// <summary>
        /// 检查Token是否合法
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>CheckResult</returns>
        public CheckResult ValidateToken(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册用户获取访问令牌接口
        /// </summary>
        /// <param name="checkUserFactory">检查用户，密码合法性</param>
        /// <param name="checkAppChannelFactory">检查APP通道合法性</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appid">应用接入ID</param>
        /// <returns>OperatedResult</returns>
        public OperatedResult<TokenInfo> GetAccessToken(Func<OperatedResult<UserInfo>> checkUserFactory, Func<string, OperatedResult<AppInfo>> checkAppChannelFactory, string signature, string timestamp, string nonce, string appid)
        {
            OperatedResult<AppInfo> _checkedAppChannel = checkAppChannelFactory(appid);
            if (!_checkedAppChannel.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedAppChannel.Message);
            }

            AppInfo _appInfo = _checkedAppChannel.Data;
            CheckResult _checkedSignature = ValidateSignature(signature, timestamp, nonce, _appInfo.AppSecret);
            if (!_checkedSignature.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedSignature.Message);
            }

            OperatedResult<UserInfo> _checkedUser = checkUserFactory();
            if (!_checkedUser.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedUser.Message);
            }

            UserInfo _userInfo = _checkedUser.Data;
            Dictionary<string, object> _payload = new Dictionary<string, object>()
            {
                    { "iss", _userInfo.UserId},
                    { "iat", UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds}
            };//负载

            TokenInfo _tokenData = new TokenInfo();
            IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
            IJsonSerializer _serializer = new JsonNetSerializer();
            IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder _encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);
            string _token = _encoder.Encode(_payload, _appInfo.SharedKey);

            return OperatedResult<TokenInfo>.Success(_tokenData);
        }
    }
}