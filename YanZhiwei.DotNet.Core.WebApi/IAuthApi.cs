using System;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Result;

namespace YanZhiwei.DotNet.Core.WebApi
{
    /// <summary>
    /// 系统认证等基础接口
    /// </summary>
    public interface IAuthApi
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
        OperatedResult<TokenInfo> GetAccessToken(Func<OperatedResult<UserInfo>> checkUserFactory,
                Func<string, OperatedResult<AppInfo>> checkAppChannelFactory, string signature, string timestamp,
                string nonce, string appid);
    }
}