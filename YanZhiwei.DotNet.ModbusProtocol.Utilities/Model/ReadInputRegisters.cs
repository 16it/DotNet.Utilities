namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 读输入寄存器
    /// </summary>
    public sealed class ReadInputRegisters : MasterReadDataBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="quantity">读取线圈/寄存器数量</param>
        public ReadInputRegisters(byte slaveID, ushort address, ushort quantity) : base(slaveID, address, quantity)
        {
        }
    }
}