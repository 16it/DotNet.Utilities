namespace YanZhiwei.DotNet.Core.WebApi
{
    using System;

    using YanZhiwei.DotNet2.Utilities.Model;
    using YanZhiwei.DotNet2.Utilities.Result;

    /// <summary>
    ///Webapi 授权系统基本接口
    /// </summary>
    public interface IAuthorizationApi
    {
        #region Methods

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
        OperatedResult<TokenInfo> GetAccessToken(string userId, string passWord,
            string signature, string timestamp,
            string nonce, Guid appid);

        #endregion Methods
    }
}