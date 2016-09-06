namespace YanZhiwei.Framework.Mvc
{
    using System;
    
    /// <summary>
    /// Attribute for power Authorize
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