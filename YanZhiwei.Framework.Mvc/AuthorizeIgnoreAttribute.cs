namespace YanZhiwei.DotNet.Framework.Mvc
{
    using System;

    /// <summary>
    /// 用于Controller上用于忽略用户验证特性
    /// </summary>
    public class AuthorizeIgnoreAttribute : Attribute
    {
        #region Constructors       
        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthorizeIgnoreAttribute()
        {
        }

        #endregion Constructors
    }
}