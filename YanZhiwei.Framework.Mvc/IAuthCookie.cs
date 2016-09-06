namespace YanZhiwei.Framework.Mvc
{
    using System;
    
    /// <summary>
    /// 验证cookie接口
    /// </summary>
    public interface IAuthCookie
    {
        #region Properties
        
        /// <summary>
        /// 是否需要验证码
        /// </summary>
        bool IsNeedVerifyCode
        {
            get;
        }
        
        /// <summary>
        /// 登陆错误次数
        /// </summary>
        int LoginErrorTimes
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用户过期时间【小时】
        /// </summary>
        int UserExpiresHours
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用户ID
        /// </summary>
        int UserId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用户名称
        /// </summary>
        string UserName
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用户凭据
        /// </summary>
        Guid UserToken
        {
            get;
            set;
        }
        
        /// <summary>
        /// 验证码
        /// </summary>
        string VerifyCode
        {
            get;
            set;
        }
        
        #endregion Properties
    }
}