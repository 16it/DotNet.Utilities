using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Tests
{
    [TestClass()]
    public class RTUModBusUnPackageTests
    {
        [TestMethod()]
        public void BuilderObjFromBytesTest()
        {
            byte[] _orderCmd0x01 = { 0x02, 0x01, 0x01, 0x01, 0x90, 0x0C };
            RTUModBusUnPackage _unpackageOrderCmd0x01 = new RTUModBusUnPackage();
            UnPackageError _unpackageError = UnPackageError.Normal;
            _unpackageOrderCmd0x01.BuilderObjFromBytes(_orderCmd0x01, out _unpackageError);
            Assert.AreEqual(_unpackageOrderCmd0x01.OrderCmd, 0x01);
        }
    }
}