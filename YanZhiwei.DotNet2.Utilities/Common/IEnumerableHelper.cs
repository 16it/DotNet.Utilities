namespace YanZhiwei.DotNet2.Utilities.Common
{
    using Collection;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    
    /// <summary>
    /// Enumerable 帮助类
    /// </summary>
    public static class IEnumerableHelper
    {
        #region Methods
        
        /// <summary>
        /// 线程安全【上锁】
        ///<para> eg: foreach(var item in someList.AsLocked(someLock))</para>
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">IEnumerable</param>
        /// <param name="syncObject">lock对象</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> AsLocked<T>(this IEnumerable<T> source, object syncObject)
        {
            /*
            * 参考：
            * 1. http://www.codeproject.com/Articles/56575/Thread-safe-enumeration-in-C
            */
            return new ThreadSafeEnumerableHelper<T>(source, syncObject);
        }
        
        /// <summary>
        /// 集合添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="self">本身集合</param>
        /// <param name="list">需要添加集合</param>
        public static void AddRange<T>(this IEnumerable<T> self, IEnumerable<T> list)
        where T : class
        {
            ((List<T>)self).AddRange(list);
        }
        
        /// <summary>
        /// 去重复集合添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="self">本身集合</param>
        /// <param name="items">需要集合</param>
        public static void AddUnique<T>(this List<T> self, IEnumerable<T> items)
        where T : class
        {
            foreach(T item in items)
            {
                if(!self.Contains(item))
                {
                    self.Add(item);
                }
            }
        }
        
        /// <summary>
        /// 去重复集合添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="self">本身集合</param>
        /// <param name="items">需要添加集合</param>
        /// <param name="comparaer">IComparer</param>
        public static void AddUnique<T>(this List<T> self, IEnumerable<T> items, IComparer<T> comparaer)
        where T : class
        {
            self.Sort(comparaer);
            
            foreach(T item in items)
            {
                int _result = self.BinarySearch(item, comparaer);//搜索前需要排序
                
                if(_result < 0)
                    self.Add(item);
            }
        }
        
        public static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            DataTable _table = new DataTable();
            
            foreach(var item in typeof(T).GetProperties())
            {
                object[] _attribute = item.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                string _displayName = _attribute.Length > 0 ? (_attribute[0] as DisplayNameAttribute).DisplayName : item.Name;
                _table.Columns.Add(new DataColumn(_displayName, item.PropertyType));
            }
            
            foreach(var report in data)
            {
                DataRow dr = _table.NewRow();
                
                foreach(var item in report.GetType().GetProperties())
                {
                    string displayName = string.Empty;
                    object[] objs = item.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    
                    if(objs.Length > 0)
                        displayName = (objs[0] as DisplayNameAttribute).DisplayName;
                        
                    dr[displayName] = item.GetValue(report, null);
                }
                
                _table.Rows.Add(dr);
            }
            
            return _table;
        }
        #endregion Methods
    }
}