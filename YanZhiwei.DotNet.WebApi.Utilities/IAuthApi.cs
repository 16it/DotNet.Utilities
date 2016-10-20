namespace YanZhiwei.DotNet.WebApi.Utilities
{
    using System;
    
    using YanZhiwei.DotNet.WebApi.Utilities.Model;
    
    /// <summary>
    /// 系统认证等基础接口
    /// </summary>
    public interface IAuthApi
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
        /// <returns>用户令牌信息</returns>
        /// 时间：2016/10/20 16:04
        /// 备注：
        TokenResult GetAccessToken(string userId, string signature, string timestamp, string nonce, string appSecret, string sharedKey, int timspanExpiredMinutes);
        
        /// <summary>
        /// 检查用户令牌
        /// </summary>
        /// <param name="token">用户令牌</param>
        /// <param name="sharedKey">用于加密解密签名以及用户令牌的Key</param>
        /// <param name="tokenExpiredDays">用户令牌过期天数</param>
        /// <returns>检查结果</returns>
        /// 时间：2016/10/20 16:08
        /// 备注：
        Tuple<bool, string> ValidateToken(string token, string sharedKey, int tokenExpiredDays);
        
        #endregion Methods
    }
}