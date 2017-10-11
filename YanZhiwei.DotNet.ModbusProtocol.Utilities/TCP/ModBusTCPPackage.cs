using YanZhiwei.DotNet.ModbusProtocol.Utilities;
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
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterWriteData">Modubs Master 写入数据</param>
        public ModBusTCPPackage(MasterWriteDataBase masterWriteData) : this(new StandardMBAPHeader(), masterWriteData)
        {
            HanlderWriteSingleCoilData(masterWriteData);
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