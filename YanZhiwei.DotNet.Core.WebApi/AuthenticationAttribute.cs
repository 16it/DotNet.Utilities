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
    public class AuthenticationAttribute : AuthorizationFilterAttribute, IAuthenticationApi
    {
        /// <summary>
        /// 验证Token令牌是否合法
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="appid">应用ID</param>
        /// <param name="checkAppChannelFactory">检查APP通道合法性 委托</param>
        /// <returns>CheckResult</returns>
        public OperatedResult<string> ValidateToken(string token, Guid appid, Func<Guid, OperatedResult<AppInfo>> checkAppChannelFactory)
        {
            OperatedResult<AppInfo> _checkedAppChannel = checkAppChannelFactory(appid);

            if (!_checkedAppChannel.State)
            {
                return OperatedResult<string>.Fail(_checkedAppChannel.Message);
            }

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
    }
}