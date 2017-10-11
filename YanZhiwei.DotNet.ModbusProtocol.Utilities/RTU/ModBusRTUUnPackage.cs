namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    using System;

    using YanZhiwei.DotNet.ModbusProtocol.Utilities.Model;
    using YanZhiwei.DotNet2.Utilities.Builder;
    using YanZhiwei.DotNet2.Utilities.Common;

    /// <summary>
    ///  Modbus Rtu模式拆包组包
    /// </summary>
    public sealed class RTUModBusUnPackage
    {
        #region Properties

        public string FullPackageData
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
        /// 数据长度
        /// </summary>
        private byte DataLength
        {
            get;
            set;
        }

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
                SlaveID = data[0];//从设备地址
                OrderCmd = data[1];//功能码
                unPackageError = UnPackageError.Normal;
                if (data.Length == 5)
                {
                    byte _errorCode = data[2];//错误代码
                    if (_errorCode == 0x01 || _errorCode == 0x02 || _errorCode == 0x03 || _errorCode == 0x04)
                        unPackageError = (UnPackageError)_errorCode;
                }
                else
                {
                    //02 01 02 1F 00 F5 CC --Read Coils
                    //02--从机地址
                    //01--功能码
                    //02--数据长度
                    //1F 00--数据
                    //F5 CC--CRC

                    //02 02 02 1F 00 F5 88 --Read Discrete Inputs
                    //02--从机地址
                    //02--功能码
                    //02--数据长度
                    //1F 00--数据
                    //F8 88--CRC

                    //02 03 14 00 01 00 02 00 03 00 04 00 05 00 00 00 00 00 00 00 00 00 00 37 57
                    //02--从机地址
                    //03--功能码
                    //14--数据长度
                    //00 01 00 02 00 03 00 04 00 05 00 00 00 00 00 00 00 00 00 00--数据
                    //37 57--CRC

                    //02 04 14 00 01 00 02 00 03 00 04 00 05 00 00 00 00 00 00 00 00 00 00 01 B1
                    //02--从机地址
                    //04--功能码
                    //14--数据长度
                    //00 01 00 02 00 03 00 04 00 05 00 00 00 00 00 00 00 00 00 00--数据
                    //01 B1--CRC

                    int _packageLength = data.Length;
                    DataLength = data[2];//数据长度
                    CrcCaluData = ArrayHelper.Copy(data, 0, _packageLength - 2);
                    Data = ArrayHelper.Copy(data, 3, 3 + DataLength);//实际数据
                    CRC = ArrayHelper.Copy(data, _packageLength - 2, _packageLength);
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
                byte[] _expectCrc = ByteHelper.ToBytes(CRCBuilder.Calu16MODBUS(CrcCaluData), false);
                if (!ArrayHelper.CompletelyEqual(_expectCrc, CRC))
                    return UnPackageError.CRCError;

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