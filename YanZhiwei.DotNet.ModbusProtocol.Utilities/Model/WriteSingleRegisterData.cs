namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 写单个寄存器
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.MasterWriteDataBase" />
    public sealed class WriteSingleRegisterData : MasterWriteDataBase
    {
        /// <summary>
        /// 单个寄存器写入数值
        /// </summary>
        public ushort Value
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="value">线圈数量</param>
        public WriteSingleRegisterData(byte slaveID, ushort address, ushort value)
            : base(slaveID, address)
        {
            Value = value;
        }
    }
}