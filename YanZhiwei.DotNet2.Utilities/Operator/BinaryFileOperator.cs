namespace YanZhiwei.DotNet2.Utilities.Operator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    using Core;
    using Common;

    /// <summary>
    /// 二进制文件序列化与反序列化
    /// </summary>
    /// 时间：2016/8/25 11:47
    /// 备注：
    public class BinaryFileOperator
    {
        #region Methods

        /// <summary>
        /// 将二进制文件反序列化成集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="path">路径</param>
        /// <returns>集合</returns>
        /// 时间：2016/8/25 11:54
        /// 备注：
        public static IEnumerable<T> Deserialize<T>(string path)
        where T : class
        {
            ValidateHelper.Begin().NotNullOrEmpty(path, "二进制文件").IsFilePath(path, "二进制文件").CheckFileExists(path, "二进制文件");
            IFormatter _formatter = new BinaryFormatter();
            _formatter.Binder = new UBinder();
            using(Stream _stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (IEnumerable<T>)_formatter.Deserialize(_stream);
            }
        }

        /// <summary>
        /// 将文件序列化到二进制文件
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">集合</param>
        /// <param name="path">存储路径</param>
        /// 时间：2016/8/25 11:52
        /// 备注：
        public static void Serialize<T>(IEnumerable<T> data, string path)
        where T : class
        {
            ValidateHelper.Begin().NotNull(data, "需要序列化数据集合").NotNullOrEmpty(path, "二进制文件").IsFilePath(path, "二进制文件");
            IFormatter _formatter = new BinaryFormatter();
            using(Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                _formatter.Serialize(stream, data);
            }
        }

        #endregion Methods
    }
}