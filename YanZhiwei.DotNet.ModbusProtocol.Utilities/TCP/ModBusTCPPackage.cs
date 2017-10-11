using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Builder;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    /// <summary>
    /// Modbus Tcp 组包
    /// </summary>
    public sealed class ModBusTCPPackage
    {
        /// <summary>
        /// MODBUS协议报文头
        /// </summary>
        public readonly MBAPHeader ModbusHeader = null;

        public byte[] ModBusAppData { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mbapHeader">MODBUS协议报文头</param>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        public ModBusTCPPackage(MBAPHeader mbapHeader, MasterWriteDataBase masterWriteData)
        {
            ModbusHeader = mbapHeader;
            HanlderWriteSingleCoilData(masterWriteData);
            HanlderWriteSingleRegisterData(masterWriteData);
            HanlderWriteMultipleCoilsData(masterWriteData);
            HanlderWriteMultipleRegisterData(masterWriteData);
        }

        private void HanlderWriteMultipleRegisterData(MasterWriteDataBase masterWriteData)
        {
            if (masterWriteData is WriteMultipleRegisterData)
            {
                //02 10 00 01 00 01 02 FF 01 33 41
                //02--从机地址
                //10--功能码
                //00 01--寄存器地址
                //00 01--寄存器数量
                //02--数据长度
                //FF 01--数据 -255
                //33 41--CRC
                WriteMultipleRegisterData _data = (WriteMultipleRegisterData)masterWriteData;
                using (ByteArrayBuilder builder = new ByteArrayBuilder())
                {
                    builder.Append(_data.SlaveID);//高位在前
                    builder.Append((byte)ModbusBaseOrderCmd.WriteMultipleRegister);//功能码
                    builder.Append(ByteHelper.ToBytes(_data.Address, true));//高位在前
                    builder.Append(ByteHelper.ToBytes(_data.Quantity, true));//数量

                    byte _coilsCount = (byte)(_data.Value.Length * 2);
                    builder.Append(_coilsCount);
                    foreach (short item in _data.Value)
                    {
                        byte[] _registerValue = item.ToBytes(true);
                        builder.Append(_registerValue);
                    }

                    ModBusAppData = builder.ToArray();
                }
            }
        }

        /// <summary>
        /// 处理多个线圈写入
        /// </summary>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        private void HanlderWriteMultipleCoilsData(MasterWriteDataBase masterWriteData)
        {
            if (masterWriteData is WriteMultipleCoilsData)
            {
                //02 0F 00 01 00 0A 02 FF 03 F1 E8
                //02--从机地址
                //0F--功能码
                //00 01--寄存器地址
                //00 0A--寄存器数量
                //02--数据长度
                //FF 03--数据
                //F1 E8--CRC
                WriteMultipleCoilsData _data = (WriteMultipleCoilsData)masterWriteData;
                using (ByteArrayBuilder builder = new ByteArrayBuilder())
                {
                    builder.Append(_data.SlaveID);//高位在前
                    builder.Append((byte)ModbusBaseOrderCmd.WriteMultipleCoils);//功能码
                    builder.Append(ByteHelper.ToBytes(_data.Address, true));//高位在前
                    builder.Append(ByteHelper.ToBytes(_data.Quantity, true));//数量
                    byte[] _coilsValue = _data.ColisStatus.ToBytes();
                    byte _coilsCount = (byte)_coilsValue.Length;
                    builder.Append(_coilsCount);
                    builder.Append(_coilsValue);
                    ModBusAppData = builder.ToArray();
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        public ModBusTCPPackage(MasterWriteDataBase masterWriteData) : this(new StandardMBAPHeader(), masterWriteData)
        {
        }

        /// <summary>
        /// 处理单个线圈写入
        /// </summary>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        private void HanlderWriteSingleCoilData(MasterWriteDataBase masterWriteData)
        {
            if (masterWriteData is WriteSingleCoilData)
            {
                //02 05 00 01 FF 00 DD C9
                //02 --从设备地址
                //05 --功能码
                //00 01 --线圈起始地址
                //FF 00 --线圈写入值
                //DD C9 --CRC
                WriteSingleCoilData _data = (WriteSingleCoilData)masterWriteData;
                using (ByteArrayBuilder builder = new ByteArrayBuilder())
                {
                    byte _on = 0xFF;
                    byte _off = 0x00;
                    builder.Append(_data.SlaveID);//高位在前
                    builder.Append((byte)ModbusBaseOrderCmd.WriteSingleCoil);//功能码
                    builder.Append(ByteHelper.ToBytes(_data.Address, true));//高位在前
                    builder.Append(_data.OnOff == true ? _on : _off);//数值
                    builder.Append(_off);
                    ModBusAppData = builder.ToArray();
                }
            }
        }

        /// <summary>
        /// 处理单个寄存器写入
        /// </summary>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        private void HanlderWriteSingleRegisterData(MasterWriteDataBase masterWriteData)
        {
            if (masterWriteData is WriteSingleRegisterData)
            {
                //02 06 00 01 00 03 98 38
                //02 --从设备地址
                //06 --功能码
                //00 01 --寄存器起始地址
                //03 --寄存器写入值
                //98 38 --CRC
                WriteSingleRegisterData _data = (WriteSingleRegisterData)masterWriteData;

                using (ByteArrayBuilder builder = new ByteArrayBuilder())
                {
                    builder.Append(_data.SlaveID);//高位在前
                    builder.Append((byte)ModbusBaseOrderCmd.WriteSingleRegister);//功能码
                    builder.Append(ByteHelper.ToBytes(_data.Address, true));//高位在前
                    builder.Append(ByteHelper.ToBytes(_data.Value, true));
                    ModBusAppData = builder.ToArray();
                }
            }
        }

        /// <summary>
        /// 读取线圈/寄存器组包 构造函数
        /// </summary>
        /// <param name="masterReadData">Modubs Master 读取数据</param>
        public ModBusTCPPackage(MasterReadDataBase masterReadData)
        {
            HanlderModbusBaseRead(masterReadData);
        }

        private void HanlderModbusBaseRead(MasterReadDataBase masterReadData)
        {
            byte _readOrderCmd = 0x00;
            if (masterReadData is ReadCoilsData)
                _readOrderCmd = (byte)ModbusBaseOrderCmd.ReadCoilStatus;
            else if (masterReadData is ReadDiscreteInputData)
                _readOrderCmd = (byte)ModbusBaseOrderCmd.ReadInputStatus;
            else if (masterReadData is ReadHoldingRegistersData)
                _readOrderCmd = (byte)ModbusBaseOrderCmd.ReadHoldingRegister;
            else if (masterReadData is ReadInputRegisters)
                _readOrderCmd = (byte)ModbusBaseOrderCmd.ReadInputRegister;
            if (_readOrderCmd != 0x00)
            {
                using (ByteArrayBuilder builder = new ByteArrayBuilder())
                {
                    builder.Append(masterReadData.SlaveID);//高位在前
                    builder.Append(_readOrderCmd);//功能码
                    builder.Append(ByteHelper.ToBytes(masterReadData.Address, true));//高位在前
                    builder.Append(ByteHelper.ToBytes(masterReadData.Quantity, true));//数量
                    ModBusAppData = builder.ToArray();
                }
            }
        }

        public byte[] ToArray()
        {
            using (ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append(ModbusHeader.TransactionIdentifier);
                builder.Append(ModbusHeader.ProtocolIdentifier);
                builder.Append(ByteHelper.ToBytes((ushort)ModBusAppData.Length));
                builder.Append(ModBusAppData);
                ModbusHeader.IncreaseTranIdentifier();
                return builder.ToArray();
            }
        }
    }
}