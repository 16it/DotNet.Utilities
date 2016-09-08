namespace YanZhiwei.Framework.Mvc
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// 用于Controller上用于用户验证特性
    /// </summary>
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        #region Fields

        /// <summary>
        /// 权限名称
        /// </summary>
        public readonly string Name = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">权限名称</param>
        public AuthorizeFilterAttribute(string name)
        {
            this.Name = name;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。
        /// </summary>
        /// <param name="filterContext">筛选器上下文。</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!this.Authorize(filterContext, this.Name))
                filterContext.Result = new ContentResult { Content = "<script>alert('抱歉,你不具有当前操作的权限！');history.go(-1)</script>" };
        }

        /// <summary>
        /// 验证筛选器上下文
        /// </summary>
        /// <param name="filterContext">筛选器上下文</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
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