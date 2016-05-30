namespace YanZhiwei.DotNet.QQWry.Utilities
{
    using System;
    using System.IO;
    using System.Text;

    using YanZhiwei.DotNet2.Utilities.Common;

    /// <summary>
    /// QQwry纯真IP数据库辅助类
    /// </summary>
    /// 时间：2016-05-27 11:14
    /// 备注：
    internal class QQWryLocator
    {
        #region Fields

        /// <summary>
        /// 有效Ip数据库记录行数
        /// </summary>
        /// 时间：2016/5/30 22:26
        /// 备注：
        public readonly long qqWryRecCount = 0;

        private long lastIpOffset;
        private byte[] qqWryData;
        private long startIpOffset;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataPath">QQwry纯真IP数据库路径</param>
        /// 时间：2016/5/30 22:26
        /// 备注：
        public QQWryLocator(string dataPath)
        {
            ValidateHelper.Begin().NotNullOrEmpty(dataPath, "QQwry纯真IP数据库路径").IsFilePath(dataPath, "QQwry纯真IP数据库");

            qqWryRecCount = LoadQQWryData(dataPath);
            ValidateHelper.Begin().CheckLessThan<long>(qqWryRecCount, "非QQwry纯真IP数据库", 1, false);
        }

        #endregion Constructors

        #region Methods

        public IPLocation Query(string ip)
        {
            ValidateHelper.Begin().IsIp(ip, "IP地址格式");
            IPLocation _ipLocation = new IPLocation() { IP = ip };
            long _convertIpValue = ParseIpString(ip);
            if ((_convertIpValue >= ParseIpString("127.0.0.1") && (_convertIpValue <= ParseIpString("127.255.255.255"))))
            {
                _ipLocation.Country = "本机内部环回地址";
                _ipLocation.Local = string.Empty;
            }
            else
            {
                if ((((_convertIpValue >= ParseIpString("0.0.0.0")) && (_convertIpValue <= ParseIpString("2.255.255.255"))) || ((_convertIpValue >= ParseIpString("64.0.0.0")) && (_convertIpValue <= ParseIpString("126.255.255.255")))) ||
                ((_convertIpValue >= ParseIpString("58.0.0.0")) && (_convertIpValue <= ParseIpString("60.255.255.255"))))
                {
                    _ipLocation.Country = "网络保留地址";
                    _ipLocation.Local = string.Empty;
                }
            }
            long _rightIndex = qqWryRecCount;
            long _leftIndex = 0L;
            long _middleIndex = 0L;
            long _startIp = 0L;
            long _endIpOff = 0L;
            long _endIp = 0L;
            int _countryFlag = 0;
            while (_leftIndex < (_rightIndex - 1L))
            {
                _middleIndex = (_rightIndex + _leftIndex) / 2L;
                _startIp = GetStartIp(_middleIndex, out _endIpOff);
                if (_convertIpValue == _startIp)
                {
                    _leftIndex = _middleIndex;
                    break;
                }
                if (_convertIpValue > _startIp)
                {
                    _leftIndex = _middleIndex;
                }
                else
                {
                    _rightIndex = _middleIndex;
                }
            }
            _startIp = GetStartIp(_leftIndex, out _endIpOff);
            _endIp = GetEndIp(_endIpOff, out _countryFlag);
            if ((_startIp <= _convertIpValue) && (_endIp >= _convertIpValue))
            {
                string _local;
                _ipLocation.Country = GetCountry(_endIpOff, _countryFlag, out _local);
                _ipLocation.Local = _local;
            }
            else
            {
                _ipLocation.Country = "未知";
                _ipLocation.Local = string.Empty;
            }
            return _ipLocation;
        }

        private static long ParseIpString(string ip)
        {
            char[] _array = new char[] { '.' };
            if (ip.Split(_array).Length == 3)
            {
                ip = ip + ".0";
            }
            string[] _ipArray = ip.Split(_array);
            long _ipAddres1 = ((long.Parse(_ipArray[0]) * 0x100L) * 0x100L) * 0x100L,
                 _ipAddres2 = (long.Parse(_ipArray[1]) * 0x100L) * 0x100L,
                 _ipAddres3 = long.Parse(_ipArray[2]) * 0x100L,
                  _ipAddres4 = long.Parse(_ipArray[3]);
            return (((_ipAddres1 + _ipAddres2) + _ipAddres3) + _ipAddres4);
        }

        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            string _country = string.Empty;
            long _offset = endIpOff + 4L;
            switch (countryFlag)
            {
                case 1:
                case 2:
                    _country = GetFlagString(ref _offset, ref countryFlag, ref endIpOff);
                    _offset = endIpOff + 8L;
                    local = (1 == countryFlag) ? "" : GetFlagString(ref _offset, ref countryFlag, ref endIpOff);
                    break;

                default:
                    _country = GetFlagString(ref _offset, ref countryFlag, ref endIpOff);
                    local = GetFlagString(ref _offset, ref countryFlag, ref endIpOff);
                    break;
            }
            return _country;
        }

        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] _buffer = new byte[5];
            Array.Copy(qqWryData, endIpOff, _buffer, 0, 5);
            countryFlag = _buffer[4];
            return ((Convert.ToInt64(_buffer[0].ToString()) + (Convert.ToInt64(_buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(_buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(_buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        private string GetFlagString(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            int _flag = 0;
            byte[] _buffer = new byte[3];

            while (true)
            {
                long _forwardOffset = offset;
                _flag = qqWryData[_forwardOffset++];
                if (_flag != 1 && _flag != 2)
                {
                    break;
                }
                Array.Copy(qqWryData, _forwardOffset, _buffer, 0, 3);
                _forwardOffset += 3;
                if (_flag == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = (Convert.ToInt64(_buffer[0].ToString()) + (Convert.ToInt64(_buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(_buffer[2].ToString()) * 0x100L) * 0x100L);
            }
            if (offset < 12L)
            {
                return string.Empty;
            }
            return GetFlagString(ref offset);
        }

        private string GetFlagString(ref long offset)
        {
            byte _lowByte = 0;
            byte _highByte = 0;
            StringBuilder _builder = new StringBuilder();
            byte[] _data = new byte[2];
            Encoding _encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                _lowByte = qqWryData[offset++];
                if (_lowByte == 0)
                {
                    return _builder.ToString();
                }
                if (_lowByte > 0x7f)
                {
                    _highByte = qqWryData[offset++];
                    _data[0] = _lowByte;
                    _data[1] = _highByte;
                    if (_highByte == 0)
                    {
                        return _builder.ToString();
                    }
                    _builder.Append(_encoding.GetString(_data));
                }
                else
                {
                    _builder.Append((char)_lowByte);
                }
            }
        }

        private long GetStartIp(long left, out long endIpOff)
        {
            long _leftOffset = startIpOffset + (left * 7L);
            byte[] _buffer = new byte[7];
            Array.Copy(qqWryData, _leftOffset, _buffer, 0, 7);
            endIpOff = (Convert.ToInt64(_buffer[4].ToString()) + (Convert.ToInt64(_buffer[5].ToString()) * 0x100L)) + ((Convert.ToInt64(_buffer[6].ToString()) * 0x100L) * 0x100L);
            return ((Convert.ToInt64(_buffer[0].ToString()) + (Convert.ToInt64(_buffer[1].ToString()) * 0x100L)) + ((Convert.ToInt64(_buffer[2].ToString()) * 0x100L) * 0x100L)) + (((Convert.ToInt64(_buffer[3].ToString()) * 0x100L) * 0x100L) * 0x100L);
        }

        /// <summary>
        /// 加载QQwry纯真IP数据库
        /// </summary>
        /// <param name="dataPath">QQwry纯真IP数据库路径</param>
        /// <returns></returns>
        /// 时间：2016/5/30 22:23
        /// 备注：
        private long LoadQQWryData(string dataPath)
        {
            using (FileStream stream = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                qqWryData = new byte[stream.Length];
                stream.Read(qqWryData, 0, qqWryData.Length);
            }
            byte[] _buffer = new byte[8];
            Array.Copy(qqWryData, 0, _buffer, 0, 8);
            startIpOffset = ((_buffer[0] + (_buffer[1] * 0x100)) + ((_buffer[2] * 0x100) * 0x100)) + (((_buffer[3] * 0x100) * 0x100) * 0x100);
            lastIpOffset = ((_buffer[4] + (_buffer[5] * 0x100)) + ((_buffer[6] * 0x100) * 0x100)) + (((_buffer[7] * 0x100) * 0x100) * 0x100);
            return Convert.ToInt64((double)(((double)(lastIpOffset - startIpOffset)) / 7.0));
        }

        #endregion Methods
    }
}