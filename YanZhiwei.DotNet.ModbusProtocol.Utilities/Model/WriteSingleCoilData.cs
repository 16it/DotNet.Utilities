namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 写单个线圈
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.MasterWriteDataBase" />
    public sealed class WriteSingleCoilData : MasterWriteDataBase
    {
        /// <summary>
        /// 开关
        /// </summary>
        public bool OnOff
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="onOff">开关状态</param>
        public WriteSingleCoilData(byte slaveID, ushort address, bool onOff)
            : base(slaveID, address)
        {
            OnOff = onOff;
        }
    }
}