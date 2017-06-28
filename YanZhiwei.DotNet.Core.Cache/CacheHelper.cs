namespace YanZhiwei.DotNet.Core.Cache
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

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
            if (!Regex.IsMatch(CacheConfigContext.ModuleName, moduleRegex, RegexOptions.IgnoreCase))
                return;

            foreach (var cacheProviders in CacheConfigContext.CacheProviders.Values)
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
            if (_cacheProvider.IsSet(key))
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

            foreach (var key in keys.Where(p => _regex.IsMatch(p.ToString())).ToList())
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
        #endregion Methods
    }
}