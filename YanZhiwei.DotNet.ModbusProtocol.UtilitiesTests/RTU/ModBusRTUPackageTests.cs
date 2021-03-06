﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Tests
{
    [TestClass()]
    public class ModBusRTUPackageTests
    {
        [TestMethod()]
        public void ModBusRTUPackageTest()
        {
            #region 单个寄存器写入

            byte[] _expectWriteSingleRegister = { 0x02, 0x06, 0x00, 0x01, 0x00, 0x03, 0x98, 0x38 };
            WriteSingleRegisterData _writeSingleRegisterData = new WriteSingleRegisterData(0x02, 1, 0x03);
            ModBusRTUPackage _writeSingleRegister = new ModBusRTUPackage(_writeSingleRegisterData);
            byte[] _actualWriteSingleRegister = _writeSingleRegister.ToArray();
            CollectionAssert.AreEqual(_expectWriteSingleRegister, _actualWriteSingleRegister);

            #endregion 单个寄存器写入

            #region 单个线圈写入

            byte[] _expectWriteSingleCoil = { 0x02, 0x05, 0x00, 0x01, 0xFF, 0x00, 0xDD, 0xC9 };
            WriteSingleCoilData _writeSingleCoilData = new WriteSingleCoilData(0x02, 1, true);
            ModBusRTUPackage _writeSingleCoil = new ModBusRTUPackage(_writeSingleCoilData);
            byte[] _actualWriteSingleCoil = _writeSingleCoil.ToArray();
            CollectionAssert.AreEqual(_expectWriteSingleCoil, _actualWriteSingleCoil);

            #endregion 单个线圈写入

            #region 多个线圈写入

            byte[] _expectWriteMultipleCoil = { 0x02, 0x0F, 0x00, 0x01, 0x00, 0x0A, 0x02, 0xFF, 0x01, 0x70, 0x29 };
            BitArray _coilsValue = new BitArray(new bool[10] { true, true, true, true, true, true, true, true, true, false });
            WriteMultipleCoilsData _writeMultipleCoilData = new WriteMultipleCoilsData(0x02, 1, _coilsValue);
            ModBusRTUPackage _writeMultipleCoil = new ModBusRTUPackage(_writeMultipleCoilData);
            byte[] _actualWriteMultipleCoil = _writeMultipleCoil.ToArray();
            CollectionAssert.AreEqual(_expectWriteMultipleCoil, _actualWriteMultipleCoil);

            #endregion 多个线圈写入

            #region 多个寄存器写入

            byte[] _expectWriteMultipleRegister = { 0x02, 0x10, 0x00, 0x01, 0x00, 0x01, 0x02, 0xFF, 0x01, 0x33, 0x41 };
            short[] _registersValue = { -255 };
            WriteMultipleRegisterData _writeWriteMultipleRegisterData = new WriteMultipleRegisterData(0x02, 1, _registersValue);
            ModBusRTUPackage _writeMultipleRegister = new ModBusRTUPackage(_writeWriteMultipleRegisterData);
            byte[] _actualWriteMultipleRegister = _writeMultipleRegister.ToArray();
            CollectionAssert.AreEqual(_expectWriteMultipleRegister, _actualWriteMultipleRegister);

            #endregion 多个寄存器写入
        }

        [TestMethod()]
        public void ModBusRTUPackageTest1()
        {
            byte[] _expectReadSingleCoil = { 0x02, 0x01, 0x00, 0x01, 0x00, 0x0A, 0xED, 0xFE };
            ReadCoilsData _readCoilsData = new ReadCoilsData(0x02, 1, 10);
            ModBusRTUPackage _readCoils = new ModBusRTUPackage(_readCoilsData);
            byte[] _actualReadCoils = _readCoils.ToArray();
            CollectionAssert.AreEqual(_expectReadSingleCoil, _actualReadCoils);
        }
    }
}