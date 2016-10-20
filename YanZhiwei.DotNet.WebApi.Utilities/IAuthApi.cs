using YanZhiwei.DotNet.WebApi.Utilities.Model;

namespace YanZhiwei.DotNet.WebApi.Utilities
{
    /// <summary>
    /// 系统认证等基础接口
    /// </summary>
    public interface IAuthApi
    {
        /// <summary>
        /// 注册用户获取访问令牌接口
        /// </summary>
        /// <param name="username">用户登录名称</param>
        /// <param name="password">用户密码</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appid">应用接入ID</param>
        /// <returns>令牌结果</returns>
        TokenResult GetAccessToken(string username, string password,
                                   string signature, string timestamp, string nonce, string appid);
    }
}