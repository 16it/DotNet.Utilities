using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet2.Utilities.Builder.Tests
{
    [TestClass()]
    public class CRCBuilderTests
    {
        [TestMethod()]
        public void Create16MODBUSTest()
        {
            byte[] _testData = new byte[16] { 0x05, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x86, 0x80, 0x00, 0x02, 0xFF, 0xFF };
            byte[] _expect = new byte[2] { 0x3C, 0xA4 };
            byte[] _actual = ByteHelper.ToBytes(CRCBuilder.Create16MODBUS(_testData), false);
            CollectionAssert.AreEqual(_expect, _actual);
        }
    }
}