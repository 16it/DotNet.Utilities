namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 读保持寄存器
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.MasterReadDataBase" />
    public sealed class ReadHoldingRegistersData : MasterReadDataBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="quantity">读取线圈/寄存器数量</param>
        public ReadHoldingRegistersData(byte slaveID, ushort address, ushort quantity) : base(slaveID, address, quantity)
        {
        }
    }
}