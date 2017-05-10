using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using YanZhiwei.DotNet3._5.Utilities.Encryptor;

namespace YanZhiwei.DotNet3._5.UtilitiesTests.Common
{
    [TestClass()]
    public class AESEncryptHelperTests
    {
        private AESEncryptor aesHelper = null;

        [TestInitialize]
        public void Init()
        {
            Aes _newAES = AESEncryptor.CreateAES("yanzhiweizhuzhouhunanchina");
            _newAES.IV = new byte[16] { 0x01, 0x02, 0x03, 0x4, 0x05, 0x06, 0x07, 0x08, 0x01, 0x02, 0x03, 0x4, 0x05, 0x06, 0x07, 0x08 };
            aesHelper = new AESEncryptor(_newAES.Key, _newAES.IV);
        }

        [TestMethod()]
        public void EncryptTest()
        {
            string _actual = aesHelper.Encrypt("YanZhiwei");
            Assert.AreEqual("v4M1o7AhQ4EOVLxbs4ZIzQ==", _actual);
        }

        [TestMethod()]
        public void DecryptTest()
        {
            string _actual = aesHelper.Decrypt("v4M1o7AhQ4EOVLxbs4ZIzQ==");
            Assert.AreEqual("YanZhiwei", _actual);
        }
    }
}