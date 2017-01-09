namespace YanZhiwei.DotNet2.Utilities.Core
{
    using Common;
    using Operator;
    using System.Collections.Generic;
    
    /// <summary>
    ///适用于串口，Socket数据协议 接口
    /// </summary>
    public interface IUnPackageProtocol
    {
        #region Methods
        
        /// <summary>
        /// 计算CRC数值
        /// </summary>
        /// <param name="buffer">需要计算CRC部分BYTE数值</param>
        /// <returns>CRC数值</returns>
        byte[] GetCaluCrcValue(byte[] buffer);
        
        /// <summary>
        /// 检查CRC数值
        /// </summary>
        /// <param name="expect">期待数值</param>
        /// <param name="actual">实际数值</param>
        /// <returns>是否一致</returns>
        bool CheckedCaluCrc(byte[] expect, byte[] actual);
        
        /// <summary>
        ///获取CRC计算BYTE数组
        /// </summary>
        /// <param name="buffer">完整报文</param>
        /// <returns>需要CRC计算BYTE数组</returns>
        byte[] GetProtocolCaluCRCSection(byte[] buffer);
        
        /// <summary>
        /// 获取完整报文中实际CRC数值
        /// </summary>
        /// <param name="buffer">完整报文</param>
        /// <returns>CRC数值</returns>
        byte[] GetProtocolCRCSection(byte[] buffer);
        
        /// <summary>
        /// 获取报文数据长度
        /// </summary>
        /// <param name="buffer">缓冲数据</param>
        /// <returns>协议数据长度</returns>
        int GetProtocolLengthSection(List<byte> buffer);
        
        #endregion Methods
    }
    
    /// <summary>
    /// 适用于串口，Socket数据协议拆包
    /// </summary>
    public abstract class UnPackageData
    {
        #region Fields
        
        /// <summary>
        /// 结束位，若值==0x00，则不检查结束位
        /// </summary>
        public readonly byte EndFlag;
        
        /// <summary>
        /// 协议最大长度
        /// </summary>
        public readonly int ProtocolMaxFullCount;
        
        /// <summary>
        /// 协议最小长度
        /// </summary>
        public readonly int ProtocolMinCount;
        
        /// <summary>
        /// 起始位
        /// </summary>
        public readonly byte StartFlag;
        
        private bool CheckEndFlag
        {
            get
            {
                return EndFlag != 0x00;
            }
        }
        
        /// <summary>
        /// 拆包接口
        /// </summary>
        private IUnPackageProtocol unPackageProtocol = null;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iUnPackage">拆包接口</param>
        /// <param name="protocolMinCount">协议最小长度</param>
        /// 时间:2017/1/9 22:42
        /// 备注:
        public UnPackageData(IUnPackageProtocol iUnPackage, int protocolMinCount) : this(iUnPackage, 0x68, 0x16, 65535, protocolMinCount)
        {
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iUnPackage">拆包接口定义实现</param>
        /// <param name="startFlag">起始位</param>
        /// <param name="endflag">结束位</param>
        /// <param name="protocolMaxFullCount">报文最大长度</param>
        /// <param name="protocolMinCount">协议最小长度</param>
        /// 时间:2017/1/9 22:41
        /// 备注:
        public UnPackageData(IUnPackageProtocol iUnPackage, byte startFlag, byte endflag, int protocolMaxFullCount, int protocolMinCount)
        {
            ValidateOperator.Begin().NotNull(iUnPackage, "适用于串口，Socket数据协议接口");
            StartFlag = startFlag;
            EndFlag = endflag;
            ProtocolMaxFullCount = protocolMaxFullCount;
            unPackageProtocol = iUnPackage;
            ProtocolMinCount = protocolMinCount;
        }
        
        #endregion Constructors
        
        #region Delegates
        
        /// <summary>
        /// 接收到完整数据委托
        /// </summary>
        /// <param name="packageData">完整报文</param>
        public delegate void ReceivedFullPackageDataEventHandler(byte[] packageData);
        
        #endregion Delegates
        
        #region Events
        
        /// <summary>
        /// 接收到完整数据事件
        /// </summary>
        public event ReceivedFullPackageDataEventHandler ReceivedFullPackageEvent;
        
        #endregion Events
        
        #region Properties
        
        /// <summary>
        /// 缓冲数据集合
        /// </summary>
        public List<byte> CacheBuffer
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否数据接收中....
        /// </summary>
        public bool DataReceiving
        {
            get;
            set;
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 开始拆包判断，当尚未进入数据缓冲状态
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public virtual void DataBufferReceiving(byte[] buffer)
        {
            if(!DataReceiving)
            {
                if(buffer[0] == StartFlag)
                {
                    DataReceiving = true;
                    CacheBuffer = new List<byte>();
                }
                else
                {
                    int _cmdWordIndex = FindedCommandWords(buffer);
                    
                    if(_cmdWordIndex > 0)
                    {
                        DataReceiving = true;
                        CacheBuffer = new List<byte>();
                        buffer = ArrayHelper.Copy(buffer, _cmdWordIndex, buffer.Length);
                    }
                }
            }
            
            if(DataReceiving)
            {
                CacheBuffer.AddRange(buffer);
                VerifyingPacketData();
            }
            else
            {
                ResetDataReceived();
            }
        }
        
        /// <summary>
        /// 当协议粘包得时候，查找处理
        /// </summary>
        /// <param name="buffer">缓冲数据buffer</param>
        /// <returns>查找到得索引</returns>
        protected virtual int FindedCommandWords(byte[] buffer)
        {
            int _cmdWordIndex = 0;
            bool _result = buffer != null;
            
            if(_result)
            {
                foreach(byte item in buffer)
                {
                    if(item == StartFlag)
                    {
                        break;
                    }
                    
                    _cmdWordIndex++;
                }
            }
            
            return _cmdWordIndex;
        }
        
        /// <summary>
        /// 重置数据，包括接收状态，以及缓冲
        /// </summary>
        protected virtual void ResetDataReceived()
        {
            DataReceiving = false;
            CacheBuffer = null;
        }
        
        /// <summary>
        /// 验证CRC是否合法
        /// </summary>
        /// <param name="checkedCrc">是否检验CRC</param>
        /// <param name="packetDataBuffer">完整报文Byte数组</param>
        /// <returns>CRC是否合法</returns>
        protected virtual bool VerifyingPacketCRC(bool checkedCrc, byte[] packetDataBuffer)
        {
            bool _result = false;
            
            if(checkedCrc && packetDataBuffer != null)
            {
                byte[] _packetDataCRC = unPackageProtocol.GetProtocolCaluCRCSection(packetDataBuffer);
                byte[] _expectCRC = unPackageProtocol.GetCaluCrcValue(_packetDataCRC);
                byte[] _actualCRC = unPackageProtocol.GetProtocolCRCSection(packetDataBuffer);
                _result = unPackageProtocol.CheckedCaluCrc(_expectCRC, _actualCRC);
            }
            
            return _result;
        }
        
        /// <summary>
        /// 判断协议长度，并返回符合协议要求得完整报文
        /// </summary>
        /// <param name="checkedCrc">是否检查CRC</param>
        /// <returns>完整报文</returns>
        protected virtual byte[] VerifyingPacketDataLength(out bool checkedCrc)
        {
            checkedCrc = false;
            byte[] _packetDataBuffer = null;
            int _dataPackageLength = unPackageProtocol.GetProtocolLengthSection(CacheBuffer);
            
            if((_dataPackageLength + ProtocolMinCount) <= CacheBuffer.Count)
            {
                _packetDataBuffer = ArrayHelper.Copy<byte>(CacheBuffer.ToArray(), 0, _dataPackageLength + ProtocolMinCount);
                checkedCrc = true;
            }
            
            return _packetDataBuffer;
        }
        
        private void VerifyingPacketData()
        {
            if(CacheBuffer.Count >= ProtocolMinCount)
            {
                bool _checkedCrc = false;
                byte[] _packetDataBuffer = VerifyingPacketDataLength(out _checkedCrc);
                
                if(_checkedCrc)
                {
                    bool _checkedEndFlagResult = true;
                    
                    if(CheckEndFlag)
                    {
                        _checkedEndFlagResult = _packetDataBuffer[_packetDataBuffer.Length - 1] == EndFlag;
                    }
                    
                    if(_checkedEndFlagResult && VerifyingPacketCRC(_checkedCrc, _packetDataBuffer))
                    {
                        if(ReceivedFullPackageEvent != null)
                        {
                            ReceivedFullPackageEvent(_packetDataBuffer);
                        }
                    }
                    
                    ResetDataReceived();
                }
                else if(CacheBuffer.Count >= ProtocolMaxFullCount)
                {
                    ResetDataReceived();
                }
            }
            
            #endregion Methods
        }
    }
}