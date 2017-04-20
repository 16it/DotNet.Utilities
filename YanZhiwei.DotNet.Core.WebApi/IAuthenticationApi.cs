using System;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Result;

namespace YanZhiwei.DotNet.Core.WebApi
{
    /// <summary>
    /// webApi 验证系统基本接口
    /// </summary>
    public interface IAuthenticationApi
    {
        /// <summary>
        /// 验证Token令牌是否合法
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="appid">应用ID</param>
        /// <param name="checkAppChannelFactory">检查APP通道合法性 委托</param>
        /// <returns>CheckResult</returns>
        CheckResult ValidateToken(string token, string appid,
                                  Func<string, OperatedResult<AppInfo>> checkAppChannelFactory);
    }
}