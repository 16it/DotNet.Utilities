namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum
{
    /// <summary>
    /// Modbus通用功能码
    /// </summary>
    public enum WriteOrderCmd : byte
    {
        /// <summary>
        /// 写单个线圈
        /// </summary>
        WriteSingleCoil = 0x05,

        /// <summary>
        ///写单个寄存器
        /// </summary>
        WriteSingleRegister = 0x06,

        /// <summary>
        /// 写多个线圈
        /// </summary>
        WriteMultipleCoils = 0x0F,

        /// <summary>
        /// 写多个寄存器
        /// </summary>
        WriteMultipleRegister = 0x10
    }
}