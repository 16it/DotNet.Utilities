namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    /// <summary>
    /// 默认 MODBUS协议报文头
    /// </summary>
    public sealed class StandardMBAPHeader : MBAPHeader
    {
        public StandardMBAPHeader() : base(0, 0)
        {
        }
    }
}