namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    using System;
    using System.Linq;
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
        public Tuple<bool, UnPackageError> BuilderObjFromBytes(byte[] data)
        {
            try
            {
                bool _analyzeResult = AnalyzePackageData(data);
                UnPackageError _unpackgeErrorCode = CheckedPackageData(data, _analyzeResult);

                if (_unpackgeErrorCode != UnPackageError.Normal)
                    return CreateBuileObjFromBytesError(_unpackgeErrorCode);

                return new Tuple<bool, UnPackageError>(true, _unpackgeErrorCode);
            }
            catch (UnPackageException)
            {
                return CreateBuileObjFromBytesError(UnPackageError.ExceptionError);
            }
        }

        private bool AnalyzePackageData(byte[] data)
        {
            bool _result = false;

            try
            {
                FullPackageData = ByteHelper.ToHexStringWithBlank(data);
                SlaveID = data[0];//从设备地址
                OrderCmd = data[1];//功能码

                if (CheckedErrorPackageData(data))
                {
                    //02 01 01 01 90 0C
                    //02--从机地址
                    //01--功能码
                    //01--数据长度
                    //01--数据
                    //90 0C--CRC
                    int _packageLength = data.Length;
                    DataLength = data[2];//数据长度
                    CrcCaluData = ArrayHelper.Copy(data, 0, _packageLength - 2);
                    Data = ArrayHelper.Copy(data, 3, _packageLength - 2);
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

        private bool CheckedErrorPackageData(byte[] data)
        {
            bool _result = true;

            if (data.Count() == 5)
            {
                byte _orderCmd = data[1];//功能码
                byte _errorCode = data[2];//错误代码
                _result = _errorCode == 0x01 || _errorCode == 0x02 || _errorCode == 0x03 || _errorCode == 0x04;

                if (_result)
                    ErrorType = (UnPackageError)_errorCode;
            }

            return _result;
        }

        private UnPackageError CheckedPackageData(byte[] data, bool analyzeResult)
        {
            try
            {
                if (!analyzeResult)
                    return ErrorType;
                byte[] _expectCrc = ByteHelper.ToBytes(CRCBuilder.Calu16MODBUS(CrcCaluData), false);
                if (!ArrayHelper.CompletelyEqual(_expectCrc, CRC))
                    return UnPackageError.CRCError;

                return UnPackageError.Normal;
            }
            catch (Exception ex)
            {
                throw CreateUnPackageException("CheckedPackageData", ex, data);
            }
        }

        private Tuple<bool, UnPackageError> CreateBuileObjFromBytesError(UnPackageError unpackgeErrorCode)
        {
            return new Tuple<bool, UnPackageError>(false, unpackgeErrorCode);
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