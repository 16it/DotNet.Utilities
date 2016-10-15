namespace YanZhiwei.DotNet3._5.Utilities.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    
    /// <summary>
    /// 通用类型映射转换
    /// </summary>
    /// 时间:2016/10/15 22:34
    /// 备注:
    public class GeneralMapper
    {
        #region Fields
        
        private static readonly IDictionary<Type, ICollection<PropertyInfo>> PropertiesCache =
            new Dictionary<Type, ICollection<PropertyInfo>>();
            
        #endregion Fields
        
        #region Methods
        
        /// <summary>
        /// 将DataTable导出成集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="table">需要导出的DataTable</param>
        /// <returns>集合</returns>
        /// 时间:2016/10/15 22:41
        /// 备注:
        public static IEnumerable<T> ToList<T>(DataTable table)
        where T : class, new()
        {
            try
            {
                Type _objType = typeof(T);
                ICollection<PropertyInfo> _properties;
                
                lock(PropertiesCache)
                {
                    if(!PropertiesCache.TryGetValue(_objType, out _properties))
                    {
                        _properties = _objType.GetProperties().Where(property => property.CanWrite).ToList();
                        PropertiesCache.Add(_objType, _properties);
                    }
                }
                
                List<T> _list = new List<T>(table.Rows.Count);
                IEnumerable<DataRow> _dataRowList = table.AsEnumerable();
                
                foreach(DataRow row in _dataRowList)
                {
                    T _objItem = new T();
                    
                    foreach(PropertyInfo prop in _properties)
                    {
                        try
                        {
                            Type _propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            object _safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], _propType);
                            prop.SetValue(_objItem, _safeValue, null);
                        }
                        catch
                        {
                        }
                    }
                    
                    _list.Add(_objItem);
                }
                
                return _list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }
        
        #endregion Methods
    }
}