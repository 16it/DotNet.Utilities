﻿namespace YanZhiwei.DotNet2.Utilities.Common
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;

    using YanZhiwei.DotNet2.Utilities.Enum;

    /// <summary>
    /// 转换帮助类
    /// </summary>
    public static class ConvertHelper
    {
        #region Methods

        /// <summary>
        /// 转换成布尔类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static bool ToBooleanOrDefault(this object data, bool defalut)
        {
            bool result = false;

            if (data != null)
            {
                if (bool.TryParse(data.ToString(), out result))
                {
                    return result;
                }
            }

            return defalut;
        }

        /// <summary>
        /// 转换成Byte类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static byte ToByteOrDefault(this object data, byte defalut)
        {
            if (data != null)
            {
                if (byte.TryParse(data.ToString(), out byte result))
                {
                    return result;
                }
            }

            return defalut;
        }

        /// <summary>
        /// 转换为农历年
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>农历年</returns>
        public static string ToChineseDate(this DateTime date)
        {
            ChineseLunisolarCalendar cnDate = new ChineseLunisolarCalendar();
            string[] months = { string.Empty, "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "冬月", "腊月" };
            string[] days = { string.Empty, "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" };
            string[] years = { string.Empty, "甲子", "乙丑", "丙寅", "丁卯", "戊辰", "己巳", "庚午", "辛未", "壬申", "癸酉", "甲戌", "乙亥", "丙子", "丁丑", "戊寅", "己卯", "庚辰", "辛己", "壬午", "癸未", "甲申", "乙酉", "丙戌", "丁亥", "戊子", "己丑", "庚寅", "辛卯", "壬辰", "癸巳", "甲午", "乙未", "丙申", "丁酉", "戊戌", "己亥", "庚子", "辛丑", "壬寅", "癸丑", "甲辰", "乙巳", "丙午", "丁未", "戊申", "己酉", "庚戌", "辛亥", "壬子", "癸丑", "甲寅", "乙卯", "丙辰", "丁巳", "戊午", "己未", "庚申", "辛酉", "壬戌", "癸亥" };
            int year = cnDate.GetYear(date);
            string year_cn = years[cnDate.GetSexagenaryYear(date)];
            int month = cnDate.GetMonth(date),
                day = cnDate.GetDayOfMonth(date),
                leapMonth = cnDate.GetLeapMonth(year);
            string month_cn = months[month];

            if (leapMonth > 0)
            {
                month_cn = month == leapMonth ? string.Format("闰{0}", months[month - 1]) : month_cn;
                month_cn = month > leapMonth ? months[month - 1] : month_cn;
            }

            return string.Format("{0}年{1}{2}", year_cn, month_cn, days[day]);
        }

        /// <summary>
        /// 将阿拉伯数字转换中文日期数字
        /// </summary>
        /// <param name="data">日期范围1~31</param>
        /// <returns>中文日期数字</returns>
        public static string ToChineseDay(int data)
        {
            string _reulst = string.Empty;

            if (!(data == 0 || data > 32))
            {
                string[] _days = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "廿十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十", "三十一" };
                _reulst = _days[data];
            }

            return _reulst;
        }

        /// <summary>
        /// 将阿拉伯数字转换成中文月份数字
        /// <para>eg:ConvertHelper.ToChineseMonth(1)==> "一"</para>
        /// </summary>
        /// <param name="data">月份范围1~12</param>
        /// <returns>中文月份数字</returns>
        public static string ToChineseMonth(this int data)
        {
            string _result = string.Empty;

            if (!(data == 0 || data > 12))
            {
                string[] _months = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
                _result = _months[data];
            }

            return _result;
        }

        /// <summary>
        /// 转换成日期
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>日期</returns>
        public static DateTime ToDateOrDefault(this object data, DateTime defalut)
        {
            if (data != null)
            {
                return DateTime.TryParse(data.ToString(), out DateTime _result) ? _result : defalut;
            }

            return defalut;
        }

        /// <summary>
        ///  转换成decimal类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static decimal ToDecimalOrDefault(this object data, decimal defalut)
        {
            if (data != null)
            {
                bool _parseResult = decimal.TryParse(data.ToString(), out decimal _parsedecimalValue);
                return _parseResult == true ? _parsedecimalValue : defalut;
            }

            return defalut;
        }

        /// <summary>
        /// 转换成double类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static double ToDoubleOrDefault(this object data, double defalut)
        {
            if (data != null)
            {
                bool _parseResult = double.TryParse(data.ToString(), out double _parseIntValue);
                return _parseResult == true ? _parseIntValue : defalut;
            }

            return defalut;
        }

        /// <summary>
        /// 二，八，十，十六进制互相转换
        /// 说明：
        /// 若转换成二进制，会补足八个长度
        /// </summary>
        /// <param name="data">需要转换的字符串</param>
        /// <param name="from">原始进制</param>
        /// <param name="to">目标进制</param>
        /// <returns>转换结果；若转换失败返回"0"</returns>
        public static string ToHexBinDecOct(this string data, HexadecimalType from, HexadecimalType to)
        {
            try
            {
                int _intValue = Convert.ToInt32(data, (int)from);
                string _targetValue = Convert.ToString(_intValue, (int)to);

                if (to == HexadecimalType.Binary)
                {
                    _targetValue = StringHelper.ComplementLeftZero(_targetValue, 8);
                }

                return _targetValue;
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// 转换成Int32类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static int ToInt32OrDefault(this object data, int defalut)
        {
            if (data != null)
            {
                bool _parseResult = int.TryParse(data.ToString(), out int _parseIntValue);
                return _parseResult == true ? _parseIntValue : defalut;
            }

            return defalut;
        }

        /// <summary>
        /// 转换成Int64类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static long ToInt64OrDefault(this object data, long defalut)
        {
            if (data != null)
            {
                bool _parseResult = long.TryParse(data.ToString(), out long _parseIntValue);
                return _parseResult == true ? _parseIntValue : defalut;
            }

            return defalut;
        }

        /// <summary>
        /// 转换成Int类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static int ToIntOrDefault(this object data, int defalut)
        {
            if (data != null)
            {
                bool _parseResult = int.TryParse(data.ToString(), out int _parseIntValue);
                return _parseResult == true ? _parseIntValue : defalut;
            }

            return defalut;
        }

        /// <summary>
        /// 按照列名称获取Int值
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="columnName">列名称</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>若列不等于NULL则返回实际值</returns>
        public static int ToIntOrDefault(this DataRow row, string columnName, int defalut)
        {
            if (row != null)
            {
                if (row.IsNull(columnName))
                {
                    int.TryParse(row[columnName].ToString(), out defalut);
                }
            }

            return defalut;
        }

        /// <summary>
        /// 按照列索引获取Int值
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>若列不等于NULL则返回实际值</returns>
        public static int ToIntOrDefault(this DataRow row, int columnIndex, int defalut)
        {
            if (row != null)
            {
                if (row.IsNull(columnIndex))
                {
                    int.TryParse(row[columnIndex].ToString(), out defalut);
                }
            }

            return defalut;
        }

        /// <summary>
        /// 转换成Int16类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static short ToShortOrDefault(this object data, short defalut)
        {
            if (data != null)
            {
                bool _parseResult = short.TryParse(data.ToString(), out short _parseIntValue);
                return _parseResult == true ? _parseIntValue : defalut;
            }

            return defalut;
        }

        /// <summary>
        /// 泛型数组转换为字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">泛型数组</param>
        /// <param name="delimiter">分隔符</param>
        /// <returns>转换好的字符串</returns>
        public static string ToString<T>(this T[] array, string delimiter)
        {
            string[] _array = Array.ConvertAll<T, string>(array, n => n.ToString());
            return string.Join(delimiter, _array);
        }

        /// <summary>
        /// 字符串类型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">需要转换的字符串</param>
        /// <returns>转换类型</returns>
        public static T ToStringBase<T>(this string data)
        {
            T _result = default(T);

            if (!string.IsNullOrEmpty(data))
            {
                TypeConverter _convert = TypeDescriptor.GetConverter(typeof(T));
                _result = (T)_convert.ConvertFrom(data);
            }

            return _result;
        }

        /// <summary>
        /// 转换成string类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static string ToStringOrDefault(this object data, string defalut)
        {
            return data == null ? defalut : data.ToString();
        }

        /// <summary>
        /// 将时间类型转换为字符串表述
        /// </summary>
        /// <param name="data">DateTime</param>
        /// <param name="formartString">格式化字符串</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>字符串</returns>
        public static string ToStringOrDefault(this DateTime data, string formartString, string defalut)
        {
            if (data != default(DateTime))
            {
                return data.ToString(formartString);
            }

            return defalut;
        }

        /// <summary>
        /// 按照列名称获取Sting值
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="columnName">列名称</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>若列不等于NULL则返回实际值</returns>
        public static string ToStringOrDefault(this DataRow row, string columnName, string defalut)
        {
            if (row != null)
            {
                defalut = row.IsNull(columnName) == true ? defalut : row[columnName].ToString();
            }

            return defalut;
        }

        /// <summary>
        /// 按照列索引获取Sting值
        /// </summary>
        /// <param name="row">DataRow</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>若列不等于NULL则返回实际值</returns>
        public static string ToStringOrDefault(this DataRow row, int columnIndex, string defalut)
        {
            if (row != null)
            {
                defalut = row.IsNull(columnIndex) == true ? defalut : row[columnIndex].ToString().Trim();
            }

            return defalut;
        }

        /// <summary>
        /// 转换成ushort类型
        /// </summary>
        /// <param name="data">需要转换的object</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static ushort ToUShortOrDefault(this object data, ushort defalut)
        {
            if (data != null)
            {
                bool _parseResult = ushort.TryParse(data.ToString(), out ushort _parseUShortValue);
                return _parseResult == true ? _parseUShortValue : defalut;
            }

            return defalut;
        }

        #endregion Methods
    }
}