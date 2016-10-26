using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using YanZhiwei.DotNet.AuthWebApi.Utilities;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet.AuthWebApiExample.Filters
{
    public class TokenProjectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var _request = actionContext.Request;
            
            if(actionContext.ActionDescriptor.ActionName == "GetAccessToken")
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
                NameValueCollection _queryString = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                
                if(_queryString != null)
                {
                    string _token = _queryString["token"].ToStringOrDefault(string.Empty);
                    
                    if(!string.IsNullOrWhiteSpace(_token))
                    {
                        AuthApiContext _authContext = new AuthApiContext();
                        Tuple<bool, string> _checkedResult = _authContext.ValidateToken(_token);
                        
                        if(!_checkedResult.Item1)
                        {
                            actionContext.Response = CreateTokenResponseMessage(_checkedResult.Item2);
                            return;
                        }
                        else
                        {
                            base.OnActionExecuting(actionContext);
                        }
                    }
                }
                else
                {
                    actionContext.Response = CreateTokenResponseMessage("非法请求数据！");
                }
            }
        }
        
        private HttpResponseMessage CreateTokenResponseMessage(string content)
        {
            AjaxResult _ajaxResult = new AjaxResult(content, AjaxResultType.Warning, null);
            HttpResponseMessage _result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(_ajaxResult), Encoding.GetEncoding("UTF-8"), "application/json") };
            return _result;
        }
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}