namespace YanZhiwei.DotNet.ProtoBuf.Utilities
{
    using global::ProtoBuf;
    using System.IO;
    using YanZhiwei.DotNet2.Utilities.Operator;
    
    /// <summary>
    /// 利用ProtoBuf序列化与反序列化对象
    /// </summary>
    public class ProtoBufHelper
    {
        #region Methods
        
        /// <summary>
        /// 反序列化对象
        ///<para>https://github.com/mgravell/protobuf-net</para>
        /// </summary>
        /// <param name="buffer">二进制流</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(byte[] buffer)
        {
            ValidateOperator.Begin().NotNull(buffer, "需要反序列化二进制流");
            
            using(MemoryStream stream = new MemoryStream(buffer))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }
        
        /// <summary>
        /// 反序列化BIN文件
        ///<para>https://github.com/mgravell/protobuf-net</para>
        /// </summary>
        /// <param name="path">BIN文件路径</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string path)
        {
            ValidateOperator.Begin().IsFilePath(path).CheckFileExists(path);
            
            using(FileStream file = File.OpenRead(path))
            {
                return Serializer.Deserialize<T>(file);
            }
        }
        
        /// <summary>
        /// 序列化二进制流
        ///<para>https://github.com/mgravell/protobuf-net</para>
        /// </summary>
        /// <param name="value">需要序列化对象</param>
        /// <returns>二进制流</returns>
        public static byte[] Serialize(object value)
        {
            ValidateOperator.Begin().NotNull(value, "需要序列化对象");
            byte[] _buffer = null;
            
            using(MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, value);
                _buffer = stream.ToArray();
            }
            
            return _buffer;
        }
        
        /// <summary>
        /// 序列化成BIN文件
        ///<para>https://github.com/mgravell/protobuf-net</para>
        /// </summary>
        /// <param name="value">需要序列化对象</param>
        /// <param name="path">bin文件存储路径</param>
        public static void Serialize<T>(T value, string path)
        {
            ValidateOperator.Begin().NotNull(value, "需要序列化对象").IsFilePath(path);
            
            using(var file = File.Create(path))
            {
                Serializer.Serialize<T>(file, value);
            }
        }
        
        #endregion Methods
    }
}