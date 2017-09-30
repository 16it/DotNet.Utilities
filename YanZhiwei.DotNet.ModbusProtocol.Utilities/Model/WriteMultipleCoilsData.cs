namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 写多个线圈
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.ModbusMasterDataBase" />
    public sealed class WriteMultipleCoilsData : ModbusMasterDataBase
    {
        public bool OnOff
        {
            get;
            set;
        }
    }
}