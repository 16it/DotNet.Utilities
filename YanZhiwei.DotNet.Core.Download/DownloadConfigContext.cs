using YanZhiwei.DotNet.Core.Config;
using YanZhiwei.DotNet.Core.Config.Model;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.Core.Download
{
    /// <summary>
    /// 文件下载的配置
    /// </summary>
    internal class DownloadConfigContext
    {
        private static readonly object syncRoot = new object();

        /// <summary>
        /// 文件下载配置
        /// </summary>
        public static DownloadConfig downloadConfig = CachedConfigContext.Instance.DownloadConfig;

        private static string fileNameEncryptorKey = null;

        /// <summary>
        /// 下载文件名称加密Key
        /// </summary>
        public static string FileNameEncryptorKey
        {
            get
            {
                if (string.IsNullOrEmpty(fileNameEncryptorKey))
                {
                    lock (syncRoot)
                    {
                        if (string.IsNullOrEmpty(fileNameEncryptorKey))
                        {
                            fileNameEncryptorKey = downloadConfig.FileNameEncryptorKey ?? "dotnetDownloadHanlder";
                        }
                    }
                }

                return fileNameEncryptorKey;
            }
        }

        private static byte[] fileNameEncryptorIv = null;

        /// <summary>
        /// 下载文件名称加密偏移向量
        /// </summary>
        public static byte[] FileNameEncryptorIv
        {
            get
            {
                if (fileNameEncryptorIv == null)
                {
                    lock (syncRoot)
                    {
                        if (fileNameEncryptorIv == null)
                        {
                            fileNameEncryptorIv = ByteHelper.ParseHexString(downloadConfig.FileNameEncryptorIvHexString);
                        }
                    }
                }

                return fileNameEncryptorIv;
            }
        }

        /// <summary>
        /// 限制的下载速度Kb
        /// </summary>
        public static ulong LimitDownloadSpeedKb
        {
            get
            {
                return downloadConfig.LimitDownloadSpeedKb;
            }
        }

        /// <summary>
        /// 文件下载的文件夹目录
        /// </summary>
        public static string DownLoadMainDirectory
        {
            get
            {
                return downloadConfig.DownLoadMainDirectory;
            }
        }
    }
}