namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;

    /// <summary>
    /// 未知或用户自定义请求响应
    /// </summary>
    public sealed class SlaveUnknownReplyData : SlaveReplyDataBase
    {
        #region Constructors

        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="orderCmdCode">功能码枚举</param>
        /// <param name="data">协议数据</param>
        public SlaveUnknownReplyData(byte slaveID, byte orderCmdCode, byte[] data)
            : base(slaveID, orderCmdCode, ModbusBaseOrderCmd.Unknown)
        {
            Data = data;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 协议数据
        /// </summary>
        public byte[] Data
        {
            get; private set;
        }

        #endregion Properties
    }
}