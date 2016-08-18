namespace YanZhiwei.DotNet3._5.Utilities.Common
{
    using DotNet2.Utilities.Common;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Script.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public static class SerializeHelper
    {
        #region Methods

        /// <summary>
        /// 将使用二进制格式保存的byte数组反序列化成对象
        /// </summary>
        /// <param name="deserializeBuffer">byte数组</param>
        /// <returns>对象</returns>
        public static T BinaryDeserialize<T>(byte[] deserializeBuffer)
        {
            ValidateHelper.Begin().NotNull(deserializeBuffer, "deserializeBuffer");
            using(MemoryStream stream = new MemoryStream(deserializeBuffer))
            {
                BinaryFormatter _binarySerializer = new BinaryFormatter();
                return (T)_binarySerializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 将对象使用二进制格式序列化成byte数组
        /// </summary>
        /// <param name="serializeData">需要序列化对象</param>
        /// <returns>byte数组</returns>
        public static byte[] BinarySerialize<T>(T serializeData)
        {
            CheckedSerializeData(serializeData);
            using(MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter _binarySerializer = new BinaryFormatter();
                _binarySerializer.Serialize(stream, serializeData);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 利用DataContractSerializer反序列化
        /// </summary>
        /// <param name="deserializeString">需要反序列化字符串</param>
        /// <returns>object</returns>
        public static T DataContractDeserialize<T>(string deserializeString)
        {
            CheckedDeserializeString(deserializeString);
            using(MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(deserializeString)))
            {
                DataContractSerializer _dataContractSerializer = new DataContractSerializer(typeof(T));
                return (T)_dataContractSerializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// 利用DataContractSerializer对象序列化
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <returns>字符串</returns>
        public static string DataContractSerialize<T>(T serializeData)
        {
            CheckedSerializeData(serializeData);
            using(MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer _dataContractSerializer = new DataContractSerializer(typeof(T));
                _dataContractSerializer.WriteObject(stream, serializeData);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 利用JavaScriptSerializer将json字符串反序列化
        /// </summary>
        /// <param name="deserializeString">Json字符串</param>
        /// <returns>object</returns>
        public static T JsonDeserialize<T>(string deserializeString)
        {
            CheckedDeserializeString(deserializeString);
            JavaScriptSerializer _jsonHelper = new JavaScriptSerializer();
            return (T)_jsonHelper.DeserializeObject(deserializeString);
        }

        /// <summary>
        /// 利用JavaScriptSerializer将对象序列化成JSON字符串
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="scriptConverters">JavaScriptConverter</param>
        /// <returns>Json字符串</returns>
        public static string JsonSerialize<T>(T serializeData, params JavaScriptConverter[] scriptConverters)
        {
            CheckedSerializeData(serializeData);
            JavaScriptSerializer _jsonHelper = new JavaScriptSerializer();

            if(scriptConverters != null)
            {
                _jsonHelper.RegisterConverters(scriptConverters);
            }

            _jsonHelper.MaxJsonLength = int.MaxValue;
            return _jsonHelper.Serialize(serializeData);
        }

        /// <summary>
        /// 利用JavaScriptSerializer将对象序列化成JSON字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <returns>Json字符串</returns>
        public static string JsonSerialize<T>(T serializeData)
        {
            return JsonSerialize<T>(serializeData);
        }

        /// <summary>
        /// 处理JsonString的时间格式问题【时间格式：yyyy-MM-dd HH:mm:ss】
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>处理好的Json字符串</returns>
        public static string ParseJsonDateTime(this string jsonString)
        {
            return ParseJsonDateTime(jsonString, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 处理JsonString的时间格式问题
        /// <para>eg:ScriptSerializerHelper.ConvertTimeJson(@"[{'getTime':'\/Date(1419564257428)\/'}]", "yyyyMMdd hh:mm:ss");==>[{'getTime':'20141226 11:24:17'}]</para>
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <param name="formart">时间格式化类型</param>
        /// <returns>处理好的Json字符串</returns>
        public static string ParseJsonDateTime(this string jsonString, string formart)
        {
            if(!string.IsNullOrEmpty(jsonString))
            {
                jsonString = Regex.Replace(
                                 jsonString,
                                 @"\\/Date\((\d+)\)\\/",
                                 match =>
                {
                    DateTime _dateTime = new DateTime(1970, 1, 1);
                    _dateTime = _dateTime.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    _dateTime = _dateTime.ToLocalTime();
                    return _dateTime.ToString(formart);
                });
            }

            return jsonString;
        }

        /// <summary>
        /// 利用XmlSerializer来反序列化
        /// </summary>
        /// <param name="deserializeString">需要反序列化的字符串</param>
        /// <returns>对象</returns>
        public static T XmlDeserialize<T>(string deserializeString)
        {
            CheckedDeserializeString(deserializeString);
            XmlSerializer _xmlSerializer = new XmlSerializer(typeof(T));
            StringReader _writer = new StringReader(deserializeString);
            return (T)_xmlSerializer.Deserialize(_writer);
        }

        /// <summary>
        /// 序列化，使用标准的XmlSerializer
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="filename">文件路径</param>
        public static void XmlSerialize<T>(T serializeData, string filename)
        {
            CheckedSerializeData(serializeData);
            ValidateHelper.Begin().IsFilePath(filename, "filename");
            using(FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlSerializer _xmlSerializer = new XmlSerializer(serializeData.GetType());
                _xmlSerializer.Serialize(stream, serializeData);
            }
        }

        /// <summary>
        /// 序列化，使用标准的XmlSerializer
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <returns>xml字符串</returns>
        public static string XmlSerialize<T>(T serializeData)
        {
            CheckedSerializeData(serializeData);
            XmlSerializer _xmlSerializer = new XmlSerializer(typeof(T));
            StringWriter _writer = new StringWriter();
            _xmlSerializer.Serialize(_writer, serializeData);
            return _writer.ToString();
        }

        /// <summary>
        /// 序列化，使用标准的XmlSerializer
        /// 不能序列化IDictionary接口.
        /// </summary>
        /// <param name="serializeData">需要序列化的对象</param>
        /// <param name="ecoding">xml字符串</param>
        /// <returns>xml字符串</returns>
        public static string XmlSerialize<T>(T serializeData, Encoding ecoding)
        {
            CheckedSerializeData(serializeData);
            using(MemoryStream stream = new MemoryStream())
            {
                XmlSerializer _serializer = new XmlSerializer(typeof(T));
                StreamWriter _writer = new StreamWriter(stream, ecoding);
                XmlSerializerNamespaces _xsn = new XmlSerializerNamespaces();
                _xsn.Add(string.Empty, string.Empty);
                _serializer.Serialize(_writer, serializeData, _xsn);
                return ecoding.GetString(stream.ToArray());
            }
        }

        private static void CheckedDeserializeString(string deserializeString)
        {
            ValidateHelper.Begin().NotNull(deserializeString, "deserializeString");
        }

        private static void CheckedSerializeData<T>(T serializeData)
        {
            ValidateHelper.Begin().NotNull(serializeData, "serializeData");
        }

        #endregion Methods
    }
}