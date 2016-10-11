using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet.EntLib4.Utilities.Tests
{
    [TestClass()]
    public class EntLibCryptographyEncryptorTests
    {
        [TestMethod()]
        public void EncryptSymmetricTest()
        {
            string _actual = EntLibCryptographyEncryptor.EncryptSymmetric("TripleDESCryptoServiceProvider", "yanzhiwei");
            Assert.AreEqual(EntLibCryptographyEncryptor.DecryptSymmetric("TripleDESCryptoServiceProvider", _actual), "yanzhiwei");
        }
        
        [TestMethod()]
        public void CreateHashTest()
        {
            string _actual = EntLibCryptographyEncryptor.CreateHash("SHA1Managed", "YanZhiwei");
            bool _succesd = EntLibCryptographyEncryptor.CompareHash("SHA1Managed", "YanZhiwei", _actual);
            Assert.IsTrue(_succesd);
        }
    }
}