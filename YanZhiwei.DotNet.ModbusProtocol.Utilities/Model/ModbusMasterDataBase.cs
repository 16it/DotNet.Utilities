namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    public class ModbusMasterDataBase
    {
        /// <summary>
        /// 从设备地址
        /// </summary>
        public byte SlaveID
        {
            get;
            protected set;
        }

        public ushort Address
        {
            get;
            protected set;
        }
    }
}