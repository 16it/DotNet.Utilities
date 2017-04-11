using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using YanZhiwei.DotNet.AuthWebApiExample.Models;
using YanZhiwei.DotNet2.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet.AuthWebApiExample.Controllers
{
    public class ProductController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetProduct(string id)
        {
            var product = new Product()
            {
                Id = 1,
                Name = "哇哈哈",
                Count = 10,
                Price = 38.8
            };
            AjaxResult<Product> _ajaxResult = new AjaxResult<Product>(string.Empty, AjaxResultType.Success, product);
            HttpResponseMessage _result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(_ajaxResult), Encoding.GetEncoding("UTF-8"), "application/json") };
            return _result;
        }
        
        [HttpPost]
        public HttpResponseMessage AddProudct(Product product)
        {
            AjaxResult<Product> _ajaxResult = new AjaxResult<Product>(string.Empty, AjaxResultType.Success, product);
            HttpResponseMessage _result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(_ajaxResult), Encoding.GetEncoding("UTF-8"), "application/json") };
            return _result;
        }
    }
}