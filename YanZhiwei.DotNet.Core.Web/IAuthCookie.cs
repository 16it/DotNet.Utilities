using System;

namespace YanZhiwei.DotNet.Core.Web
{
    /// <summary>
    /// 验证cookie
    /// </summary>
    public interface IAuthCookie<T>
    {
        /// <summary>
        /// 用户过期时间
        /// </summary>
        int UserExpiresMinutes
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
        /// 用户Id
        /// </summary>
        T UserId
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
        
        /// <summary>
        /// 登陆错误次数
        /// </summary>
        int LoginErrorTimes
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否需要验证码
        /// </summary>
        bool IsNeedVerifyCode
        {
            get;
        }
    }
}