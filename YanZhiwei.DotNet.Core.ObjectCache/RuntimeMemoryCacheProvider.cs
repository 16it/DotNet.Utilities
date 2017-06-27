namespace YanZhiwei.DotNet.Core.ObjectCache
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text.RegularExpressions;

    using DotNet2.Utilities.Operator;

    using YanZhiwei.DotNet.Core.Cache;

    /// <summary>
    /// RuntimeMemoryCache 辅助类
    /// </summary>
    public class RuntimeMemoryCacheProvider : ICacheProvider
    {
        #region Properties

        /// <summary>
        /// MemoryCache
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="keyRegex">正则表达式</param>
        public virtual void Clear(string keyRegex)
        {
            List<string> _keys = new List<string>();
            List<string> _cacheKeys = Cache.Select(m => m.Key).ToList();

            foreach(string key in _cacheKeys)
            {
                if(Regex.IsMatch(key, keyRegex, RegexOptions.IgnoreCase))
                    _keys.Add(key);
            }

            for(int i = 0; i < _keys.Count; i++)
            {
                Cache.Remove(_keys[i]);
            }
        }

        /// <summary>
        /// 以键取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>
        /// 值
        /// </returns>
        public virtual object Get(string key)
        {
            CheckedParamter(key);
            string _cacheKey = GetCacheKey(key);
            object _value = Cache.Get(_cacheKey);

            if(_value == null)
            {
                return null;
            }

            DictionaryEntry _entry = (DictionaryEntry)_value;

            if(!key.Equals(_entry.Key))
            {
                return null;
            }

            return _entry.Value;
        }

        /// <summary>
        /// 从缓存中获取强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>获取的强类型数据</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Get(key);
        }

        /// <summary>
        /// 该key是否设置过缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            string _cacheKey = GetCacheKey(key);
            return Cache.Contains(_cacheKey);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            CheckedParamter(key);
            string _cacheKey = GetCacheKey(key);
            Cache.Remove(_cacheKey);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="minutes">分钟</param>
        /// <param name="isAbsoluteExpiration">是否绝对时间</param>
        /// <param name="onRemoveFacotry">委托</param>
        public virtual void Set(string key, object value, int minutes, bool isAbsoluteExpiration, Action<string, object, string> onRemoveFacotry)
        {
            CheckedParamter(key, value);
            string _cacheKey = GetCacheKey(key);
            DictionaryEntry _entry = new DictionaryEntry(key, value);
            CacheItemPolicy _cacheItemPolicy = CreateCacheItemPolicy(isAbsoluteExpiration, minutes, onRemoveFacotry);

            if(Cache.Contains(_cacheKey))
                Cache.Set(_cacheKey, _entry, _cacheItemPolicy);

            else
                Cache.Add(_cacheKey, _entry, _cacheItemPolicy);
        }

        private void CheckedParamter(string key, object value)
        {
            ValidateOperator.Begin().NotNullOrEmpty(key, "缓存键").NotNull(value, "缓存数据");
        }

        private void CheckedParamter(string key)
        {
            ValidateOperator.Begin().NotNullOrEmpty(key, "缓存键");
        }

        private CacheItemPolicy CreateCacheItemPolicy(bool isAbsoluteExpiration, int minutes, Action<string, object, string> onRemoveFacotry)
        {
            CacheItemPolicy _cacheItemPolicy = null;

            if(isAbsoluteExpiration)
            {
                _cacheItemPolicy = new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(minutes),
                    RemovedCallback = arg => onRemoveFacotry(arg.CacheItem.Key, arg.CacheItem.Value, arg.RemovedReason.ToString())
                };
            }

            else
            {
                _cacheItemPolicy = new CacheItemPolicy()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(minutes),
                    RemovedCallback = arg => onRemoveFacotry(arg.CacheItem.Key, arg.CacheItem.Value, arg.RemovedReason.ToString())
                };
            }

            return _cacheItemPolicy;
        }

        private string GetCacheKey(string key)
        {
            return string.Concat(string.Empty, ":", key, "@", key.GetHashCode());
        }

        #endregion Methods
    }
}