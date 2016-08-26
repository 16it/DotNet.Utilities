using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Encryptor;

namespace YanZhiwei.DotNet2.Utilities.DataOperator.Tests
{
    [TestClass()]
    public class MD5EncryptHelperTests
    {
        public void EqualsRandomMD5Test()
        {
            string _data = "yanzhiwei";
            Assert.IsTrue(_data.EqualsRandomMD5(_data.ToRandomMD5()));
        }
    }
}