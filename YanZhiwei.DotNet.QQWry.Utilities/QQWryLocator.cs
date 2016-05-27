using System;
using System.IO;
using System.Text;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.QQWry.Utilities
{
    /// <summary>
    /// QQwry纯真IP数据库辅助类
    /// </summary>
    /// 时间：2016-05-27 11:14
    /// 备注：
    public class QQWryLocator
    {
        ///// <summary>
        ///// QQwry纯真IP数据库路径
        ///// </summary>
        ///// 时间：2016-05-27 11:17
        ///// 备注：
        //public readonly string QQWryPath = null;

        //public QQWryLocator(string qqWryPath)
        //{
        //    ValidateHelper.Begin().NotNullOrEmpty(qqWryPath, "QQwry纯真IP数据库路径").IsFilePath(qqWryPath, "QQwry纯真IP数据库路径");
        //    QQWryPath = qqWryPath;
        //}

        //private long GetQQWryCount()
        //{
        //    byte[] data = null;
        //    using (FileStream fileStream = new FileStream(QQWryPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        data = new byte[fileStream.Length];
        //        fileStream.Read(data, 0, data.Length);
        //    }
        //    byte[] array = new byte[8];
        //    Array.Copy(data, 0, array, 0, 8);
        //    long _startIpOffset = (long)((int)array[0] + (int)array[1] * 256 + (int)array[2] * 256 * 256 + (int)array[3] * 256 * 256 * 256);
        //    long _lastIpOffset = (long)((int)array[4] + (int)array[5] * 256 + (int)array[6] * 256 * 256 + (int)array[7] * 256 * 256 * 256);
        //    long ipCount = Convert.ToInt64((double)(_lastIpOffset - _startIpOffset) / 7.0);
        //    return ipCount;
        //}

        private byte[] data;

        private Regex regex = new Regex("(((\\d{1,2})|(1\\d{2})|(2[0-4]\\d)|(25[0-5]))\\.){3}((\\d{1,2})|(1\\d{2})|(2[0-4]\\d)|(25[0-5]))");

        private long firstStartIpOffset;

        private long lastStartIpOffset;

        private long ipCount;

        public long Count
        {
            get
            {
                return this.ipCount;
            }
        }

        public QQWryLocator(string dataPath)
        {
            using (FileStream fileStream = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this.data = new byte[fileStream.Length];
                fileStream.Read(this.data, 0, this.data.Length);
            }
            byte[] array = new byte[8];
            Array.Copy(this.data, 0, array, 0, 8);
            this.firstStartIpOffset = (long)((int)array[0] + (int)array[1] * 256 + (int)array[2] * 256 * 256 + (int)array[3] * 256 * 256 * 256);
            this.lastStartIpOffset = (long)((int)array[4] + (int)array[5] * 256 + (int)array[6] * 256 * 256 + (int)array[7] * 256 * 256 * 256);
            this.ipCount = Convert.ToInt64((double)(this.lastStartIpOffset - this.firstStartIpOffset) / 7.0);
            if (this.ipCount <= 1L)
            {
                throw new ArgumentException("ip FileDataError");
            }
        }

        private static long IpToInt(string ip)
        {
            char[] separator = new char[]
            {
                '.'
            };
            if (ip.Split(separator).Length == 3)
            {
                ip += ".0";
            }
            string[] array = ip.Split(separator);
            long num = long.Parse(array[0]) * 256L * 256L * 256L;
            long num2 = long.Parse(array[1]) * 256L * 256L;
            long num3 = long.Parse(array[2]) * 256L;
            long num4 = long.Parse(array[3]);
            return num + num2 + num3 + num4;
        }

        private static string IntToIP(long ip_Int)
        {
            long num = (ip_Int & (long)((ulong)-16777216)) >> 24;
            if (num < 0L)
            {
                num += 256L;
            }
            long num2 = (ip_Int & 16711680L) >> 16;
            if (num2 < 0L)
            {
                num2 += 256L;
            }
            long num3 = (ip_Int & 65280L) >> 8;
            if (num3 < 0L)
            {
                num3 += 256L;
            }
            long num4 = ip_Int & 255L;
            if (num4 < 0L)
            {
                num4 += 256L;
            }
            return string.Concat(new string[]
            {
                num.ToString(),
                ".",
                num2.ToString(),
                ".",
                num3.ToString(),
                ".",
                num4.ToString()
            });
        }

        public IPLocation Query(string ip)
        {
            if (!this.regex.Match(ip).Success)
            {
                throw new ArgumentException("IP格式错误");
            }
            IPLocation iPLocation = new IPLocation
            {
                IP = ip
            };
            long num = QQWryLocator.IpToInt(ip);
            if (num >= QQWryLocator.IpToInt("127.0.0.1") && num <= QQWryLocator.IpToInt("127.255.255.255"))
            {
                iPLocation.Country = "本机内部环回地址";
                iPLocation.Local = "";
            }
            else if ((num >= QQWryLocator.IpToInt("0.0.0.0") && num <= QQWryLocator.IpToInt("2.255.255.255")) || (num >= QQWryLocator.IpToInt("64.0.0.0") && num <= QQWryLocator.IpToInt("126.255.255.255")) || (num >= QQWryLocator.IpToInt("58.0.0.0") && num <= QQWryLocator.IpToInt("60.255.255.255")))
            {
                iPLocation.Country = "网络保留地址";
                iPLocation.Local = "";
            }
            long num2 = this.ipCount;
            long num3 = 0L;
            long endIpOff = 0L;
            int countryFlag = 0;
            long startIp;
            while (num3 < num2 - 1L)
            {
                long num4 = (num2 + num3) / 2L;
                startIp = this.GetStartIp(num4, out endIpOff);
                if (num == startIp)
                {
                    num3 = num4;
                    break;
                }
                if (num > startIp)
                {
                    num3 = num4;
                }
                else
                {
                    num2 = num4;
                }
            }
            startIp = this.GetStartIp(num3, out endIpOff);
            long endIp = this.GetEndIp(endIpOff, out countryFlag);
            if (startIp <= num && endIp >= num)
            {
                string local;
                iPLocation.Country = this.GetCountry(endIpOff, countryFlag, out local);
                iPLocation.Local = local;
            }
            else
            {
                iPLocation.Country = "未知";
                iPLocation.Local = "";
            }
            return iPLocation;
        }

        private long GetStartIp(long left, out long endIpOff)
        {
            long sourceIndex = this.firstStartIpOffset + left * 7L;
            byte[] array = new byte[7];
            Array.Copy(this.data, sourceIndex, array, 0L, 7L);
            endIpOff = Convert.ToInt64(array[4].ToString()) + Convert.ToInt64(array[5].ToString()) * 256L + Convert.ToInt64(array[6].ToString()) * 256L * 256L;
            return Convert.ToInt64(array[0].ToString()) + Convert.ToInt64(array[1].ToString()) * 256L + Convert.ToInt64(array[2].ToString()) * 256L * 256L + Convert.ToInt64(array[3].ToString()) * 256L * 256L * 256L;
        }

        private long GetEndIp(long endIpOff, out int countryFlag)
        {
            byte[] array = new byte[5];
            Array.Copy(this.data, endIpOff, array, 0L, 5L);
            countryFlag = (int)array[4];
            return Convert.ToInt64(array[0].ToString()) + Convert.ToInt64(array[1].ToString()) * 256L + Convert.ToInt64(array[2].ToString()) * 256L * 256L + Convert.ToInt64(array[3].ToString()) * 256L * 256L * 256L;
        }

        private string GetCountry(long endIpOff, int countryFlag, out string local)
        {
            long num = endIpOff + 4L;
            string flagStr;
            switch (countryFlag)
            {
                case 1:
                case 2:
                    flagStr = this.GetFlagStr(ref num, ref countryFlag, ref endIpOff);
                    num = endIpOff + 8L;
                    local = ((1 == countryFlag) ? "" : this.GetFlagStr(ref num, ref countryFlag, ref endIpOff));
                    break;
                default:
                    flagStr = this.GetFlagStr(ref num, ref countryFlag, ref endIpOff);
                    local = this.GetFlagStr(ref num, ref countryFlag, ref endIpOff);
                    break;
            }
            return flagStr;
        }

        private string GetFlagStr(ref long offset, ref int countryFlag, ref long endIpOff)
        {
            byte[] array = new byte[3];
            while (true)
            {
                long num = offset;
                byte[] arg_19_0 = this.data;
                long expr_13 = num;
                num = expr_13 + 1L;
                int num2 = arg_19_0[(int)(checked((IntPtr)expr_13))];
                if (num2 != 1 && num2 != 2)
                {
                    break;
                }
                Array.Copy(this.data, num, array, 0L, 3L);
                num += 3L;
                if (num2 == 2)
                {
                    countryFlag = 2;
                    endIpOff = offset - 4L;
                }
                offset = Convert.ToInt64(array[0].ToString()) + Convert.ToInt64(array[1].ToString()) * 256L + Convert.ToInt64(array[2].ToString()) * 256L * 256L;
            }
            if (offset < 12L)
            {
                return "";
            }
            return this.GetStr(ref offset);
        }

        private string GetStr(ref long offset)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = new byte[2];
            Encoding encoding = Encoding.GetEncoding("GB2312");
            while (true)
            {
                byte[] arg_30_0 = this.data;
                long num;
                offset = (num = offset) + 1L;
                byte b = arg_30_0[(int)(checked((IntPtr)num))];
                if (b == 0)
                {
                    break;
                }
                if (b > 127)
                {
                    byte[] arg_54_0 = this.data;
                    long num2;
                    offset = (num2 = offset) + 1L;
                    byte b2 = arg_54_0[(int)(checked((IntPtr)num2))];
                    array[0] = b;
                    array[1] = b2;
                    if (b2 == 0)
                    {
                        goto Block_3;
                    }
                    stringBuilder.Append(encoding.GetString(array));
                }
                else
                {
                    stringBuilder.Append((char)b);
                }
            }
            return stringBuilder.ToString();
        }
    }
}