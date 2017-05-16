namespace YanZhiwei.DotNet2.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// string帮助类
    /// </summary>
    public static class StringHelper
    {
        #region Methods

        /// <summary>
        /// 自增流水号
        /// <para>eg:abcd2-->abcd3</para>
        /// </summary>
        /// <param name="data">The input.</param>
        /// <returns>自增流水号</returns>
        /// 创建时间:2015-05-22 14:45
        /// 备注说明:<c>null</c>
        public static string AutoIncrementSeqNo(string data)
        {
            /*
             *参考:
             *1. http://www.dotblogs.com.tw/neptunic/archive/2010/12/26/autoincrementindex.aspx
             */
            int _numberIndex = 0;
            int _checkIndex = 1;

            foreach (char c in data)
            {
                if (!char.IsNumber(c))
                {
                    _numberIndex = _checkIndex;
                }

                _checkIndex++;
            }

            string _numbers = (_numberIndex == data.Length) ? string.Empty : data.Substring(_numberIndex, data.Length - _numberIndex);
            string _seqNumber = string.IsNullOrEmpty(_numbers) ? string.Format("{0}1", data) : string.Format("{0}{1}", data.Remove(_numberIndex), (Convert.ToInt64(_numbers) + 1).ToString().PadLeft(data.Length - _numberIndex, '0'));
            return _seqNumber;
        }

        /// <summary>
        /// 对字符串遍历分割
        /// <para>eg: StringHelper.BuilderDelimiter("Yan", '-');==>"Y-a-n"</para>
        /// </summary>
        /// <param name="data">需要分割的字符串</param>
        /// <param name="delimiter">每个字符分割符号</param>
        /// <returns>分割好的字符串</returns>
        public static string BuilderDelimiter(this string data, char delimiter)
        {
            char[] _chars = data.ToCharArray();
            StringBuilder _builder = new StringBuilder();

            foreach (char c in _chars)
            {
                _builder.AppendFormat("{0}{1}", c, delimiter);
            }

            string _resultStr = _builder.ToString();
            int _invalid = _resultStr.LastIndexOf(delimiter);

            if (_invalid != -1)
            {
                _resultStr = _resultStr.Substring(0, _invalid);
            }

            return _resultStr;
        }

        /// <summary>
        /// 清除字符串内空格
        /// <para>eg:StringHelper.ClearBlanks(" 11 22 33 44  ");==>11223344</para>
        /// </summary>
        /// <param name="data">需要处理的字符串</param>
        /// <returns>处理好后的字符串</returns>
        public static string ClearBlanks(this string data)
        {
            int _length = data.Length;
            StringBuilder _builder = new StringBuilder(_length);

            for (int i = 0; i < data.Length; i++)
            {
                char _c = data[i];

                if (!char.IsWhiteSpace(_c))
                {
                    _builder.Append(_c);
                }
            }

            return _builder.ToString();
        }

        /// <summary>
        /// 忽略大小写比较
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="compareData">比较字符串</param>
        /// <returns>是否相等</returns>
        /// 时间：2016/8/29 9:14
        /// 备注：
        public static bool CompareIgnoreCase(this string data, string compareData)
        {
            return string.Equals(data, compareData, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 补足位数_左边
        /// <para>eg:StringHelper.ComplementLeftZero("Yanzhiwei", 15);==>"000000Yanzhiwei"</para>
        /// </summary>
        /// <param name="data">需要操作的字符串</param>
        /// <param name="targetLength">目标长度</param>
        /// <returns>操作完成后字符串</returns>
        public static string ComplementLeftZero(this string data, int targetLength)
        {
            int _curLength = data.Length;

            if (_curLength < targetLength)
            {
                StringBuilder _builder = new StringBuilder(targetLength);

                for (int i = 0; i < targetLength - data.Length; i++)
                {
                    _builder.Append("0");
                }

                _builder.Append(data);
                return _builder.ToString();
            }

            return data;
        }

        /// <summary>
        /// 补足位数_右边
        /// <para>eg:StringHelper.ComplementRigthZero("Yanzhiwei", 15);==>Yanzhiwei000000</para>
        /// </summary>
        /// <param name="data">需要操作的字符串</param>
        /// <param name="targetLength">目标长度</param>
        /// <returns>操作完成后字符串</returns>
        public static string ComplementRigthZero(this string data, int targetLength)
        {
            int _curLength = data.Length;

            if (_curLength < targetLength)
            {
                StringBuilder _builder = new StringBuilder(targetLength);
                _builder.Append(data);

                for (int i = 0; i < targetLength - data.Length; i++)
                {
                    _builder.Append("0");
                }

                return _builder.ToString();
            }

            return data;
        }

        /// <summary>
        /// 对字符串进行编码
        /// </summary>
        /// <param name="data">需要编码的字符串</param>
        /// <returns>编码后的字符串</returns>
        /// 时间:2016/10/16 13:02
        /// 备注:
        public static string Escape(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                StringBuilder _builder = new StringBuilder();

                foreach (char c in data)
                {
                    _builder.Append((char.IsLetterOrDigit(c)
                                     || c == '-' || c == '_' || c == '\\'
                                     || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
                }

                return _builder.ToString();
            }

            return data;
        }

        /// <summary>
        /// 为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// 截取字符串,超过最大长度则以'...'表示
        /// </summary>
        /// <param name="data">需要截取的字符串</param>
        /// <param name="maxLen">字符串最大长度，超过最大长度则以'...'表示</param>
        /// <returns>截取后字符串</returns>
        public static string GetFriendly(this string data, int maxLen)
        {
            if (maxLen <= 0)
            {
                return data;
            }
            else
            {
                if (data.Length > maxLen)
                {
                    return data.Substring(0, maxLen) + "...";
                }
                else
                {
                    return data;
                }
            }
        }

        /// <summary>
        /// 整除索引处加入符号
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="count">除数</param>
        /// <param name="character">符号</param>
        /// <returns>处理后的字符串</returns>
        public static string InsertCharAtDividedPosition(this string data, int count, string character)
        {
            int _index = 0;

            while (++_index * count + (_index - 1) < data.Length)
            {
                data = data.Insert(_index * count + (_index - 1), character);
            }

            return data;
        }

        /// <summary>
        /// Determines whether [is null or empty].
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>是否为NULL或者empty</returns>
        public static bool IsNullOrEmpty(this string data)
        {
            return string.IsNullOrEmpty(data);
        }

        /// <summary>
        /// 判断空引用和字符串中的每一个字符是否是空格
        /// </summary>
        /// <param name="data">The value.</param>
        /// <returns>否是空格</returns>
        public static bool IsNullOrWhiteSpace(this string data)
        {
            if (data == null)
            {
                return true;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (!char.IsWhiteSpace(data[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 将字符串第一位置为小写
        /// </summary>
        /// <param name="data">需要操作的字符串</param>
        /// <returns>操作后的字符串</returns>
        public static string LowerFirstChar(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                return data.ToLower().Substring(0, 1) + data.Substring(1, data.Length - 1);
            }

            return data;
        }

        /// <summary>
        /// 将千分位字符串转换成数字
        /// <para>eg:StringHelper.ParseThousandthString("111,222,333");==>111222333</para>
        /// </summary>
        /// <param name="data">需要转换的千分位</param>
        /// <returns>数字;若转换失败则返回-1</returns>
        public static int ParseThousandthString(this string data)
        {
            int _value = -1;

            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    _value = int.Parse(data, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                }
                catch (Exception ex)
                {
                    _value = -1;
                    Debug.WriteLine(string.Format("将千分位字符串{0}转换成数字异常，原因:{0}", data, ex.Message));
                }
            }

            return _value;
        }

        /// <summary>
        /// 去除文本中的html代码。
        /// </summary>
        public static string RemoveHtml(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
                return Regex.Replace(inputString, @"<[^>]+>", "");

            return inputString;
        }

        /// <summary>
        /// 移除Json字符串诸如“{”,“}”符号
        /// </summary>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>Json字符串</returns>
        /// 时间：2016/6/29 16:31
        /// 备注：
        public static string RemoveJsonStringSymbol(string jsonString)
        {
            return jsonString.Replace("{", "").Replace("}", "").Replace("\"", "");
        }

        /// <summary>
        /// 字符串逆转
        /// <para>eg:StringHelper.Reverse("YanZhiwei");</para>
        /// </summary>
        /// <param name="data">需要逆转的字符串</param>
        /// <returns>逆转后的字符串</returns>
        public static string Reverse(this string data)
        {
            char[] _chars = data.ToCharArray();
            Array.Reverse(_chars);
            return new string(_chars);
        }

        /// <summary>
        /// 利用Array.Reverse反转字符串
        /// </summary>
        /// <param name="data">操作字符串</param>
        /// <returns>反转字符串</returns>
        public static string ReverseUsingArrayClass(this string data)
        {
            char[] _chars = data.ToCharArray();
            Array.Reverse(_chars);
            return new string(_chars);
        }

        /// <summary>
        /// 利用遍历反转字符串
        /// </summary>
        /// <param name="data">操作字符串</param>
        /// <returns>反转字符串</returns>
        public static string ReverseUsingCharacterBuffer(this string data)
        {
            char[] _charArray = new char[data.Length];
            int _inputStrLength = data.Length - 1;

            for (int i = 0; i <= _inputStrLength; i++)
            {
                _charArray[i] = data[_inputStrLength - i];
            }

            return new string(_charArray);
        }

        /// <summary>
        /// 利用Stack反转字符串
        /// </summary>
        /// <param name="data">操作字符串</param>
        /// <returns>反转字符串</returns>
        public static string ReverseUsingStack(this string data)
        {
            Stack<char> _resultStack = new Stack<char>();

            foreach (char c in data)
            {
                _resultStack.Push(c);
            }

            StringBuilder _builder = new StringBuilder();

            while (_resultStack.Count > 0)
            {
                _builder.Append(_resultStack.Pop());
            }

            return _builder.ToString();
        }

        /// <summary>
        /// 利用StringBuilder反转字符串
        /// </summary>
        /// <param name="data">操作字符串</param>
        /// <returns>反转字符串</returns>
        public static string ReverseUsingStringBuilder(this string data)
        {
            StringBuilder _builder = new StringBuilder(data.Length);

            for (int i = data.Length - 1; i >= 0; i--)
            {
                _builder.Append(data[i]);
            }

            return _builder.ToString();
        }

        /// <summary>
        /// 利用XOR反转字符串
        /// </summary>
        /// <param name="data">操作字符串</param>
        /// <returns>反转字符串</returns>
        public static string ReverseUsingXOR(this string data)
        {
            char[] _charArray = data.ToCharArray();
            int _length = data.Length - 1;

            for (int i = 0; i < _length; i++, _length--)
            {
                _charArray[i] ^= _charArray[_length];
                _charArray[_length] ^= _charArray[i];
                _charArray[i] ^= _charArray[_length];
            }

            return new string(_charArray);
        }

        /// <summary>
        /// 按照符号截取字符串
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>截取后的字符串</returns>
        /// 日期：2015-09-18 11:40
        /// 备注：
        public static string SubString(this string data, char delimiter)
        {
            int _indexof = data.IndexOf(delimiter);

            if (_indexof != -1)
            {
                data = data.Substring(0, _indexof);
            }

            return data;
        }

        /// <summary>
        /// 按照最后符号截取字符串
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>截取后的字符串</returns>
        public static string SubStringFromLast(this string data, char delimiter)
        {
            int _indexof = data.LastIndexOf(delimiter);

            if (_indexof != -1)
            {
                data = data.Substring(0, _indexof);
            }

            return data;
        }

        /// <summary>
        /// 对字符串进行解码
        /// </summary>
        /// <param name="data">需要解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        /// 时间:2016/10/16 13:06
        /// 备注:
        public static string UnEscape(this string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                StringBuilder _builder = new StringBuilder();
                int _len = data.Length;
                int i = 0;

                while (i != _len)
                {
                    if (Uri.IsHexEncoding(data, i))
                        _builder.Append(Uri.HexUnescape(data, ref i));
                    else
                        _builder.Append(data[i++]);
                }

                return _builder.ToString();
            }

            return data;
        }

        /// <summary>
        ///  获取全局唯一值
        /// </summary>
        /// <returns>全局唯一值</returns>
        public static string Unique()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        /// <summary>
        /// 将字符串第一位置为大写
        /// <para>eg:studentName==>StudentName</para>
        /// </summary>
        /// <param name="data">需要操作的字符串</param>
        /// <returns>操作后的字符串</returns>
        public static string UpperFirstChar(this string data)
        {
            return data.ToUpper().Substring(0, 1) + data.Substring(1, data.Length - 1);
        }

        /// <summary>
        /// 文字换行
        /// <para>eg:StringHelper.WrapText("YanZhiwei", 3);==>"Yan\r\nZhi\r\nwei"</para>
        /// </summary>
        /// <param name="data">需要换行的文字</param>
        /// <param name="maxWidth">多少长度换行</param>
        /// <returns>换行好的文字</returns>
        public static string WrapText(this string data, int maxWidth)
        {
            int _stringCount = data.Length;

            if (maxWidth > 0 && _stringCount > maxWidth)
            {
                StringBuilder _builderString = new StringBuilder(data);
                int _breakCount = _builderString.Length / maxWidth;

                for (int i = 0; i < _breakCount; i++)
                {
                    int _insertPosition = i * maxWidth;

                    if (_insertPosition != 0)
                    {
                        int _offset = (i - 1) * 2;
                        _builderString.Insert(_insertPosition + _offset, Environment.NewLine);
                    }
                }

                return _builderString.ToString();
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// 获取中英文长度，中文长度2，其他1
        /// </summary>
        /// <param name="data">需要判断字符串</param>
        /// <returns>长度</returns>
        public static int Length(string data)
        {
            int _length = 0;
            if (!string.IsNullOrEmpty(data))
            {
                ASCIIEncoding _dataAscii = new ASCIIEncoding();
                //将字符串转换为ASCII编码的字节数字
                byte[] _buffer = _dataAscii.GetBytes(data);
                for (int i = 0; i <= _buffer.Length - 1; i++)
                {
                    if (_buffer[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                        _length++;
                    _length++;
                }
            }
            return _length;
        }

        #endregion Methods
    }
}