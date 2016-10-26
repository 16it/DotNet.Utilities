using System.Net.Http;
using System.Web.Http.Filters;
using YanZhiwei.DotNet2.Utilities.Exception;

namespace YanZhiwei.DotNet.AuthWebApiExample.Filters
{
    public class APIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //业务异常
            if(actionExecutedContext.Exception is FrameworkException)
            {
                actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.ExpectationFailed };
                BusinessException exception = (BusinessException)actionExecutedContext.Exception;
                actionExecutedContext.Response.Headers.Add("BusinessExceptionMessage", exception.Message);
            }
            //其它异常
            else
            {
                actionExecutedContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }
        }
    }
}