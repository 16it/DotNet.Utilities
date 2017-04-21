using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using YanZhiwei.DotNet3._5.Utilities.Common;

namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Core
{
    /// <summary>
    /// 发起模拟Web请求
    /// </summary>
    public sealed class SimulateWebRequest
    {
        #region Fields

        /// <summary>
        /// accept
        /// </summary>
        private string accept = "*/*";

        /// <summary>
        /// 是否允许重定向
        /// </summary>
        private bool allowAutoRedirect = true;

        /// <summary>
        /// contentType
        /// </summary>
        private string contentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// 设置cookie
        /// </summary>
        private CookieContainer cookie;

        /// <summary>
        /// 过期时间
        /// </summary>
        private int time = 5000;

        #endregion Fields

        /// <summary>
        /// 设置accept(默认:*/*)
        /// </summary>
        /// <param name="accept"></param>
        public void SetAccept(string accept)
        {
            this.accept = accept;
        }

        /// <summary>
        /// 设置contentType(默认:application/x-www-form-urlencoded)
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            this.contentType = contentType;
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(CookieContainer cookie)
        {
            this.cookie = cookie;
        }

        /// <summary>
        /// 是否允许重定向(默认:true)
        /// </summary>
        /// <param name="allowAutoRedirect"></param>
        public void SetIsAllowAutoRedirect(bool allowAutoRedirect)
        {
            this.allowAutoRedirect = allowAutoRedirect;
        }

        /// <summary>
        /// 设置请求过期时间（单位：毫秒）（默认：5000）
        /// </summary>
        /// <param name="time"></param>
        public void SetTimeOut(int time)
        {
            this.time = time;
        }

        /// <summary>
        /// 远程证书验证，固定返回true
        /// </summary>
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert,
                X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        //注册证书验证回调事件，在请求之前注册
        private void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
            += RemoteCertificateValidate;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <param name="url">请求URL地址</param>
        /// <returns>返回结果</returns>
        public static T HttpPost<T>(string url)
        {
            HttpWebRequest _request = System.Net.WebRequest.Create(url) as HttpWebRequest;
            _request.Method = "POST";
            _request.Timeout = 30000;
            _request.AllowAutoRedirect = true;
            _request.ServicePoint.ConnectionLimit = int.MaxValue;
            _request.ContentLength = 0;

            using (var response = (HttpWebResponse)_request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string _responeString = reader.ReadToEnd();
                    return SerializeHelper.JsonDeserialize<T>(_responeString);
                }
            }
        }

        /// <summary>
        /// 获得响应中的图像
        /// </summary>
        /// <param name="url">链接</param>
        /// <returns>若发生异常则返回NULL</returns>
        public Stream GetResponseImage(string url)
        {
            Stream _responseStream = null;

            try
            {
                HttpWebRequest _request = (HttpWebRequest)System.Net.WebRequest.Create(url);
                _request.KeepAlive = true;
                _request.Method = "GET";
                _request.AllowAutoRedirect = allowAutoRedirect;
                _request.CookieContainer = cookie;
                _request.ContentType = this.contentType;
                _request.Accept = this.accept;
                _request.Timeout = time;
                Encoding _encoding = Encoding.GetEncoding("UTF-8");
                this.SetCertificatePolicy();
                HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
                _responseStream = _response.GetResponseStream();
                return _responseStream;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData"></param>
        /// <returns>Html</returns>
        public string HttpUploadFile(string url, string file, NameValueCollection postData)
        {
            return HttpUploadFile(url, file, postData, Encoding.UTF8);
        }

        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string file, NameValueCollection postData, Encoding encoding)
        {
            return HttpUploadFile(url, new string[] { file }, postData, encoding);
        }

        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <returns>Html</returns>
        public string HttpUploadFile(string url, string[] files, NameValueCollection postData)
        {
            return HttpUploadFile(url, files, postData, Encoding.UTF8);
        }

        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string HttpUploadFile(string url, string[] files, NameValueCollection postData, Encoding encoding)
        {
            string _boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] _boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + _boundary + "\r\n");
            byte[] _endbytes = Encoding.ASCII.GetBytes("\r\n--" + _boundary + "--\r\n");
            //1.HttpWebRequest
            HttpWebRequest _request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            _request.ContentType = "multipart/form-data; boundary=" + _boundary;
            _request.Method = "POST";
            _request.KeepAlive = true;
            _request.Accept = this.accept;
            _request.Timeout = this.time;
            _request.AllowAutoRedirect = this.allowAutoRedirect;

            if (cookie != null)
                _request.CookieContainer = cookie;

            _request.Credentials = CredentialCache.DefaultCredentials;
            using (Stream stream = _request.GetRequestStream())
            {
                //1.1 key/value
                string _formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                if (postData != null)
                {
                    foreach (string key in postData.Keys)
                    {
                        stream.Write(_boundarybytes, 0, _boundarybytes.Length);
                        string formitem = string.Format(_formdataTemplate, key, postData[key]);
                        byte[] formitembytes = encoding.GetBytes(formitem);
                        stream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                //1.2 file
                string _headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                byte[] _buffer = new byte[4096];
                int _bytesRead = 0;

                for (int i = 0; i < files.Length; i++)
                {
                    stream.Write(_boundarybytes, 0, _boundarybytes.Length);
                    string _header = string.Format(_headerTemplate, "file" + i, Path.GetFileName(files[i]));
                    byte[] _headerbytes = encoding.GetBytes(_header);
                    stream.Write(_headerbytes, 0, _headerbytes.Length);
                    using (FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                    {
                        while ((_bytesRead = fileStream.Read(_buffer, 0, _buffer.Length)) != 0)
                        {
                            stream.Write(_buffer, 0, _bytesRead);
                        }
                    }
                }

                //1.3 form end
                stream.Write(_endbytes, 0, _endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            using (StreamReader stream = new StreamReader(_response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }
    }
}