namespace YanZhiwei.DotNet4._5.Utilities.WebForm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using YanZhiwei.DotNet4._5.Utilities.Model;

    /// <summary>
    /// HttpRequest辅助类
    /// </summary>
    public static class HttpRequestHelper
    {
        #region Methods

        /// <summary>
        /// 获取HttpRequest原始信息
        /// </summary>
        /// <param name="request">HttpRequestMessage</param>
        /// <returns>HttpRequest原始信息</returns>
        public static HttpRequestRaw ToRaw(this HttpRequestMessage request)
        {
            HttpRequestRaw _requestRaw = new HttpRequestRaw();
            HttpRequestBasic(request, _requestRaw);
            HttpRequestHeaders(request, _requestRaw);
            HttpRequestBody(request, _requestRaw);
            return _requestRaw;
        }

        private static void HttpRequestBasic(HttpRequestMessage request, HttpRequestRaw requestRaw)
        {
            requestRaw.RequestMethod = request.Method.ToString();
            requestRaw.RequestUri = request.RequestUri.ToString();
            requestRaw.RequestVersion = string.Format("HTTP/{0}", request.Version);
        }

        private static void HttpRequestBody(HttpRequestMessage request, HttpRequestRaw requestRaw)
        {
            try
            {
                string _bodyString;

                using (var stream = request.Content.ReadAsStreamAsync().Result)
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = 0;
                    }

                    _bodyString = request.Content.ReadAsStringAsync().Result;
                }
                requestRaw.Body = _bodyString;
            }
            catch (Exception)
            {
            }
        }

        private static void HttpRequestHeaders(HttpRequestMessage request, HttpRequestRaw requestRaw)
        {
            requestRaw.Headers = new List<string>();
            BuilderRequestHeaderKeyValue(request.Headers, requestRaw);
            BuilderRequestHeaderKeyValue(request.Content.Headers, requestRaw);
        }

        private static void BuilderRequestHeaderKeyValue(HttpHeaders headers, HttpRequestRaw requestRaw)
        {
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    requestRaw.Headers.Add(string.Format("{0}:{1}", item.Key, headers.GetValues(item.Key).FirstOrDefault()));
                }
            }
        }

        #endregion Methods
    }
}