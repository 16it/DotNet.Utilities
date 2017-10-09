namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 读取单个线圈
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.MasterReadDataBase" />
    public sealed class ReadCoilsData : MasterReadDataBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="quantity">读取线圈/寄存器数量</param>
        public ReadCoilsData(byte slaveID, ushort address, ushort quantity) : base(slaveID, address, quantity)
        {
        }
    }
}