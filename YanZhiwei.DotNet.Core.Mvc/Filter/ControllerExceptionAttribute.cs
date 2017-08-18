namespace YanZhiwei.DotNet.Core.Mvc.Filter
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 系统集中异常处理
    /// 重写官方默认的HandleErrorAttribute处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ControllerExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        #region Methods

        /// <summary>
        /// 处理"服务器不理解请求的语法"
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        public virtual void HanlderStatusCode_400(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
        {
            if (!isAjaxRequest)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "Page_400", //错误页
                    MasterName = null,     //指定母版页
                    ViewData = null,       //指定模型
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 400;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest, filterContext.Exception.Message);
            }
        }

        /// <summary>
        /// 处理"请求要求身份验证"
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        public virtual void HanlderStatusCode_401(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
        {
            if (!isAjaxRequest)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "Page_401", //错误页
                    MasterName = null,     //指定母版页
                    ViewData = null,       //指定模型
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 401;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "你必须登录才能执行此操作");
            }
        }

        /// <summary>
        /// 处理"服务器拒绝请求"
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        public virtual void HanlderStatusCode_403(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
        {
            if (!isAjaxRequest)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "Page_403", //错误页
                    MasterName = null,     //指定母版页
                    ViewData = null,       //指定模型
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 403;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, "您没有访问此操作的权限。");
            }
        }

        /// <summary>
        /// 处理"服务器找不到请求的网页"
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        public virtual void HanlderStatusCode_404(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
        {
            if (!isAjaxRequest)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "Page_404", //错误页
                    MasterName = null,     //指定母版页
                    ViewData = null,       //指定模型
                    TempData = filterContext.Controller.TempData //临时数据
                };
                filterContext.Result = result;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 404;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.NotFound, "未找到请求的资源");
            }
        }

        /// <summary>
        /// 处理"服务器内部错误"
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        public virtual void HanlderStatusCode_500(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
        {
            if (!isAjaxRequest)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "Page_500", //错误页
                    MasterName = null,     //指定母版页
                    ViewData = null,       //指定模型
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, filterContext.Exception.Message);
            }
        }

        /// <summary>
        /// 处理未知状态异常
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        public virtual void HanlderStatusCode_Unknown(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
        {
            if (!isAjaxRequest)
            {
                ViewResult result = new ViewResult
                {
                    ViewName = "Page_500",
                    MasterName = null,
                    ViewData = null,
                    TempData = filterContext.Controller.TempData
                };
                filterContext.Result = result;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 500;
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, filterContext.Exception.Message);
            }
        }

        /// <summary>
        /// 在发生异常时调用。
        /// </summary>
        /// <param name="filterContext">筛选器上下文。</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled == true)
            {
                return;
            }
            else
            {
                string _controlName = (string)filterContext.RouteData.Values["controller"];
                string _actionName = (string)filterContext.RouteData.Values["action"];
                string _url = filterContext.RequestContext.HttpContext.Request.Url.LocalPath;
                UnHanlderException(_controlName, _actionName, _url, filterContext.Exception);
            }

            //查看是否通过异步请求
            bool _IsAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
            //根据http状态码 跳转到指定的异常页面
            var _httpStatusCode = new HttpException(null, filterContext.Exception).GetHashCode();

            switch (_httpStatusCode)
            {
                case 400:
                    HanlderStatusCode_400(_IsAjaxRequest, _httpStatusCode, filterContext);
                    break;

                case 401:
                    HanlderStatusCode_401(_IsAjaxRequest, _httpStatusCode, filterContext);
                    break;

                case 403:
                    HanlderStatusCode_403(_IsAjaxRequest, _httpStatusCode, filterContext);
                    break;

                case 404:
                    HanlderStatusCode_404(_IsAjaxRequest, _httpStatusCode, filterContext);
                    break;

                case 500:
                    HanlderStatusCode_500(_IsAjaxRequest, _httpStatusCode, filterContext);
                    break;

                default:
                    HanlderStatusCode_Unknown(_IsAjaxRequest, _httpStatusCode, filterContext);
                    break;
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        /// <summary>
        /// 未处理的异常抽象方法
        /// </summary>
        /// <param name="controlName">ControlName.</param>
        /// <param name="actionName">ActionName.</param>
        /// <param name="url">URL</param>
        /// <param name="ex">Exception</param>
        public abstract void UnHanlderException(string controlName, string actionName, string url, Exception ex);

        #endregion Methods
    }
}