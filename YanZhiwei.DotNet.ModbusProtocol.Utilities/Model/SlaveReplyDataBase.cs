using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 从机终端数据请求响应数据基类
    /// </summary>
    public abstract class SlaveReplyDataBase
    {
        /// <summary>
        /// 从机地址
        /// </summary>
        public byte SlaveID
        {
            get;
            protected set;
        }

        /// <summary>
        /// 功能码
        /// </summary>
        public byte OrderCmdCode
        {
            get;
            protected set;
        }

        /// <summary>
        /// 功能码枚举
        /// </summary>
        public ModbusBaseOrderCmd OrderCmd
        {
            get;
            protected set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="orderCmdCode">功能码</param>
        /// <param name="orderCmd">功能码枚举</param>
        public SlaveReplyDataBase(byte slaveID, byte orderCmdCode, ModbusBaseOrderCmd orderCmd)
        {
            SlaveID = slaveID;
            OrderCmdCode = orderCmdCode;
            OrderCmd = orderCmd;
        }
    }
}