namespace YanZhiwei.DotNet.Core.RedisCache
{
    using ServiceStack.Redis;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using YanZhiwei.DotNet.Core.Cache;

    /// <summary>
    /// Redis缓存Provider
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Cache.ICacheProvider" />
    public class RedisCacheProvider : ICacheProvider
    {
        #region Fields

        private static readonly IRedisClient redisReadClient = RedisManager.GetReadOnlyClient();
        private static readonly IRedisClient redisWriteClient = RedisManager.GetClient();
        private static readonly object syncRoot = new object();

        #endregion Fields

        #region Methods

        /// <summary>
        /// 以键取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>
        /// 值
        /// </returns>
        public object Get(string key)
        {
            if (redisWriteClient.ContainsKey(key))
            {
                return redisWriteClient.Get<object>(key);
            }

            return null;
        }

        /// <summary>
        /// 从缓存中获取强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>
        /// 获取的强类型数据
        /// </returns>
        public T Get<T>(string key)
        {
            if (redisWriteClient.ContainsKey(key))
            {
                return redisWriteClient.Get<T>(key);
            }

            return default(T);
        }

        /// <summary>
        /// 该key是否设置过缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return redisWriteClient.ContainsKey(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            redisWriteClient.Remove(key);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        public void RemoveByPattern(string pattern)
        {
            List<string> _keys = new List<string>();
            List<string> _cacheKeys = redisReadClient.GetAllKeys();

            foreach (string key in _cacheKeys)
            {
                if (Regex.IsMatch(key, pattern, RegexOptions.IgnoreCase))
                    _keys.Add(key);
            }

            for (int i = 0; i < _keys.Count; i++)
            {
                redisWriteClient.Remove(_keys[i]);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="minutes">分钟</param>
        /// <param name="isAbsoluteExpiration">是否绝对时间</param>
        public void Set(string key, object value, int minutes, bool isAbsoluteExpiration)
        {
            lock (syncRoot)
            {
                if (!redisWriteClient.ContainsKey(key))
                {
                    if (value != null)
                    {
                        redisWriteClient.Set(key, value, TimeSpan.FromMinutes(minutes));
                    }
                }
            }
        }

        #endregion Methods
    }
}