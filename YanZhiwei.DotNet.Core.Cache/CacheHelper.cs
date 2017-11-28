namespace YanZhiwei.DotNet.Core.Cache
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using YanZhiwei.DotNet2.Utilities.Encryptor;
    using YanZhiwei.DotNet2.Utilities.Model;
    using YanZhiwei.DotNet4.Utilities.Common;
    using YanZhiwei.DotNet4.Utilities.Core;
    
    /// <summary>
    /// 缓存帮助
    /// </summary>
    public static class CacheHelper
    {
        #region Methods
        
        /// <summary>
        /// 根据正则表达式移除缓存
        /// </summary>
        /// <param name="keyRegex">缓存键的正则表达式</param>
        /// <param name="moduleRegex">模块正则表达式</param>
        public static void Clear(string keyRegex, string moduleRegex)
        {
            if(!Regex.IsMatch(CacheConfigContext.ModuleName, moduleRegex, RegexOptions.IgnoreCase))
                return;
                
            foreach(var cacheProviders in CacheConfigContext.CacheProviders.Values)
                cacheProviders.RemoveByPattern(keyRegex);
        }
        
        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void Clear()
        {
            Clear(".*", ".*");
        }
        
        /// <summary>
        /// 以键取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object Get(string key)
        {
            WrapCacheConfigItem _cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            return _cacheConfig.CacheProvider.Get(key);
        }
        
        /// <summary>
        /// 以键取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Get<T>(string key)
        {
            WrapCacheConfigItem _cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            return _cacheConfig.CacheProvider.Get<T>(key);
        }
        
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="acquireFactory">若缓存不存在则获取后在存储到缓存</param>
        /// <returns>值</returns>
        public static T Get<T>(string key, Func<T> acquireFactory)
        {
            WrapCacheConfigItem _cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            ICacheProvider _cacheProvider = _cacheConfig.CacheProvider;
            
            if(_cacheProvider.IsSet(key))
            {
                return _cacheProvider.Get<T>(key);
            }
            
            var _result = acquireFactory();
            _cacheProvider.Set(key, _result, _cacheConfig.CacheConfigItem.Minitus, _cacheConfig.CacheConfigItem.IsAbsoluteExpiration);
            return _result;
        }
        
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            WrapCacheConfigItem _cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            _cacheConfig.CacheProvider.Remove(key);
        }
        
        /// <summary>
        /// 根据正则表达式移除缓存
        /// </summary>
        /// <param name="cacheProvider">ICacheProvider</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="keys">Key</param>
        public static void RemoveByPattern(this ICacheProvider cacheProvider, string pattern, IEnumerable<string> keys)
        {
            var _regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            foreach(var key in keys.Where(p => _regex.IsMatch(p.ToString())).ToList())
                cacheProvider.Remove(key);
        }
        
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, object value)
        {
            WrapCacheConfigItem _cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            _cacheConfig.CacheProvider.Set(key, value, _cacheConfig.CacheConfigItem.Minitus, _cacheConfig.CacheConfigItem.IsAbsoluteExpiration);
        }
        
        /// <summary>
        /// 将结果转换为缓存的数组，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">键</param>
        /// <returns>查询结果</returns>
        public static TSource[] ToCacheArray<TSource>(this IQueryable<TSource> source, string key)
        {
            string _key = string.Format("{0}{1}", key, GetKey(source.Expression));
            TSource[] _result = (TSource[])Get(_key);
            
            if(_result != null)
            {
                return _result;
            }
            
            _result = source.ToArray();
            Set(_key, _result);
            return _result;
        }
        
        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="key">键</param>
        /// <returns>查询结果</returns>
        public static List<TSource> ToCacheList<TSource>(this IQueryable<TSource> source, string key)
        {
            string _key = string.Format("{0}{1}", key, GetKey(source.Expression));
            List<TSource> _result = (List<TSource>)Get(_key);
            
            if(_result != null)
            {
                return _result;
            }
            
            _result = source.ToList();
            Set(_key, _result);
            return _result;
        }
        
        /// <summary>
        /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TResult">分页数据类型</typeparam>
        /// <param name="source">要查询的数据集</param>
        /// <param name="predicate">查询条件谓语表达式</param>
        /// <param name="pageCondition">分页查询条件</param>
        /// <param name="selector">数据筛选表达式</param>
        /// <param name="key">键</param>
        /// <returns>查询的分页结果</returns>
        public static PageList<TResult> ToPageCache<TEntity, TResult>(this IQueryable<TEntity> source,
                Expression<Func<TEntity, bool>> predicate,
                PageCondition pageCondition,
                Expression<Func<TEntity, TResult>> selector, string key)
        {
            string _key = string.Format("{0}{1}", key, GetKey(source, predicate, pageCondition, selector));
            PageList<TResult> _result = (PageList<TResult>)Get(_key);
            
            if(_result != null)
            {
                return _result;
            }
            
            _result = source.ToPage(predicate, pageCondition, selector);
            Set(_key, _result);
            return _result;
        }
        
        private static string GetKey<TEntity, TResult>(IQueryable<TEntity> source,
                Expression<Func<TEntity, bool>> predicate,
                PageCondition pageCondition,
                Expression<Func<TEntity, TResult>> selector)
        {
            if((pageCondition.SortConditions == null || pageCondition.SortConditions.Length == 0) && string.IsNullOrWhiteSpace(pageCondition.PrimaryKeyField))
            {
                throw new ArgumentException("排序条件集合为空的话，请设置主键字段数值！");
            }
            
            source = source.Where(predicate);
            SortCondition[] _sortConditions = pageCondition.SortConditions;
            
            if(_sortConditions == null || _sortConditions.Length == 0)
            {
                source = source.OrderBy(pageCondition.PrimaryKeyField);
            }
            
            else
            {
                int _count = 0;
                IOrderedQueryable<TEntity> orderSource = null;
                
                foreach(SortCondition sortCondition in _sortConditions)
                {
                    orderSource = _count == 0
                                  ? CollectionPropertySorter<TEntity>.OrderBy(source, sortCondition.SortField, sortCondition.ListSortDirection)
                                  : CollectionPropertySorter<TEntity>.ThenBy(orderSource, sortCondition.SortField, sortCondition.ListSortDirection);
                    _count++;
                }
                
                source = orderSource;
            }
            
            int _pageIndex = pageCondition.PageIndex, pageSize = pageCondition.PageSize;
            source = source != null
                     ? source.Skip((_pageIndex - 1) * pageSize).Take(pageSize)
                     : Enumerable.Empty<TEntity>().AsQueryable();
            IQueryable<TResult> _query = source.Select(selector);
            return GetKey(_query.Expression);
        }
        
        private static string GetKey(Expression expression)
        {
            return MD5Encryptor.Encrypt(expression.ToString());
        }
        
        #endregion Methods
    }
}