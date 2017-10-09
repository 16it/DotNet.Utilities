namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum
{
    /// <summary>
    /// Modbus通用功能码
    /// </summary>
    public enum ReadOrderCmd : byte
    {
        /// <summary>
        ///读线圈
        /// </summary>
        ReadCoilStatus = 0x01,

        /// <summary>
        /// 读离散量输入
        /// </summary>
        ReadInputStatus = 0x02,

        /// <summary>
        ///读保持寄存器
        /// </summary>
        ReadHoldingRegister = 0x03,

        /// <summary>
        /// 读输入寄存器
        /// </summary>
        ReadInputRegister = 0x04
    }
}