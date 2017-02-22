using ProtoBuf;
using System.IO;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet.ProtoBuf.Utilities
{
    /// <summary>
    /// 利用ProtoBuf序列化与反序列化对象
    /// </summary>
    public class ProtoBufHelper
    {
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
        /// </summary>
        /// <param name="value">需要序列化对象</param>
        /// <param name="path">bin文件存储路径</param>
        public static void Serialize(object value, string path)
        {
            ValidateOperator.Begin().NotNull(value, "需要序列化对象").IsFilePath(path);
            
            using(var file = File.Create(path))
            {
                Serializer.Serialize(file, value);
            }
        }
    }
}