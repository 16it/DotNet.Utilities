using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Tests
{
    [TestClass()]
    public class ModBusRTUPackageTests
    {
        [TestMethod()]
        public void ModBusRTUPackageTest()
        {
            byte[] _expectWriteSingleRegister = { 0x02, 0x06, 0x00, 0x01, 0x00, 0x03, 0x98, 0x38 };
            WriteSingleRegisterData _writeSingleRegisterData = new WriteSingleRegisterData(0x02, 1, 0x03);
            ModBusRTUPackage _writeSingleRegister = new ModBusRTUPackage(_writeSingleRegisterData);
            byte[] _actualWriteSingleRegister = _writeSingleRegister.ToArray();
            CollectionAssert.AreEqual(_expectWriteSingleRegister, _actualWriteSingleRegister);

            byte[] _expectWriteSingleCoil = { 0x02, 0x05, 0x00, 0x01, 0xFF, 0x00, 0xDD, 0xC9 };
            WriteSingleCoilData _writeSingleCoilData = new WriteSingleCoilData(0x02, 1, true);
            ModBusRTUPackage _writeSingleCoil = new ModBusRTUPackage(_writeSingleCoilData);
            byte[] _actualWriteSingleCoil = _writeSingleCoil.ToArray();
            CollectionAssert.AreEqual(_expectWriteSingleCoil, _actualWriteSingleCoil);

        }
    }
}