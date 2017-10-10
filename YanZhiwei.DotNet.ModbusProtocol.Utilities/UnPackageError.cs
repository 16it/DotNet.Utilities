using System.ComponentModel;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    /// <summary>
    /// 报文拆包错误枚举
    /// </summary>
    public enum UnPackageError : byte
    {
        /// <summary>
        /// 初始化为正常
        /// </summary>
        [Description("初始化为正常")]
        Normal = 0xFF,
        
        /// <summary>
        /// 包头错误
        /// </summary>
        [Description("包头错误")]
        HeaderFlagError = 0xFE,
        
        /// <summary>
        /// 流水号
        /// </summary>
        [Description("流水号错误")]
        SeqNoError = 0xFD,
        
        /// <summary>
        /// 长度域错误
        /// </summary>
        [Description("长度域错误")]
        DataLengthError = 0xFC,
        
        /// <summary>
        /// 包定义的长度和实际收到长度不符合错误
        /// </summary
        [Description("包定义的长度和实际收到长度不符合错误")]
        PackageLengthError = 0xFB,
        
        /// <summary>
        /// CRC错误
        /// </summary>
        [Description("CRC错误")]
        CRCError = 0xFA,
        
        /// <summary>
        /// 包尾错误
        /// </summary>
        [Description("包尾错误")]
        EndFlagError = 0xF9,
        
        /// <summary>
        /// 分析包时未知错误
        /// </summary>
        [Description("分析包时未知错误")]
        ExceptionError = 0xF8,
        
        /// <summary>
        /// 不支持该功能
        /// </summary>
        [Description("不支持该功能")]
        NotSupportedOrderCmd = 0x01,
        
        /// <summary>
        /// 越界
        /// </summary>
        [Description("越界")]
        Transboundary = 0x02,
        
        /// <summary>
        /// 寄存器数量超出范围
        /// </summary>
        [Description("寄存器数量超出范围")]
        RegisterCountError = 0x03,
        
        /// <summary>
        /// 读写错误
        /// </summary>
        [Description("读写错误")]
        ReadWriteError = 0x04
    }
}