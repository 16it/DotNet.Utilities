namespace YanZhiwei.DotNet2.Utilities.Core
{
    using Common;
    using DataOperator;
    using System;
    using System.IO;

    /// <summary>
    /// Byte数组构建器
    /// </summary>
    /// 时间：2016/6/15 14:01
    /// 备注：
    public class ByteArrayBuilder : IDisposable
    {
        #region Fields

        /// <summary>
        /// False
        /// </summary>
        private const byte streamFalse = (byte)0;

        /// <summary>
        /// True
        /// </summary>
        private const byte streamTrue = (byte)1;

        /// <summary>
        /// MemoryStream
        /// </summary>
        private MemoryStream store = new MemoryStream();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// 时间：2016/6/15 14:02
        /// 备注：
        public ByteArrayBuilder()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">初始化byte数组</param>
        public ByteArrayBuilder(byte[] data)
        {
            store.Close();
            store.Dispose();
            store = new MemoryStream(data);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="base64">base64字符串</param>
        /// 时间：2016/6/15 14:03
        /// 备注：
        public ByteArrayBuilder(string base64)
        {
            store.Close();
            store.Dispose();
            store = new MemoryStream(Convert.FromBase64String(base64));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 当前Byte数组长度
        /// </summary>
        public int Length
        {
            get
            {
                return (int)store.Length;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 附加byte数组类型数值
        /// </summary>
        /// <param name="b">byte数组类型数值</param>
        public void Append(byte[] b)
        {
            store.Write(b, 0, b.Length);
        }

        /// <summary>
        /// 附加一个bool数值
        /// </summary>
        /// <param name="b">bool数值</param>
        /// 时间：2016/6/15 14:03
        /// 备注：
        public void Append(bool b)
        {
            store.WriteByte(b ? streamTrue : streamFalse);
        }

        /// <summary>
        /// 附加byte类型数值
        /// </summary>
        /// <param name="b">byte类型数值</param>
        public void Append(byte b)
        {
            store.WriteByte(b);
        }

        /// <summary>
        /// 附加int类型数值
        /// </summary>
        /// <param name="i">int类型数值</param>
        public void Append(int i)
        {
            Append(BitConverter.GetBytes(i));
        }

        /// <summary>
        /// 附加long类型数值
        /// </summary>
        /// <param name="l">long类型数值</param>
        public void Append(long l)
        {
            Append(BitConverter.GetBytes(l));
        }

        /// <summary>
        /// 附加int类型数值
        /// </summary>
        /// <param name="i">int类型数值</param>
        public void Append(short i)
        {
            Append(BitConverter.GetBytes(i));
        }

        /// <summary>
        /// 附加uint类型数值
        /// </summary>
        /// <param name="ui">uint类型数值</param>
        public void Append(uint ui)
        {
            Append(BitConverter.GetBytes(ui));
        }

        /// <summary>
        /// 附加ulong类型数值
        /// </summary>
        /// <param name="ul">ulong类型数值</param>
        public void Append(ulong ul)
        {
            Append(BitConverter.GetBytes(ul));
        }

        /// <summary>
        /// 附加ushort类型数值
        /// </summary>
        /// <param name="us">ushort类型数值</param>
        public void Append(ushort us)
        {
            Append(BitConverter.GetBytes(us));
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            store.Close();
            store.Dispose();
            store = new MemoryStream();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// 时间：2016/6/20 15:01
        /// 备注：
        public void Dispose()
        {
            store.Close();
            store.Dispose();
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <returns>数组</returns>
        /// 时间：2016/6/20 15:01
        /// 备注：
        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            Array.Copy(store.GetBuffer(), data, Length);
            return data;
        }

        /// <summary>
        /// 返回带空格的十六进制字符串
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// 时间：2016/6/20 14:59
        /// 备注：
        public override string ToString()
        {
            return ByteHelper.ToHexStringWithBlank(ToArray());
        }

        #endregion Methods
    }
}