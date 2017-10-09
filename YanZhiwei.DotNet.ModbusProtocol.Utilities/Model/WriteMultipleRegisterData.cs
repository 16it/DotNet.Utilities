using System;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 多个寄存器写入
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.MasterWriteDataBase" />
    public sealed class WriteMultipleRegisterData : MasterWriteDataBase
    {
        /// <summary>
        /// 写入多个寄存器数组
        /// </summary>
        public ushort[] Value
        {
            get;
            private set;
        }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        public ushort Quantity
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="value">线圈数量</param>
        public WriteMultipleRegisterData(byte slaveID, ushort address, ushort[] value)
            : base(slaveID, address)
        {
            if (value == null)
                throw new ArgumentException("多个写入寄存器数组不能等于NULL.");

            if (value.Length > ushort.MaxValue)
                throw new ArgumentException("多个写入寄存器数组数量超出范围.");
            Value = value;
            Quantity = (ushort)value.Length;
        }
    }
}