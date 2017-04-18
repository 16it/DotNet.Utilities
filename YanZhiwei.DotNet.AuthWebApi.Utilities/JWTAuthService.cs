namespace YanZhiwei.DotNet.AuthWebApi.Utilities
{
    using DotNet2.Utilities.Common;
    using DotNet2.Utilities.Encryptor;
    using DotNet2.Utilities.ExtendException;
    using DotNet2.Utilities.Result;
    using JWT;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using YanZhiwei.DotNet2.Utilities.Model;

    /// <summary>
    /// 采用JWT生成令牌WEB API验证类
    /// </summary>
    /// 时间：2016/10/20 14:11
    /// 备注：
    public class JWTAuthService : IAuthApi
    {
        #region Methods

        /// <summary>
        /// 获取用户令牌
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appSecret">应用接入ID对应Key</param>
        /// <param name="sharedKey">用于加密解密签名以及用户令牌的Key</param>
        /// <param name="timspanExpiredMinutes">时间戳过期时间【分钟】</param>
        /// <returns>
        /// 用户令牌信息
        /// </returns>
        public TokenInfo GetAccessToken(string userId, string signature, string timestamp, string nonce, string appSecret, string sharedKey, int timspanExpiredMinutes)
        {
            TokenInfo _result = new TokenInfo();
            CheckResult _checkedResult = ValidateSignature(signature, timestamp, nonce, appSecret, timspanExpiredMinutes);

            if (_checkedResult.State)
            {
                Dictionary<string, object> _payload = new Dictionary<string, object>()
                {
                    { "userId", userId},
                    { "claim", UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds}
                };
                //_payload 负载
                //
                string _token = JsonWebToken.Encode(_payload, sharedKey, JwtHashAlgorithm.HS256);
                _result.Access_token = _token;
                _result.Expires_in = timspanExpiredMinutes * 24 * 3600;
            }
            else
            {
                throw new FrameworkException(_checkedResult.Message);
            }

            return _result;
        }

        /// <summary>
        /// 检查用户令牌
        /// </summary>
        /// <param name="token">用户令牌</param>
        /// <param name="sharedKey">用于加密解密签名以及用户令牌的Key</param>
        /// <param name="tokenExpiredDays">用户令牌过期天数</param>
        /// <returns>
        /// 检查结果
        /// </returns>
        public Tuple<bool, string> ValidateToken(string token, string sharedKey, int tokenExpiredDays)
        {
            //返回的结果对象
            Tuple<bool, string> _checkeResult = new Tuple<bool, string>(false, "数据完整性检查不通过");

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    string _decodedJwt = JsonWebToken.Decode(token, sharedKey);

                    if (!string.IsNullOrEmpty(_decodedJwt))
                    {
                        dynamic _root = JObject.Parse(_decodedJwt);
                        string _userid = _root.userId;
                        double _jwtcreated = (double)_root.claim;
                        bool _validTokenExpired = (new TimeSpan((int)(UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds - _jwtcreated)).TotalDays) > tokenExpiredDays;

                        if (_validTokenExpired)
                        {
                            _checkeResult = new Tuple<bool, string>(false, "用户令牌失效.");
                        }

                        _checkeResult = new Tuple<bool, string>(true, _userid);
                    }
                }
                catch (SignatureVerificationException)
                {
                    _checkeResult = new Tuple<bool, string>(false, "用户令牌非法.");
                }
            }

            return _checkeResult;
        }

        private CheckResult ValidateSignature(string signature, string timestamp, string nonce, string appSecret, int timspanExpiredMinutes)
        {
            string[] _arrayParamter = { appSecret, timestamp, nonce };
            Array.Sort(_arrayParamter);
            string _signatureString = string.Join("", _arrayParamter);
            _signatureString = MD5Encryptor.Encrypt(_signatureString);

            if (signature.CompareIgnoreCase(signature) && CheckHelper.IsNumber(timestamp))
            {
                DateTime _timestampMillis = UnixEpochHelper.DateTimeFromUnixTimestampMillis(timestamp.ToDoubleOrDefault(0f));
                double _minutes = DateTime.UtcNow.Subtract(_timestampMillis).TotalMinutes;

                if (_minutes > timspanExpiredMinutes)
                {
                    return CheckResult.Fail("签名时间戳失效");
                }
            }

            return CheckResult.Success();
        }

        #endregion Methods
    }
}