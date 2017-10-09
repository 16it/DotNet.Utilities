using System;
using System.Collections;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet.ModbusProtocol.Utilities.Model
{
    /// <summary>
    /// 写多个线圈
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.ModbusProtocol.Utilities.Model.MasterWriteDataBase" />
    public sealed class WriteMultipleCoilsData : MasterWriteDataBase
    {
        /// <summary>
        /// 线圈数量
        /// </summary>
        public ushort Quantity
        {
            get;
            private set;
        }

        /// <summary>
        /// 写入线圈状态
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
        /// <param name="address">线圈/寄存器地址</param>
        /// <param name="colisStatus">写入线圈状态</param>
        public WriteMultipleCoilsData(byte slaveID, ushort address, BitArray colisStatus)
        : base(slaveID, address)
        {
            if (colisStatus == null)
                throw new ArgumentException("多个写入线圈数组不能等于NULL.");

            if (colisStatus.Length > ushort.MaxValue)
                throw new ArgumentException("多个写入线圈数组数量超出范围.");

            Quantity = (ushort)colisStatus.Length;
            ColisStatus = colisStatus;
            
            
        }
    }
}