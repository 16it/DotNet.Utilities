using System;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 读保持寄存器响应数据
    /// </summary>
    public sealed class SlaveReadHoldingRegisterReplyData : SlaveReplyDataBase
    {
        /// <summary>
        /// 多个寄存器数值
        /// </summary>
        public short[] Value
        {
            get;
            private set;
        }

        /// <summary>
        /// 读取寄存器数量
        /// </summary>
        public ushort Quantity
        {
            get
            {
                return (ushort)Value.Length;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="orderCmdCode">功能码枚举</param>
        /// <param name="value">保持寄存器响应数据</param>
        public SlaveReadHoldingRegisterReplyData(byte slaveID, byte orderCmdCode, byte[] value) : base(slaveID, orderCmdCode, ModbusBaseOrderCmd.ReadHoldingRegister)
        {
            int i = 0;
            int j = 0;
            Value = new short[value.Length / 2];
            foreach (byte item in value)
            {
                if (i % 2 == 0)
                {
                    byte[] _data = ArrayHelper.Copy(value, i, i + 2);
                    Array.Reverse(_data);
                    Value[j] = ByteHelper.ToInt16(_data);
                    j++;
                }
                i++;
            }
        }
    }
}