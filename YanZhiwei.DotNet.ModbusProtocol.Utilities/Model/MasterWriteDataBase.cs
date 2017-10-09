namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// Modubs Master 写入数据基类
    /// </summary>
    public abstract class MasterWriteDataBase
    {
        /// <summary>
        /// 从机地址
        /// </summary>
        public byte SlaveID
        {
            get;
            protected set;
        }

        /// <summary>
        ///线圈/寄存器地址
        /// </summary>
        public ushort Address
        {
            get;
            protected set;
        }

        /// <summary>
        /// 基类构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        protected MasterWriteDataBase(byte slaveID, ushort address)
        {
            SlaveID = slaveID;
            Address = address;
        }
    }
}