namespace YanZhiwei.DotNet.Core.Download
{
    /// <summary>
    /// 文件下载的配置
    /// </summary>
    internal class DownloadConfigContext
    {
        /// <summary>
        /// 下载文件名称加密Key
        /// </summary>
        public static string FileNameEncryptorKey
        {
            get
            {
                return "DotNet.Core.Download";
            }
        }

        /// <summary>
        /// 下载文件名称加密偏移向量
        /// </summary>
        public static byte[] FileNameEncryptorIv
        {
            get
            {
                return new byte[8] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            }
        }
    }
}