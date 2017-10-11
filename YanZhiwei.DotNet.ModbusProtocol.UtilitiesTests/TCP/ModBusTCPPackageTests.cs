using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Tests
{
    [TestClass()]
    public class ModBusTCPPackageTests
    {
        [TestMethod()]
        public void ModBusTCPPackageTest()
        {
            byte[] _expectWriteSingleCoil = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x02, 0x05, 0x00, 0x01, 0xFF, 0x00 };
            WriteSingleCoilData _writeSingleCoilData = new WriteSingleCoilData(0x02, 1, true);
            ModBusTCPPackage _writeSingleCoil = new ModBusTCPPackage(_writeSingleCoilData);
            byte[] _actualWriteSingleCoil = _writeSingleCoil.ToArray();
            CollectionAssert.AreEqual(_expectWriteSingleCoil, _actualWriteSingleCoil);
        }
    }
}