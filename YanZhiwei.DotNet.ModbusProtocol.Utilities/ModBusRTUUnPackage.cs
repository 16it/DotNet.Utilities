namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    using System;
    using YanZhiwei.DotNet2.Utilities.Builder;
    using YanZhiwei.DotNet2.Utilities.Common;

    /// <summary>
    ///  Modbus Rtu模式拆包组包
    /// </summary>
    public sealed class RTUModBusUnPackage
    {
        #region Properties

        /// <summary>
        /// 数据长度
        /// </summary>
        public byte DataLength
        {
            get;
            private set;
        }

        /// <summary>
        /// CRC
        /// </summary>
        public byte[] CRC
        {
            get;
            private set;
        }

        /// <summary>
        /// CRC计算部分
        /// </summary>
        public byte[] CrcCaluData
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前寄存器个数
        /// </summary>
        public ushort CurRegisterCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前单个寄存器地址
        /// </summary>
        public ushort CurSingleRegisterAddr
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前单个寄存器地址写入值
        /// </summary>
        public ushort CurSingleRegisterValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前起始寄存器地址
        /// </summary>
        public ushort CurStartRegisterAddr
        {
            get;
            private set;
        }

        /// <summary>
        /// 应用数据部分
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 拆包错误枚举
        /// </summary>
        public UnPackageError ErrorType
        {
            get;
            private set;
        }

        public string FullPackageData
        {
            get;
            private set;
        }

        /// <summary>
        /// 功能码
        /// </summary>
        public byte OrderCmd
        {
            get;
            private set;
        }

        /// <summary>
        /// 从机地址
        /// </summary>
        public byte SlaveID
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 拆包
        /// </summary>
        /// <param name="data">数据报文</param>
        /// <returns>返回结果;1.是否拆包成功；2.拆包成功后对象</returns>
        public bool BuilderObjFromBytes(byte[] data, out UnPackageError unpackageError)
        {
            try
            {
                unpackageError = UnPackageError.Normal;
                bool _analyzeResult = AnalyzePackageData(data, out unpackageError);
                if (unpackageError == UnPackageError.Normal)
                    unpackageError = CheckedPackageData(data);

                return unpackageError == UnPackageError.Normal;
            }
            catch (UnPackageException)
            {
                unpackageError = UnPackageError.ExceptionError;
                return false;
            }
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
                    //02--从机地址
                    //01--功能码
                    //01--数据长度
                    //01--数据
                    //90 0C--CRC
                    int _packageLength = data.Length;
                    DataLength = data[3];//数据长度
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

        private UnPackageError CheckedPackageData(byte[] data)
        {
            try
            {
                byte[] _expectCrc = ByteHelper.ToBytes(CRCBuilder.Calu16MODBUS(CrcCaluData));
                if (!ArrayHelper.CompletelyEqual(_expectCrc, CRC))
                    return UnPackageError.CRCError;

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