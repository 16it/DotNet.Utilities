namespace YanZhiwei.Framework.Mvc
{
    using System;

    /// <summary>
    /// 用于Controller上用于忽略用户验证特性
    /// </summary>
    public class AuthorizeIgnoreAttribute : Attribute
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeIgnoreAttribute"/> class.
        /// </summary>
        public AuthorizeIgnoreAttribute()
        {
        }

        #endregion Constructors
    }
}