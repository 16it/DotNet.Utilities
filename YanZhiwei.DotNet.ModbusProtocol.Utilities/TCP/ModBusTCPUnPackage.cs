using System;
using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    public sealed class ModBusTCPUnPackage
    {
        #region Properties

        public string FullPackageData
        {
            get;
            private set;
        }

        /// <summary>
        /// 事务标识符
        /// </summary>
        public byte[] TransactionIdentifier
        {
            get;
            private set;
        }

        /// <summary>
        /// CRC
        /// </summary>
        private byte[] CRC
        {
            get;
            set;
        }

        /// <summary>
        /// 协议标识符
        /// </summary>
        public byte[] ProtocolIdentifier
        {
            get;
            private set;
        }

        /// <summary>
        /// CRC计算部分
        /// </summary>
        private byte[] CrcCaluData
        {
            get;
            set;
        }

        /// <summary>
        /// 应用数据部分
        /// </summary>
        private byte[] Data
        {
            get;
            set;
        }

        /// <summary>
        /// 长度
        /// </summary>
        private byte[] Length
        {
            get;
            set;
        }

        /// <summary>
        /// 数据长度
        /// </summary>
        private byte DataLength { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        private byte OrderCmd
        {
            get;
            set;
        }

        /// <summary>
        /// 从机地址
        /// </summary>
        private byte SlaveID
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="data">数据报文</param>
        /// <returns>返回结果;1.是否拆包成功；2.拆包成功后对象</returns>
        public UnPackageError BuilderObjFromBytes(byte[] data, out SlaveReplyDataBase slaveReplyData)
        {
            slaveReplyData = null;
            UnPackageError _unpackageError = UnPackageError.Normal;
            try
            {
                bool _analyzeResult = AnalyzePackageData(data, out _unpackageError);
                if (_unpackageError == UnPackageError.Normal)
                    _unpackageError = CheckedPackageData(data, out slaveReplyData);

                return _unpackageError;
            }
            catch (UnPackageException)
            {
                _unpackageError = UnPackageError.ExceptionError;
            }
            return _unpackageError;
        }

        private bool AnalyzePackageData(byte[] data, out UnPackageError unPackageError)
        {
            bool _result = false;

            try
            {
                FullPackageData = ByteHelper.ToHexStringWithBlank(data);
                TransactionIdentifier = ArrayHelper.Copy(data, 0, 2);//事物标识符
                ProtocolIdentifier = ArrayHelper.Copy(data, 2, 4);//协议标识符
                Length = ArrayHelper.Copy(data, 4, 6);//长度
                SlaveID = data[6];//从设备地址
                OrderCmd = data[7];//功能码
                unPackageError = UnPackageError.Normal;
                if (data.Length == 9)
                {
                    byte _errorCode = data[8];//错误代码
                    if (_errorCode == 0x01 || _errorCode == 0x02 || _errorCode == 0x03 || _errorCode == 0x04)
                        unPackageError = (UnPackageError)_errorCode;
                }
                else
                {
                    //00 77 00 00 00 05 02 01 02 1F 00--Read Coils
                    //00 74 --事物标识符
                    //00 00 --协议标识符
                    //00 05 --长度
                    //02--从机地址
                    //01--功能码
                    //02 --数据长度
                    //1F 00--数据

                    DataLength = data[8];//数据长度
                    Data = ArrayHelper.Copy(data, 9, 9 + DataLength);//实际数据

                    _result = true;
                }
            }
            catch (Exception ex)
            {
                throw CreateUnPackageException("AnalyzePackageData", ex, data);
            }

            return _result;
        }

        private UnPackageError CheckedPackageData(byte[] data, out SlaveReplyDataBase replyDataBase)
        {
            try
            {
                replyDataBase = null;

                switch (OrderCmd)
                {
                    case 0x01:
                        replyDataBase = new SlaveReadCoilsReplyData(SlaveID, OrderCmd, Data);
                        break;

                    case 0x02:
                        replyDataBase = new SlaveReadDiscreteInputsReplyData(SlaveID, OrderCmd, Data);
                        break;

                    case 0x03:
                        replyDataBase = new SlaveReadHoldingRegisterReplyData(SlaveID, OrderCmd, Data);
                        break;

                    case 0x04:
                        replyDataBase = new SlaveReadInputRegistersReplyData(SlaveID, OrderCmd, Data);
                        break;

                    default:
                        replyDataBase = new SlaveUnknownReplyData(SlaveID, OrderCmd, Data);
                        break;
                }
                return UnPackageError.Normal;
            }
            catch (Exception ex)
            {
                throw CreateUnPackageException("CheckedPackageData", ex, data);
            }
        }

        /// <summary>
        /// 创建拆包异常
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="ex">Exception</param>
        /// <param name="data">数据报文</param>
        /// <returns>UnPackageException</returns>
        private UnPackageException CreateUnPackageException(string methodName, Exception ex, byte[] data)
        {
            return new UnPackageException(methodName, ex.Message, ex, data);
        }

        #endregion Methods
    }
}