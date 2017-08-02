namespace YanZhiwei.DotNet3._5.Utilities.TypeConverter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    using SysComponentModel = System.ComponentModel;

    /// <summary>
    /// 通用词典类型TypeConverter
    /// </summary>
    /// <typeparam name="K">Key type (simple)</typeparam>
    /// <typeparam name="V">Value type (simple)</typeparam>
    public class GenericDictionaryTypeConverter<K, V> : SysComponentModel.TypeConverter
    {
        #region Fields

        /// <summary>
        /// The type converter key
        /// </summary>
        protected readonly SysComponentModel.TypeConverter typeConverterKey;

        /// <summary>
        /// The type converter value
        /// </summary>
        protected readonly SysComponentModel.TypeConverter typeConverterValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 类型不存在类型转换器" + typeof(K).FullName
        /// or
        /// 类型不存在类型转换器" + typeof(V).FullName
        /// </exception>
        public GenericDictionaryTypeConverter()
        {
            typeConverterKey = TypeDescriptor.GetConverter(typeof(K));
            if (typeConverterKey == null)
                throw new InvalidOperationException("类型不存在类型转换器" + typeof(K).FullName);
            typeConverterValue = TypeDescriptor.GetConverter(typeof(V));
            if (typeConverterValue == null)
                throw new InvalidOperationException("类型不存在类型转换器" + typeof(V).FullName);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型。
        /// </summary>
        /// <param name="context"><see cref="T:System.ComponentModel.ITypeDescriptorContext" />，提供格式上下文。</param>
        /// <param name="sourceType">一个 <see cref="T:System.Type" />，表示要转换的类型。</param>
        /// <returns>
        /// 如果该转换器能够执行转换，则为 true；否则为 false。
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的对象转换为此转换器的类型。
        /// </summary>
        /// <param name="context"><see cref="T:System.ComponentModel.ITypeDescriptorContext" />，提供格式上下文。</param>
        /// <param name="culture">用作当前区域性的 <see cref="T:System.Globalization.CultureInfo" />。</param>
        /// <param name="value">要转换的 <see cref="T:System.Object" />。</param>
        /// <returns>
        /// 表示转换的 value 的 <see cref="T:System.Object" />。
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string _input = (string)value;
                string[] _items = string.IsNullOrEmpty(_input) ? new string[0] : _input.Split(';').Select(x => x.Trim()).ToArray();

                var _result = new Dictionary<K, V>();
                Array.ForEach(_items, s =>
                {
                    string[] _keyValueStr = string.IsNullOrEmpty(s) ? new string[0] : s.Split(',').Select(x => x.Trim()).ToArray();
                    if (_keyValueStr.Length == 2)
                    {
                        object _dictionaryKey = (K)typeConverterKey.ConvertFromInvariantString(_keyValueStr[0]);
                        object _dictionaryValue = (V)typeConverterValue.ConvertFromInvariantString(_keyValueStr[1]);
                        if (_dictionaryKey != null && _dictionaryValue != null)
                        {
                            if (!_result.ContainsKey((K)_dictionaryKey))
                                _result.Add((K)_dictionaryKey, (V)_dictionaryValue);
                        }
                    }
                });

                return _result;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的值对象转换为指定的类型。
        /// </summary>
        /// <param name="context"><see cref="T:System.ComponentModel.ITypeDescriptorContext" />，提供格式上下文。</param>
        /// <param name="culture"><see cref="T:System.Globalization.CultureInfo" />。如果传递 null，则采用当前区域性。</param>
        /// <param name="value">要转换的 <see cref="T:System.Object" />。</param>
        /// <param name="destinationType"><paramref name="value" /> 参数要转换成的 <see cref="T:System.Type" />。</param>
        /// <returns>
        /// 表示转换的 value 的 <see cref="T:System.Object" />。
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string _result = string.Empty;
                if (value != null)
                {
                    int _counter = 0;
                    var _dictionary = (IDictionary<K, V>)value;
                    foreach (var keyValue in _dictionary)
                    {
                        _result += string.Format("{0}, {1}", Convert.ToString(keyValue.Key, CultureInfo.InvariantCulture), Convert.ToString(keyValue.Value, CultureInfo.InvariantCulture));
                        if (_counter != _dictionary.Count - 1)
                            _result += ";";
                        _counter++;
                    }
                }
                return _result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion Methods
    }
}