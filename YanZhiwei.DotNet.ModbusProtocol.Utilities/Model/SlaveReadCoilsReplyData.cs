using System.Collections;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    public sealed class SlaveReadCoilsReplyData : SlaveReplyDataBase
    {
        /// <summary>
        /// 线圈状态
        /// </summary>
        public BitArray ColisStatus
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="orderCmd">功能码</param>
        /// <param name="colisStatus">线圈状态</param>
        public SlaveReadCoilsReplyData(byte slaveID, byte orderCmdCode, ModbusBaseOrderCmd orderCmd, byte[] colisStatus) : base(slaveID, orderCmdCode, orderCmd)
        {
            ColisStatus = new BitArray(colisStatus);
        }
    }
}