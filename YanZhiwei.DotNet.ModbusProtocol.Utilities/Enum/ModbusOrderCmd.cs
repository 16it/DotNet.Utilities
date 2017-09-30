namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum
{
    /// <summary>
    /// Modbus主要功能码
    /// </summary>
    public enum ModbusOrderCmd : byte
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
        ReadInputRegister = 0x04,

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