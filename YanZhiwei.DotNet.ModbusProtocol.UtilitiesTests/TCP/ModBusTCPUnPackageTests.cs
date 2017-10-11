using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Tests
{
    [TestClass()]
    public class ModBusTCPUnPackageTests
    {
        [TestMethod()]
        public void BuilderObjFromBytesTest()
        {
            byte[] _orderCmd0x01 = { 0x00, 0x77, 0x00, 0x00, 0x00, 0x05, 0x02, 0x01, 0x02, 0x1F, 0x00 };
            ModBusTCPUnPackage _unpackageOrderCmd0x01 = new ModBusTCPUnPackage();
            SlaveReplyDataBase _slaveReplyData = null;
            UnPackageError _unpackageError = _unpackageOrderCmd0x01.BuilderObjFromBytes(_orderCmd0x01, out _slaveReplyData);
            Assert.AreEqual(_slaveReplyData.OrderCmdCode, 0x01);
        }
    }
}