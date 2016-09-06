namespace YanZhiwei.Framework.Mvc
{
    using System;
    using System.Web.Mvc;
    
    /// <summary>
    /// Attribute for power Authorize
    /// </summary>
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        #region Constructors
        
        public AuthorizeFilterAttribute(string name)
        {
            this.Name = name;
        }
        
        #endregion Constructors
        
        #region Properties
        
        public string Name
        {
            get;
            set;
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// 时间：2016-01-14 11:33
        /// 备注：
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!this.Authorize(filterContext, this.Name))
                filterContext.Result = new ContentResult { Content = "<script>alert('抱歉,你不具有当前操作的权限！');history.go(-1)</script>" };
        }
        
        /// <summary>
        /// Authorizes the specified filter context.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="permissionName">Name of the permission.</param>
        /// <returns></returns>
        /// 时间：2016-01-14 11:33
        /// 备注：
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        protected virtual bool Authorize(ActionExecutingContext filterContext, string permissionName)
        {
            if(filterContext.HttpContext == null)
                throw new ArgumentNullException("httpContext");
                
            if(!filterContext.HttpContext.User.Identity.IsAuthenticated)
                return false;
                
            return true;
        }
        
        #endregion Methods
    }
}