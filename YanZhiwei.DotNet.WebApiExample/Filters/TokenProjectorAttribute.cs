using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using YanZhiwei.DotNet2.Utilities.Common;
using System.Collections.Specialized;
using YanZhiwei.DotNet.WebApi.Utilities;

namespace YanZhiwei.DotNet.WebApiExample.Filters
{
    public class TokenProjectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var _request = actionContext.Request;
            // string aa = HttpUtility.UrlDecode(_request.Headers.GetValues("token").FirstOrDefault());
            NameValueCollection _queryString = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
            
            if(_queryString != null)
            {
                string _token = _queryString["token"].ToStringOrDefault(string.Empty);
                
                if(!string.IsNullOrWhiteSpace(_token))
                {
                    AuthApiContext _authContext = new AuthApiContext();
                    var aa = _authContext.ValidateToken(_token);
                }
            }
            
            //if(actionContext.ActionDescriptor.ActionName.CompareIgnoreCase("GetAccessToken"))
            //{
            //    //string userId = HttpUtility.UrlDecode(_request.Headers.GetValues("userId").FirstOrDefault()),
            //    //       signature = HttpUtility.UrlDecode(_request.Headers.GetValues("signature").FirstOrDefault()),
            //    //       timestamp = HttpUtility.UrlDecode(_request.Headers.GetValues("timestamp").FirstOrDefault()),
            //    //       nonce = HttpUtility.UrlDecode(_request.Headers.GetValues("nonce").FirstOrDefault()),
            //    //       appSecret = HttpUtility.UrlDecode(_request.Headers.GetValues("appId").FirstOrDefault());
            //}
            base.OnActionExecuting(actionContext);
        }
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}