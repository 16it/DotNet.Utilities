using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace YanZhiwei.DotNet.WebApiExample.Filters
{
    public class ApiSecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
}