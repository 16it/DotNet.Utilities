namespace YanZhiwei.DotNet3._5.Utilities.WebForm
{
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    
    using Common;
    using Enum;
    using WebClient;
    
    /// <summary>
    /// 向远程Url Post/Get数据类
    /// </summary>
    public class NetHelper
    {
        #region Methods
        
        /// <summary>
        /// 向远程Url Get数据类
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="uri">请求URL地址</param>
        /// <param name="serializationType">序列化方式</param>
        /// <returns>反序列化对象</returns>
        public static T HttpGet<T>(string uri, SerializationType serializationType)
        {
            string _responseText = HttpGet(uri);
            T _t = default(T);
            
            if(serializationType == SerializationType.Xml)
            {
                _t = SerializeHelper.XmlDeserialize<T>(_responseText);
            }
            else if(serializationType == SerializationType.Json)
            {
                _t = (T)SerializeHelper.JsonDeserialize<T>(_responseText);
            }
            
            return _t;
        }
        
        /// <summary>
        /// 向远程Url Get数据类
        /// </summary>
        /// <param name="uri">请求URL地址</param>
        /// <returns>响应字符串</returns>
        public static string HttpGet(string uri)
        {
            StringBuilder _responeBuilder = new StringBuilder();
            HttpWebRequest _request = HttpWebRequest.Create(uri) as HttpWebRequest;
            _request.Method = "GET";
            _request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse _response = _request.GetResponse() as HttpWebResponse;
            byte[] _buffer = new byte[8192];
            using(Stream stream = _response.GetResponseStream())
            {
                int _count = 0;
                
                do
                {
                    _count = stream.Read(_buffer, 0, _buffer.Length);
                    
                    if(_count != 0)
                        _responeBuilder.Append(Encoding.UTF8.GetString(_buffer, 0, _count));
                }
                while(_count > 0);
            }
            return _responeBuilder.ToString();
        }
        
        /// <summary>
        /// 向远程Url Post数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="uri">请求URL地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="serializationType">序列化方式</param>
        /// <returns>反序列化对象</returns>
        public static T HttpPost<T>(string uri, object data, SerializationType serializationType)
        {
            string _responseText = HttpPost(uri, data, serializationType);
            T _t = default(T);
            
            if(serializationType == SerializationType.Xml)
            {
                _t = SerializeHelper.XmlDeserialize<T>(_responseText);
            }
            else if(serializationType == SerializationType.Json)
            {
                _t = SerializeHelper.JsonDeserialize<T>(_responseText);
            }
            
            return _t;
        }
        
        /// <summary>
        /// 向远程Url Post数据
        /// </summary>
        /// <param name="uri">请求URL地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="serializationType">序列化方式</param>
        /// <returns>响应字符串</returns>
        public static string HttpPost(string uri, object data, SerializationType serializationType)
        {
            HttpWebRequest _request = HttpWebRequest.Create(uri) as HttpWebRequest;
            _request.Method = "POST";
            _request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            string _requestParamter = string.Empty;
            
            if(data is string)
            {
                _requestParamter = (string)data;
            }
            else
            {
                if(serializationType == SerializationType.Xml)
                {
                    _requestParamter = SerializeHelper.XmlSerialize(data);
                }
                else if(serializationType == SerializationType.Json)
                {
                    _requestParamter = SerializeHelper.JsonSerialize(data);
                }
            }
            
            CNNWebClient _webClient = new CNNWebClient();
            _webClient.Timeout = 300;
            byte[] _responeBuffer = _webClient.UploadData(uri, "POST", Encoding.UTF8.GetBytes(_requestParamter));
            return Encoding.UTF8.GetString(_responeBuffer);
        }
        
        /// <summary>
        /// 向远程Url Post数据
        /// </summary>
        /// <param name="uri">请求URL地址</param>
        /// <param name="data">请求参数</param>
        /// <returns>响应字符串</returns>
        public static string HttpPost(string uri, NameValueCollection data)
        {
            CNNWebClient _webClient = new CNNWebClient();
            _webClient.Encoding = Encoding.UTF8;
            _webClient.Timeout = 300;
            byte[] _responeBuffer = _webClient.UploadValues(uri, "POST", data);
            return Encoding.UTF8.GetString(_responeBuffer);
        }
        
        #endregion Methods
    }
}