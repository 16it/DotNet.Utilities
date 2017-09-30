namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 写单个寄存器
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.ModbusMasterDataBase" />
    public sealed class WriteSingleRegisterData : ModbusMasterDataBase
    {
        public ushort Value
        {
            get;
            set;
        }
    }
}