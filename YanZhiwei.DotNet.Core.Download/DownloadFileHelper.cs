using System.Security.Cryptography;
using YanZhiwei.DotNet2.Utilities.DesignPattern;
using YanZhiwei.DotNet3._5.Utilities.Encryptor;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet.Core.Download
{
    /// <summary>
    /// 下载文件加密解密辅助类
    /// </summary>
    public class DownloadFileHelper
    {
        private static AESEncryptor fileEncryptorHelper = null;

        /// <summary>
        /// 获取对象实例
        /// </summary>
        public static DownloadFileHelper Instance
        {
            get
            {
                return Singleton<DownloadFileHelper>.GetInstance();
            }
        }

        static DownloadFileHelper()
        {
            Aes _Aes = AESEncryptor.CreateAES(DownloadConfigContext.FileNameEncryptorKey);
            fileEncryptorHelper = new AESEncryptor(_Aes.Key, DownloadConfigContext.FileNameEncryptorIv);
        }

        /// <summary>
        /// 加密下载文件
        /// </summary>
        /// <param name="fileName">需要下载文件名称</param>
        /// <returns>加密后的文件</returns>
        public string EncryptFileName(string fileName)
        {
            fileName = fileName.FilterSpecial();
            fileName = StringHelper.Escape(fileName);
            return fileEncryptorHelper.Encrypt(fileName);
        }

        /// <summary>
        /// 解密下载文件
        /// </summary>
        /// <param name="encryptFileName">加密下载文件字符串</param>
        /// <returns>原始下载文件名称</returns>
        public string DecryptFileName(string encryptFileName)
        {
            string _fileName = fileEncryptorHelper.Decrypt(encryptFileName);
            return StringHelper.UnEscape(_fileName);
        }
    }
}