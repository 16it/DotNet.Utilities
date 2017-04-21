using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using YanZhiwei.DotNet2.Utilities.Operator;
namespace YanZhiwei.DotNet.WebApi.Utilities
{
    /// <summary>
    /// HttpRequestMessage 辅助类
    /// </summary>
    public static class HttpRequestMessageHelper
    {
        /// <summary>
        /// 从请求的URL或者Header按键取值
        /// </summary>
        /// <param name="request">HttpRequestMessage</param>
        /// <param name="key">需要取的键</param>
        /// <returns>键对应的值</returns>
        public static string GetUriOrHeaderValue(this HttpRequestMessage request, string key)
        {
            ValidateOperator.Begin().NotNull(request, "HttpRequestMessage").NotNullOrEmpty(key, "需要从URL或者Header的Key");
            string _keyValue = request.RequestUri.ParseQueryString()[key];
            
            if(!string.IsNullOrWhiteSpace(_keyValue))
            {
                return _keyValue;
            }
            
            IEnumerable<string> _headerValue;
            
            if(request.Headers.TryGetValues(key, out _headerValue))
            {
                return _headerValue.FirstOrDefault();
            }
            
            return string.Empty;
        }
    }
}