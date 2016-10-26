using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using YanZhiwei.DotNet.AuthWebApi.Utilities;
using YanZhiwei.DotNet.AuthWebApi.Utilities.Model;

namespace YanZhiwei.DotNet.WebApiExample.Controllers
{
    public class AuthController : ApiController
    {
        private AuthApiContext AuthContext = new AuthApiContext();
        
        public HttpResponseMessage GetAccessToken(string userId, string signature, string timestamp, string nonce, string appId)
        {
            string appSecret = string.Empty;
            
            if(appId == "aabbcc")
                appSecret = "XXHHAREJDDF";
                
            TokenResult _tokenResult = AuthContext.GetAccessToken(userId, signature, timestamp, nonce, appSecret);
            HttpResponseMessage _result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(_tokenResult), Encoding.GetEncoding("UTF-8"), "application/json") };
            return _result;
        }
    }
}