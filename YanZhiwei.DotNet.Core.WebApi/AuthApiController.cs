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
    public abstract class AuthApiController : ApiBaseController, IAuthorizationApi
    {
        /// <summary>
        /// 注册用户获取访问令牌接口
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passWord">用户密码</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appid">应用接入ID</param>
        /// <returns>OperatedResult</returns>
        public OperatedResult<TokenInfo> GetAccessToken(string userId, string passWord,
                string signature, string timestamp,
                string nonce, Guid appid)
        {
            CheckResult _checkedParamter = CheckedCreateAccessTokenParamter(userId, passWord, signature, timestamp, nonce, appid);

            if (!_checkedParamter.State)
                return OperatedResult<TokenInfo>.Fail(_checkedParamter.Message);

            #region 检查用户合法性

            CheckResult<UserInfo> _checkedUser = CheckedUser(userId, passWord);

            if (!_checkedUser.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedUser.Message);
            }

            #endregion 检查用户合法性

            #region 检查api请求通道合法性

            CheckResult<AppInfo> _checkedAppChannel = CheckedAppInfo(appid);

            if (!_checkedAppChannel.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedAppChannel.Message);
            }

            AppInfo _appInfo = _checkedAppChannel.Data;

            #endregion 检查api请求通道合法性

            #region 检查请求签名合法性

            CheckResult _checkedSignature = SignatureHelper.Validate(signature, timestamp, nonce, _appInfo.AppSecret, _appInfo.SignatureExpiredMinutes);

            if (!_checkedSignature.State)
            {
                return OperatedResult<TokenInfo>.Fail(_checkedSignature.Message);
            }

            #endregion 检查请求签名合法性

            UserInfo _userInfo = _checkedUser.Data;
            TokenInfo _tokenData = BuilderTokenInfo(_userInfo, _appInfo);
            return OperatedResult<TokenInfo>.Success(_tokenData);
        }

        private CheckResult CheckedCreateAccessTokenParamter(string userId, string passWord, string signature, string timestamp, string nonce, Guid appid)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(passWord))
                return CheckResult.Fail("用户名或密码为空");

            if (string.IsNullOrEmpty(signature))
                return CheckResult.Fail("请求签名为空");

            if (string.IsNullOrEmpty(timestamp))
                return CheckResult.Fail("时间戳为空");

            if (string.IsNullOrEmpty(nonce))
                return CheckResult.Fail("随机数为空");

            if (appid == Guid.Empty)
                return CheckResult.Fail("应用接入ID非法");

            return CheckResult.Success();
        }

        private TokenInfo BuilderTokenInfo(UserInfo userInfo, AppInfo appInfo)
        {
            Dictionary<string, object> _payload = new Dictionary<string, object>()
            {
                { "iss", userInfo.UserId},
                { "iat", UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds}
            };//负载数据
            TokenInfo _tokenData = new TokenInfo
            {
                Access_token = JwtHelper.CreateTokens(appInfo.SharedKey, _payload),
                Expires_in = appInfo.TokenExpiredDay * 24 * 3600
            };
            return _tokenData;
        }

        /// <summary>
        /// 检查用户的合法性
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passWord">用户密码</param>
        /// <returns>UserInfo</returns>
        public abstract CheckResult<UserInfo> CheckedUser(string userId, string passWord);

        /// <summary>
        /// 检查API请求通道合法性
        /// </summary>
        /// <param name="appid">应用接入ID</param>
        /// <returns>AppInfo</returns>
        public abstract CheckResult<AppInfo> CheckedAppInfo(Guid appid);
    }
}