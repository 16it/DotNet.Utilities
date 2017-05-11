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
                return "yanzhiweizhuzhouhunanchina";
            }
        }
        
        /// <summary>
        /// 下载文件名称加密偏移向量
        /// </summary>
        public static byte[] FileNameEncryptorIv
        {
            get
            {
                return new byte[16] { 0x01, 0x02, 0x03, 0x4, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x4, 0x05, 0x06, 0x07, 0x08 };
            }
        }
        
        /// <summary>
        /// 限制的下载速度Kb
        /// </summary>
        public static ulong LimitDownloadSpeedKb
        {
            get
            {
                return 100;
            }
        }
        
        /// <summary>
        /// 文件下载的文件夹目录
        /// </summary>
        public static string DownLoadMainDirectory
        {
            get;
            set;
        }
    }
}