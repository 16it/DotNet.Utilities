namespace YanZhiwei.DotNet4.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    
    using Attribute;
    
    /// <summary>
    /// EnumTitleAttribute 特性帮助类
    /// </summary>
    public static class EnumTitleAttributeHelper
    {
        #region Fields
        
        /// <summary>
        /// 枚举分隔符
        /// </summary>
        public const string ENUM_TITLE_SEPARATOR = ",";
        
        #endregion Fields
        
        #region Methods
        
        /// <summary>
        /// 根据枚举值，返回描述字符串
        /// 如果多选枚举，返回以","分割的字符串
        /// </summary>
        /// <param name="e">EnumTitleAttribute</param>
        /// <returns>返回描述字符串</returns>
        public static string GetAllEnumTitle(this EnumTitleAttribute e)
        {
            string[] _valueArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type _type = e.GetType();
            StringBuilder _builder = new StringBuilder();
            
            foreach(string enumValue in _valueArray)
            {
                FieldInfo _field = _type.GetField(enumValue.Trim());
                
                if(_field == null)
                    continue;
                    
                EnumTitleAttribute[] _attrs = _field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                
                if(_attrs != null && _attrs.Length > 0)
                {
                    _builder.AppendFormat("{0}{1}", _attrs[0].Title, ENUM_TITLE_SEPARATOR);
                }
            }
            
            return _builder.ToString().TrimEnd(ENUM_TITLE_SEPARATOR.ToArray());
        }
        
        /// <summary>
        /// 根据枚举获取包含所有所有值和描述的哈希表，其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <returns></returns>
        public static Dictionary<T, string> GetAllItemList<T>()
        where T : struct
        {
            return GetItemValueList<T, T>(true);
        }
        
        /// <summary>
        /// 获取枚举所有项的标题,其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <typeparam name="TKey">泛型</typeparam>
        /// <returns></returns>
        public static Dictionary<TKey, string> GetAllItemValueList<T, TKey>()
        where T : struct
        {
            return GetItemValueList<T, TKey>(true);
        }
        
        /// <summary>
        /// 根据枚举值，返回描述字符串
        /// 如果多选枚举，返回以","分割的字符串
        /// </summary>
        /// <param name="e">枚举</param>
        /// <returns>以","分割的字符串</returns>
        public static string GetEnumTitle(Enum e)
        {
            string[] _valueArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type _type = e.GetType();
            StringBuilder _builder = new StringBuilder();
            
            foreach(string enumValue in _valueArray)
            {
                FieldInfo _field = _type.GetField(enumValue.Trim());
                
                if(_field == null)
                    continue;
                    
                EnumTitleAttribute[] _attrs = _field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                
                if(_attrs != null && _attrs.Length > 0 && _attrs[0].IsDisplay)
                {
                    _builder.AppendFormat("{0}{1}", _attrs[0].Title, ENUM_TITLE_SEPARATOR);
                }
            }
            
            return _builder.ToString().TrimEnd(ENUM_TITLE_SEPARATOR.ToArray());
        }
        
        /// <summary>
        /// 根据枚举获取包含所有所有值和描述的哈希表，其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <returns></returns>
        public static Dictionary<T, string> GetItemList<T>()
        where T : struct
        {
            return GetItemValueList<T, T>(false);
        }
        
        /// <summary>
        /// 获取枚举所有项的标题,其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetItemValueList<T>()
        where T : struct
        {
            return GetItemValueList<T, int>(false);
        }
        /// <summary>
        /// 获取枚举所有项的标题,其文本是由应用在枚举值上的EnumTitleAttribute设定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <typeparam name="TKey">泛型</typeparam>
        /// <param name="isAll">是否生成“全部”项</param>
        /// <returns>字典</returns>
        public static Dictionary<TKey, string> GetItemValueList<T, TKey>(bool isAll)
        where T : struct
        {
            Dictionary<TKey, string> _dic = new Dictionary<TKey, string>();
            
            var _titles = GetItemAttributeList<T>().OrderBy(t => t.Value.Order);
            
            foreach(var item in _titles)
            {
                if(!isAll && (!item.Value.IsDisplay || string.Compare(item.Key.ToString(), "None", true) == 0))
                    continue;
                    
                if(item.Key.ToString() == "None" && isAll)
                {
                    _dic.Add((TKey)(object)item.Key, "全部");
                }
                else
                {
                    if(!string.IsNullOrEmpty(item.Value.Title))
                        _dic.Add((TKey)(object)item.Key, item.Value.Title);
                }
            }
            
            return _dic;
        }
        
        /// <summary>
        /// 返回键值对，建为枚举的EnumTitle中指定的名称和近义词名称，值为枚举项
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<string, T> GetTitleAndSynonyms<T>()
        where T : struct
        {
            Dictionary<string, T> _dic = new Dictionary<string, T>();
            //枚举值
            Array _arrEnumValue = typeof(T).GetEnumValues();
            
            foreach(object enumValue in _arrEnumValue)
            {
                FieldInfo _field = typeof(T).GetField(enumValue.ToString());
                
                if(_field == null)
                {
                    continue;
                }
                
                EnumTitleAttribute[] _arrEnumTitleAttr = _field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                
                if(_arrEnumTitleAttr == null || _arrEnumTitleAttr.Length < 1 || !_arrEnumTitleAttr[0].IsDisplay)
                {
                    continue;
                }
                
                if(!_dic.ContainsKey(_arrEnumTitleAttr[0].Title))
                {
                    _dic.Add(_arrEnumTitleAttr[0].Title, (T)enumValue);
                }
                
                if(_arrEnumTitleAttr[0].Synonyms == null || _arrEnumTitleAttr[0].Synonyms.Length < 1)
                {
                    continue;
                }
                
                foreach(string item in _arrEnumTitleAttr[0].Synonyms)
                {
                    if(!_dic.ContainsKey(item))
                    {
                        _dic.Add(item, (T)enumValue);
                    }
                }
            }
            
            return _dic;
        }
        
        private static EnumTitleAttribute GetEnumTitleAttribute(Enum e)
        {
            string[] valueArray = e.ToString().Split(ENUM_TITLE_SEPARATOR.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Type type = e.GetType();
            EnumTitleAttribute _ret = null;
            
            foreach(string enumValue in valueArray)
            {
                FieldInfo _field = type.GetField(enumValue.Trim());
                
                if(_field == null)
                    continue;
                    
                EnumTitleAttribute[] _attrs = _field.GetCustomAttributes(typeof(EnumTitleAttribute), false) as EnumTitleAttribute[];
                
                if(_attrs != null && _attrs.Length > 0)
                {
                    _ret = _attrs[0];
                    break;
                }
            }
            
            return _ret;
        }
        
        private static Dictionary<T, EnumTitleAttribute> GetItemAttributeList<T>()
        where T : struct
        {
            Dictionary<T, EnumTitleAttribute> _dic = new Dictionary<T, EnumTitleAttribute>();
            
            Array _array = typeof(T).GetEnumValues();
            
            foreach(object t in _array)
            {
                EnumTitleAttribute _att = GetEnumTitleAttribute(t as Enum);
                
                if(_att != null)
                    _dic.Add((T)t, _att);
            }
            
            return _dic;
        }
        
        #endregion Methods
    }
}