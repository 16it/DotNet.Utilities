using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Builder;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    /// <summary>
    /// Modbus Rtu 组包
    /// </summary>
    public sealed class ModBusRTUPackage
    {
        /// <summary>
        /// CRC计算部分
        /// </summary>
        private byte[] CRCCaluData { get; set; }

        /// <summary>
        /// 写入线圈/寄存器组包 构造函数
        /// </summary>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        public ModBusRTUPackage(MasterWriteDataBase masterWriteData)
        {
            HanlderWriteSingleCoilData(masterWriteData);
            HanlderWriteSingleRegisterData(masterWriteData);
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
                    builder.Append((byte)WriteOrderCmd.WriteSingleRegister);//功能码
                    builder.Append(ByteHelper.ToBytes(_data.Address, true));//高位在前
                    builder.Append(ByteHelper.ToBytes(_data.Value, true));
                    CRCCaluData = builder.ToArray();
                }
            }
        }

        public byte[] ToArray()
        {
            using (ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append(CRCCaluData);
                byte[] _crcValue = ByteHelper.ToBytes(CRCBuilder.Calu16MODBUS(CRCCaluData), false);
                builder.Append(_crcValue);
                return builder.ToArray();
            }
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
                    builder.Append((byte)WriteOrderCmd.WriteSingleCoil);//功能码
                    builder.Append(ByteHelper.ToBytes(_data.Address, true));//高位在前
                    builder.Append(_data.OnOff == true ? _on : _off);//数值
                    builder.Append(_off);
                    CRCCaluData = builder.ToArray();
                }
            }
        }

        /// <summary>
        /// 读取线圈/寄存器组包 构造函数
        /// </summary>
        /// <param name="masterReadData">Modubs Master 读取数据</param>
        public ModBusRTUPackage(MasterReadDataBase masterReadData)
        {
        }
    }
}