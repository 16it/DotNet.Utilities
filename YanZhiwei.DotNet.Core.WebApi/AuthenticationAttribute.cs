using JWT;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http.Filters;
using YanZhiwei.DotNet.JWT.Utilities;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Result;

namespace YanZhiwei.DotNet.Core.WebApi
{
    /// <summary>
    /// WebApi 授权验证实现
    /// </summary>
    public abstract class AuthenticationAttribute : AuthorizationFilterAttribute, IAuthenticationApi
    {
        /// <summary>
        /// 验证Token令牌是否合法
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="appid">应用ID</param>
        /// <returns>CheckResult</returns>
        public OperatedResult<string> ValidateToken(string token, Guid appid)
        {
            CheckResult _checkedParamter = CheckedValidateTokenParamter(token, appid);
            if (!_checkedParamter.State)
                return OperatedResult<string>.Fail(_checkedParamter.Message);

            CheckResult<AppInfo> _checkedAppChannel = CheckedAppInfo(appid);

            if (!_checkedAppChannel.State)
                return OperatedResult<string>.Fail(_checkedAppChannel.Message);

            try
            {
                AppInfo _appInfo = _checkedAppChannel.Data;
                string _tokenString = JwtHelper.ParseTokens(token, _appInfo.SharedKey);

                if (string.IsNullOrEmpty(_tokenString))
                    return OperatedResult<string>.Fail("用户令牌Token为空");

                dynamic _root = JObject.Parse(_tokenString);
                string _userid = _root.iss;
                double _jwtcreated = (double)_root.iat;
                bool _validTokenExpired =
                    (new TimeSpan((int)(UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds - _jwtcreated))
                     .TotalDays) > _appInfo.TokenExpiredDay;
                return _validTokenExpired == true ? OperatedResult<string>.Fail($"用户ID{_userid}令牌失效") : OperatedResult<string>.Success(_userid);
            }
            catch (FormatException)
            {
                return OperatedResult<string>.Fail("用户令牌非法");
            }
            catch (SignatureVerificationException)
            {
                return OperatedResult<string>.Fail("用户令牌非法");
            }
        }

        private CheckResult CheckedValidateTokenParamter(string token, Guid appid)
        {
            if (string.IsNullOrEmpty(token))
                return CheckResult.Fail("用户令牌为空");
            if (Guid.Empty == appid)
                return CheckResult.Fail("应用ID非法");
            return CheckResult.Success();
        }

        /// <summary>
        /// 检查API请求通道合法性
        /// </summary>
        /// <param name="appid">应用接入ID</param>
        /// <returns>AppInfo</returns>
        public abstract CheckResult<AppInfo> CheckedAppInfo(Guid appid);
    }
}