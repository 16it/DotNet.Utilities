using System.Collections;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Enum;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 读取离散量输入响应数据
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.SlaveReplyDataBase" />
    public sealed class SlaveReadDiscreteInputsReplyData : SlaveReplyDataBase
    {
        /// <summary>
        /// 离散量输入状态
        /// </summary>
        public BitArray DiscreteInputsStatus
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="slaveID">从机地址</param>
        /// <param name="orderCmdCode">功能码枚举</param>
        /// <param name="inputStatus">离散量输入</param>
        public SlaveReadDiscreteInputsReplyData(byte slaveID, byte orderCmdCode, byte[] inputStatus) : base(slaveID, orderCmdCode, ModbusBaseOrderCmd.ReadInputStatus)
        {
            DiscreteInputsStatus = new BitArray(inputStatus);
        }
    }
}