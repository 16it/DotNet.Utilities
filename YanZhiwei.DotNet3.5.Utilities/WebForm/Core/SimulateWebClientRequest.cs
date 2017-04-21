namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Core
{
    using System.Collections.Specialized;
    using System.Text;

    using YanZhiwei.DotNet3._5.Utilities.Common;
    using YanZhiwei.DotNet3._5.Utilities.Enum;
    using YanZhiwei.DotNet3._5.Utilities.WebClient;

    /// <summary>
    /// 利用WebClient模拟Get，Post请求
    /// </summary>
    public sealed class SimulateWebClientRequest
    {
        #region Methods

        /// <summary>
        /// 向远程Url Post数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <param name="header">Header数据</param>
        /// <param name="serializationType">post数据以及返回数据序列号处理方式</param>
        /// <returns>返回数据</returns>
        public static string Post(string url, object postData, NameValueCollection header, SerializationType serializationType)
        {
            string _serializaPostDataString = SerializePostDataObject(postData, serializationType);

            using (CNNWebClient _webClient = new CNNWebClient())
            {
                _webClient.Timeout = 300;
                if (header != null)
                    _webClient.Headers.Add(header);

                byte[] _responseBuffer = _webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(_serializaPostDataString));

                return _responseBuffer != null ? Encoding.UTF8.GetString(_responseBuffer) : string.Empty;
            }
        }

        /// <summary>
        /// 向远程Url Post数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <param name="serializationType">反序列化方式，支持Json,XML</param>
        /// <returns>反序列化结果</returns>
        public static T Post<T>(string url, object postData, SerializationType serializationType)
            where T : class
        {
            string _responeString = Post(url, postData, null, serializationType);
            return DeserializeResponeString<T>(_responeString, serializationType);
        }

        /// <summary>
        ///  向远程Url Post数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="serializationType">post数据</param>
        /// <returns></returns>
        public static T Post<T>(string url, SerializationType serializationType)
            where T : class
        {
            string _responeString = Post(url, new NameValueCollection(), null, serializationType);
            return DeserializeResponeString<T>(_responeString, serializationType);
        }

        /// <summary>
        /// 向远程Url Post数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <param name="header">Header数据</param>
        /// <param name="serializationType">post数据以及返回数据序列号处理方式</param>
        /// <returns>返回数据</returns>
        public static T Post<T>(string url, object postData, NameValueCollection header, SerializationType serializationType)
            where T : class
        {
            string _responeString = Post(url, postData, header, serializationType);
            return DeserializeResponeString<T>(_responeString, serializationType);
        }

        private static T DeserializeResponeString<T>(string responeString, SerializationType serializationType)
            where T : class
        {
            if (!string.IsNullOrEmpty(responeString))
            {
                return serializationType == SerializationType.Xml ? SerializeHelper.XmlDeserialize<T>(responeString) : SerializeHelper.JsonDeserialize<T>(responeString);
            }
            else
            {
                return null;
            }
        }

        private static string SerializePostDataObject(object postData, SerializationType serializationType)
        {
            string _serializaPostDataString = string.Empty;
            if (postData != null)
            {
                if (postData is string)
                {
                    _serializaPostDataString = (string)postData;
                }
                else
                {
                    _serializaPostDataString = serializationType == SerializationType.Xml ? SerializeHelper.XmlSerialize(postData) : SerializeHelper.JsonSerialize(postData);
                }
            }
            return _serializaPostDataString;
        }

        #endregion Methods

        #region Other

        /*
         *参考
         *1. http://www.cnblogs.com/sunkaixuan/p/4992576.html

         *知识
          1. get方法传输的数据的写法： Request.QueryString["name"];
          2. post方法传输的数据的写法:Request.Form["name"];
          3. get和post方法传送数据的代码写法:Request.Params["name"];
          4. get是从服务器上获取数据，post是向服务器传送数据;
          5. get是把参数数据队列加到提交表单的ACTION属性所指的URL中，值和表单内各个字段一一对应，在URL中可以看到。post是通过HTTP post机制，将表单内各个字段与其内容放置在HTML HEADER内一起传送到ACTION属性所指的URL地址。用户看不到这个过程。
          6. 对于get方式，服务器端用Request.QueryString获取变量的值，对于post方式，服务器端用Request.Form获取提交的数据。
          7. get传送的数据量较小，不能大于2KB。post传送的数据量较大，一般被默认为不受限制。但理论上，IIS4中最大量为80KB，IIS5中为100KB。
          8. get安全性非常低，post安全性较高。但是执行效率却比Post方法好。
          9. get方式的安全性较Post方式要差些，包含机密信息的话，建议用Post数据提交方式；
          10.在做数据查询时，建议用Get方式；而在做数据添加、修改或删除时，建议用Post方式；
          */

        #endregion Other
    }
}