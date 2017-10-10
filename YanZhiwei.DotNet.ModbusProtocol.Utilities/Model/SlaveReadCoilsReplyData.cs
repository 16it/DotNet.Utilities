using System.Collections;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 读取读线圈请求响应数据
    /// </summary>
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
        /// <param name="colisStatus">线圈状态</param>
        public SlaveReadCoilsReplyData(byte slaveID, byte orderCmdCode, byte[] colisStatus) : base(slaveID, orderCmdCode, ModbusBaseOrderCmd.ReadCoilStatus)
        {
            ColisStatus = new BitArray(colisStatus);
        }
    }
}