using System.Security.Cryptography;
using YanZhiwei.DotNet3._5.Utilities.Encryptor;

namespace YanZhiwei.DotNet.Core.Download
{
    internal class DownloadFileHelper
    {
        private static AESEncryptor fileEncryptorHelper = null;

        static DownloadFileHelper()
        {
            Aes _Aes = AESEncryptor.CreateAES(DownloadConfigContext.FileNameEncryptorKey);
            fileEncryptorHelper = new AESEncryptor(_Aes.Key, DownloadConfigContext.FileNameEncryptorIv);
        }

        public string EncryptFileName(string fileName)
        {
            return fileEncryptorHelper.Encrypt(fileName);
        }

        public string DecryptFileName(string encryptFileName)
        {
            return fileEncryptorHelper.Decrypt(encryptFileName);
        }
    }
}