using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
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
        private const string accept = "*/*";
        
        /// <summary>
        /// 是否允许重定向
        /// </summary>
        private const bool allowAutoRedirect = true;
        
        /// <summary>
        /// contentType
        /// </summary>
        private const string contentType = "application/x-www-form-urlencoded";
        
        /// <summary>
        /// 过期时间
        /// </summary>
        private const int timeOut = 5000;
        
        #endregion Fields
        
        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="header">Headers</param>
        /// <returns>结果</returns>
        public static T Post<T>(string url, NameValueCollection header)
        {
            HttpWebRequest _request = WebRequest.Create(url) as HttpWebRequest;
            _request.Method = "POST";
            _request.Timeout = timeOut;
            _request.AllowAutoRedirect = allowAutoRedirect;
            _request.ServicePoint.ConnectionLimit = int.MaxValue;
            _request.ContentLength = 0;
            
            if(header != null)
                _request.Headers.Add(header);
                
            using(HttpWebResponse response = (HttpWebResponse)_request.GetResponse())
            {
                using(StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string _responeString = reader.ReadToEnd();
                    return SerializeHelper.JsonDeserialize<T>(_responeString);
                }
            }
        }
        
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData"></param>
        /// <returns>Html</returns>
        public static string UploadFile(string url, string file, NameValueCollection postData)
        {
            return UploadFile(url, file, postData, Encoding.UTF8);
        }
        
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="file">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string UploadFile(string url, string file, NameValueCollection postData, Encoding encoding)
        {
            return UploadFile(url, new string[] { file }, postData, encoding);
        }
        
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <returns>Html</returns>
        public static string UploadFile(string url, string[] files, NameValueCollection postData)
        {
            return UploadFile(url, files, postData, Encoding.UTF8);
        }
        
        /// <summary>
        /// POST文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="files">文件路径</param>
        /// <param name="postData">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string UploadFile(string url, string[] files, NameValueCollection postData, Encoding encoding)
        {
            string _boundarynumber = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] _boundarybuffer = Encoding.ASCII.GetBytes("\r\n--" + _boundarynumber + "\r\n");
            byte[] _allRequestbuffer = Encoding.ASCII.GetBytes("\r\n--" + _boundarynumber + "--\r\n");
            HttpWebRequest _request = CreateUploadFileWebRequest(url, _boundarynumber);
            
            using(Stream requestStream = _request.GetRequestStream())
            {
                BuilderUploadFilePostParamter(requestStream, _boundarybuffer, postData, encoding);
                FetchUploadFiles(requestStream, _boundarybuffer, files, encoding, _allRequestbuffer);
            }
            
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            
            using(StreamReader stream = new StreamReader(_response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }
        
        private static void FetchUploadFiles(Stream requestStream, byte[] boundarybuffer, string[] files, Encoding encoding, byte[] allRequestBuffer)
        {
            string _headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            byte[] _buffer = new byte[4096];
            int _bytesRead = 0;
            
            for(int i = 0; i < files.Length; i++)
            {
                requestStream.Write(boundarybuffer, 0, boundarybuffer.Length);
                string _header = string.Format(_headerTemplate, "file" + i, Path.GetFileName(files[i]));
                byte[] _headerbytes = encoding.GetBytes(_header);
                requestStream.Write(_headerbytes, 0, _headerbytes.Length);
                
                using(FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    while((_bytesRead = fileStream.Read(_buffer, 0, _buffer.Length)) != 0)
                    {
                        requestStream.Write(_buffer, 0, _bytesRead);
                    }
                }
            }
            
            requestStream.Write(allRequestBuffer, 0, allRequestBuffer.Length);
        }
        
        private static void BuilderUploadFilePostParamter(Stream requestStream, byte[] boundarybuffer, NameValueCollection postData, Encoding encoding)
        {
            string _formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            
            if(postData != null)
            {
                foreach(string key in postData.Keys)
                {
                    requestStream.Write(boundarybuffer, 0, boundarybuffer.Length);
                    string _formitem = string.Format(_formdataTemplate, key, postData[key]);
                    byte[] _formitembuffer = encoding.GetBytes(_formitem);
                    requestStream.Write(_formitembuffer, 0, _formitembuffer.Length);
                }
            }
        }
        
        private static HttpWebRequest CreateUploadFileWebRequest(string url, string boundarynumber)
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            _request.ContentType = "multipart/form-data; boundary=" + boundarynumber;
            _request.Method = "POST";
            _request.KeepAlive = true;
            _request.Accept = accept;
            _request.Timeout = timeOut;
            _request.AllowAutoRedirect = allowAutoRedirect;
            _request.Credentials = CredentialCache.DefaultCredentials;
            return _request;
        }
    }
}