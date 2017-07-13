namespace YanZhiwei.DotNet4._5.Utilities.WebForm
{
    using System;
    using System.IO;
    using System.Net.Http;

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
        public static string ToRaw(this HttpRequestMessage request)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteBasic(request, writer);
                WriteHeaders(request, writer);
                WriteBody(request, writer);
                return writer.ToString();
            }
        }

        private static void WriteBasic(HttpRequestMessage request, StringWriter writer)
        {
            writer.WriteLine(request.Method);
            writer.WriteLine(request.RequestUri);
            writer.WriteLine("HTTP/" + request.Version);
        }

        private static void WriteBody(HttpRequestMessage request, StringWriter writer)
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
                if (!string.IsNullOrEmpty(_bodyString))
                    writer.WriteLine(_bodyString);
            }
            catch (Exception)
            {
            }
        }

        private static void WriteHeaders(HttpRequestMessage request, StringWriter writer)
        {
            writer.Write(request.Headers.ToString());
            writer.Write(request.Content.Headers.ToString());
            writer.WriteLine();
        }

        #endregion Methods
    }
}