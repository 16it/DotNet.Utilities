using YanZhiwei.DotNet2.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.Common.Tests
{
    [TestClass()]
    public class HexHelperTests
    {
        [TestMethod()]
        public void ToUShortTest()
        {
            ushort _expected = 255;
            Assert.AreEqual(_expected, HexHelper.ToUShort("ff"));
        }
        
        [TestMethod()]
        public void ToUIntTest()
        {
            uint _expected = 65535;
            Assert.AreEqual(_expected, HexHelper.ToUShort("ffff"));
        }
        
        [TestMethod()]
        public void ToULongTest()
        {
            ulong _expected = 65535;
            Assert.AreEqual(_expected, HexHelper.ToUShort("ffff"));
        }
        
        [TestMethod()]
        public void ToHexStringTest()
        {
            Assert.AreEqual("FF", HexHelper.ToHexString(255));
            Assert.AreEqual("FFFF", HexHelper.ToHexString(65535));
            Assert.AreEqual("0A", HexHelper.ToHexString(10));
        }
    }
}