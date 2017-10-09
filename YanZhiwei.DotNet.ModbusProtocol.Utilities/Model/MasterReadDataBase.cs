namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// Modubs Master 读取数据基类
    /// </summary>
    public abstract class MasterReadDataBase
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
        /// 读取线圈/寄存器数量
        /// </summary>
        public ushort Quantity
        {
            get;
            protected set;
        }

        /// <summary>
        /// 基类构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="quantity">读取线圈/寄存器数量</param>
        protected MasterReadDataBase(byte slaveID, ushort address, ushort quantity)
        {
            SlaveID = slaveID;
            Address = address;
            Quantity = quantity;
        }
    }
}