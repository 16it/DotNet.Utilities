using System;
using System.Collections.Generic;
using YanZhiwei.DotNet.JWT.Utilities;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Result;

namespace YanZhiwei.DotNet.Core.WebApi
{
    /// <summary>
    /// WebApi 验证基类
    /// </summary>
    public class AuthApiController : ApiBaseController, IAuthorizationApi
    {
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
        public OperatedResult<TokenInfo> GetAccessToken(Func<OperatedResult<UserInfo>> checkUserFactory,
                Func<string, OperatedResult<AppInfo>> checkAppChannelFactory, string signature, string timestamp,
                string nonce, string appid)
        {
            OperatedResult<AppInfo> _checkedAppChannel = checkAppChannelFactory(appid);

            if (!_checkedAppChannel.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedAppChannel.Message);
            }

            AppInfo _appInfo = _checkedAppChannel.Data;
            CheckResult _checkedSignature = SignatureHelper.Validate(signature, timestamp, nonce, _appInfo.AppSecret, _appInfo.SignatureExpiredMinutes);

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
            };//负载数据
            TokenInfo _tokenData = new TokenInfo
            {
                Access_token = JwtHelper.CreateTokens(_appInfo.SharedKey, _payload),
                Expires_in = _appInfo.TokenExpiredDay * 24 * 3600
            };
            return OperatedResult<TokenInfo>.Success(_tokenData);
        }
    }
}