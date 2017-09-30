namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 写单个线圈
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.ModbusMasterDataBase" />
    public sealed class WriteSingleCoilData : ModbusMasterDataBase
    {
        public bool OnOff
        {
            get;
            set;
        }
    }
}