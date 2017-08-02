namespace YanZhiwei.DotNet3._5.Utilities.TypeConverter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    using SysComponentModel = System.ComponentModel;

    /// <summary>
    /// 通用列表类型TypeConverter
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class GenericListTypeConverter<T> : SysComponentModel.TypeConverter
    {
        #region Fields

        /// <summary>
        /// TypeConverter
        /// </summary>
        protected readonly SysComponentModel.TypeConverter typeConverter;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///构造函数
        /// </summary>
        /// <exception cref="InvalidOperationException">类型不存在类型转换器 " + typeof(T).FullName</exception>
        public GenericListTypeConverter()
        {
            typeConverter = SysComponentModel.TypeDescriptor.GetConverter(typeof(T));

            if (typeConverter == null)
                throw new InvalidOperationException("类型不存在类型转换器 " + typeof(T).FullName);
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
            {
                string[] items = GetStringArray(sourceType.ToString());
                return items.Any();
            }

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
                string[] _items = GetStringArray((string)value);
                var _result = new List<T>();
                Array.ForEach(_items, s =>
                {
                    object _item = typeConverter.ConvertFromInvariantString(s);

                    if (_item != null)
                    {
                        _result.Add((T)_item);
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
                    for (int i = 0; i < ((IList<T>)value).Count; i++)
                    {
                        var _temp = Convert.ToString(((IList<T>)value)[i], CultureInfo.InvariantCulture);
                        _result += _temp;

                        if (i != ((IList<T>)value).Count - 1)
                            _result += ",";
                    }
                }

                return _result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// 从逗号分隔的字符串中获取字符串数组
        /// </summary>
        /// <param name="input">分隔的字符串</param>
        /// <returns>数组</returns>
        protected virtual string[] GetStringArray(string input)
        {
            return string.IsNullOrEmpty(input) ? new string[0] : input.Split(',').Select(x => x.Trim()).ToArray();
        }

        #endregion Methods
    }
}