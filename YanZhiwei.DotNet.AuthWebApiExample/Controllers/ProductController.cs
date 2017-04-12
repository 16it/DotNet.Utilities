using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using YanZhiwei.DotNet.AuthWebApiExample.Models;
using YanZhiwei.DotNet2.Utilities.Result;

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

            HttpResponseMessage _result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(AjaxResult<Product>.Success(product)), Encoding.GetEncoding("UTF-8"), "application/json") };
            return _result;
        }

        [HttpPost]
        public HttpResponseMessage AddProudct(Product product)
        {
            HttpResponseMessage _result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(AjaxResult<Product>.Success(product)), Encoding.GetEncoding("UTF-8"), "application/json") };
            return _result;
        }
    }
}