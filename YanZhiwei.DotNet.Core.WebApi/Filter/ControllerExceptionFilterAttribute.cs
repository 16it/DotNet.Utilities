using System;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using YanZhiwei.DotNet4._5.Utilities.WebForm;

namespace YanZhiwei.DotNet.Core.WebApi.Filter
{
    /// <summary>
    /// 异常记录过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ControllerExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 当WebApi Action发生异常
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <param name="actionName">WebApi Action名称</param>
        /// <param name="requestRaw">Request原始信息</param>
        /// <param name="customeApiException">API异常信息</param>
        public abstract void OnActionExceptioning(HttpActionContext actionContext, string actionName, string requestRaw, Exception customeApiException);

        /// <summary>
        /// 当WebApi Action发生异常
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <param name="actionName">WebApi Action名称</param>
        /// <param name="requestRaw">Request原始信息</param>
        public abstract void OnActionExceptioning(HttpActionContext actionContext, string actionName, string requestRaw);

        /// <summary>
        /// 引发异常事件。
        /// </summary>
        /// <param name="actionExecutedContext">操作的上下文。</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string _content = actionExecutedContext.Request.Content.ReadAsStringAsync().Result;
            Uri _requestUri = actionExecutedContext.Request.RequestUri;
            string _actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            StringBuilder _curRequestBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(_content))
            {
                _curRequestBuilder.AppendLine("Content:");
                _curRequestBuilder.AppendLine(string.Format("{0}", _content));
            }
            _curRequestBuilder.AppendLine(string.Format("RequestUri:{0}", _requestUri));
            _curRequestBuilder.AppendLine(string.Format("Raw HTTP Request:\n{0}", actionExecutedContext.Request.ToRaw()));

            if (actionExecutedContext.Exception != null)
            {
                Exception _customeApiException = new Exception(_curRequestBuilder.ToString(), actionExecutedContext.Exception);
                OnActionExceptioning(actionExecutedContext.ActionContext, _actionName, _curRequestBuilder.ToString(), _customeApiException);
            }
            else
            {
                OnActionExceptioning(actionExecutedContext.ActionContext, _actionName, _curRequestBuilder.ToString());
            }
        }
    }
}