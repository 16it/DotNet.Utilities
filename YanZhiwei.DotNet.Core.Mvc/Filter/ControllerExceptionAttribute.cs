using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace YanZhiwei.DotNet.Core.Mvc.Filter
{
    /// <summary>
    /// 系统集中异常处理
    /// 重写官方默认的HandleErrorAttribute处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ControllerExceptionAttribute : FilterAttribute, IExceptionFilter
    {

        /// <summary>
        /// 未处理的异常抽象方法
        /// </summary>
        /// <param name="controlName">ControlName.</param>
        /// <param name="actionName">ActionName.</param>
        /// <param name="url">URL</param>
        /// <param name="ex">Exception</param>
        public abstract void UnHanlderException(string controlName, string actionName, string url, Exception ex);

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
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, filterContext.Exception.Message);
            }

        }

        /// <summary>
        /// Hanlders the status code 500.
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        private void HanlderStatusCode_500(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
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
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, filterContext.Exception.Message);
            }

        }

        /// <summary>
        /// Hanlders the status code 404.
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        private void HanlderStatusCode_404(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
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
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "未找到请求的资源");
            }


        }

        /// <summary>
        /// Hanlders the status code 403.
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        private void HanlderStatusCode_403(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
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
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "You do not have permission to access this operation");
            }

        }

        /// <summary>
        /// Hanlders the status code 401.
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        private void HanlderStatusCode_401(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
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
        /// Hanlders the status code 400.
        /// </summary>
        /// <param name="isAjaxRequest">是否ajax请求</param>
        /// <param name="httpStatusCode">HTTP状态码</param>
        /// <param name="filterContext">ExceptionContext</param>
        private void HanlderStatusCode_400(bool isAjaxRequest, int httpStatusCode, ExceptionContext filterContext)
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
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, filterContext.Exception.Message);
            }

        }
    }
}