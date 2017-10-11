using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    /// <summary>
    /// MODBUS协议报文头
    /// </summary>
    public abstract class MBAPHeader
    {
        private static readonly object syncRoot = new object();

        /// <summary>
        /// 事务标识符递增
        /// </summary>
        public void IncreaseTranIdentifier()
        {
            lock (syncRoot)
            {
                if (transactionId == ushort.MaxValue)
                    transactionId = ushort.MinValue;
                transactionId++;
                TransactionIdentifier = ByteHelper.ToBytes(transactionId, false);
            }
        }

        private static ushort transactionId
        {
            get; set;
        }

        /// <summary>
        /// 事务标识符
        /// </summary>
        public byte[] TransactionIdentifier
        {
            get;
            protected set;
        }

        /// <summary>
        /// 协议标识符
        /// </summary>
        public byte[] ProtocolIdentifier
        {
            get;
            protected set;
        }

        /// <summary>
        /// 单元标识符
        /// </summary>
        public byte[] Length
        {
            get;
            protected set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transactionIdentifier">事务标识符</param>
        /// <param name="protocolIdentifier">单元标识符</param>
        /// <param name="data">计算长度的数据部分</param>
        public MBAPHeader(ushort transactionIdentifier, ushort protocolIdentifier, byte[] data)
        {
            transactionId = transactionIdentifier == ushort.MinValue ? transactionId : transactionIdentifier;
            TransactionIdentifier = ByteHelper.ToBytes(transactionId, false);
            ProtocolIdentifier = ByteHelper.ToBytes(protocolIdentifier, false);
            Length = data == null ? new byte[2] { 0x00, 0x00 } : ByteHelper.ToBytes((ushort)data.Length, false);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transactionIdentifier">事务标识符</param>
        /// <param name="protocolIdentifier">单元标识符</param>
        public MBAPHeader(ushort transactionIdentifier, ushort protocolIdentifier) : this(transactionIdentifier, protocolIdentifier, null)
        {
        }
    }
}