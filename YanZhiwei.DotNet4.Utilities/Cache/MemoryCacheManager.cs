namespace YanZhiwei.DotNet4.Utilities.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// MemoryCache 辅助类
    /// </summary>
    public class MemoryCacheManager
    {
        #region Fields

        private static ObjectCache cache = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// ObjectCache
        /// </summary>
        public static ObjectCache CacheStore
        {
            get
            {
                if (cache == null)
                {
                    NameValueCollection _config = new NameValueCollection();
                    _config.Add("PhysicalMemoryLimitPercentage", MemoryCache.Default.PhysicalMemoryLimit.ToString());
                    cache = new MemoryCache("defaultCache", _config);
                }

                return cache;
            }

            set
            {
                cache = value;
            }
        }

        /// <summary>
        /// 当前缓存大小
        /// </summary>
        public static long CurrentCacheSize
        {
            get
            {
                long _cacheSize = 0;

                foreach (var item in CacheStore)
                {
                    using (Stream s = new MemoryStream())
                    {
                        if (item.Value is IQueryable)
                        {
                            long _listSize = 0;

                            foreach (var q in (IQueryable)item.Value)
                            {
                                BinaryFormatter _binaryFormatter = new BinaryFormatter();
                                _binaryFormatter.Serialize(s, q);
                                _listSize += s.Length;
                            }

                            _cacheSize += _listSize;
                            continue;
                        }

                        BinaryFormatter _formatter = new BinaryFormatter();
                        _formatter.Serialize(s, item.Value);
                        _cacheSize += s.Length;
                    }
                }

                return _cacheSize;
            }
        }

        /// <summary>
        /// 默认过期时间
        /// </summary>
        public static DateTimeOffset DefaultExpirationTime
        {
            get
            {
                return DateTimeOffset.Now.AddMinutes(5);
            }
        }

        /// <summary>
        /// 获取计算机缓存可使用的内存量
        /// </summary>
        /// <value>
        /// 可使用的内存量
        /// </value>
        public static long MaximumCacheSize
        {
            get
            {
                return ((MemoryCache)(CacheStore)).CacheMemoryLimit;
            }
        }

        /// <summary>
        /// 可以使用的总的物理计算机内存的百分比
        /// </summary>
        /// <value>
        /// 物理计算机内存的百分比
        /// </value>
        public static long PhysicalMemoryLimit
        {
            get
            {
                return ((MemoryCache)(CacheStore)).PhysicalMemoryLimit;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="objectToCache">缓存对象</param>
        public static void Add(string key, object objectToCache)
        {
            Add(key, objectToCache, new CacheItemPolicy()
            {
                AbsoluteExpiration = DefaultExpirationTime
            });
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="objectToCache">缓存对象</param>
        /// <param name="policy">缓存策略</param>
        public static void Add(string key, object objectToCache, CacheItemPolicy policy)
        {
            CacheStore.Add(key, objectToCache, policy);
        }

        /// <summary>
        /// 若缓存存在则获取，若缓存不存在则增加
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="objectToCache">缓存对象</param>
        /// <returns>缓存对象</returns>
        public static T AddOrGet<T>(string key, object objectToCache)
            where T : class
        {
            return AddOrGet<T>(key, objectToCache, new CacheItemPolicy()

            {
                AbsoluteExpiration = DefaultExpirationTime
            });
        }

        /// <summary>
        /// 若缓存存在则获取，若缓存不存在则增加
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="objectToCache">缓存对象</param>
        /// <param name="policy">缓存策略</param>
        /// <returns>缓存对象</returns>
        public static T AddOrGet<T>(string key, object objectToCache, CacheItemPolicy policy)
            where T : class
        {
            object _cachedObject = Get<T>(key);

            if (_cachedObject == null)

            {
                Add(key, objectToCache, policy);
                _cachedObject = objectToCache;
            }

            return (T)_cachedObject;
        }

        /// <summary>
        ///  若缓存存在则获取，若缓存不存在则增加
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="cacheFactory">缓存委托</param>
        /// <returns>缓存对象</returns>
        public static T AddOrGet<T>(string key, Func<T> cacheFactory)
            where T : class
        {
            return AddOrGet<T>(key, cacheFactory, new CacheItemPolicy()

            {
                AbsoluteExpiration = DefaultExpirationTime
            });
        }

        /// <summary>
        ///  若缓存存在则获取，若缓存不存在则增加
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="cacheFactory">缓存委托</param>
        /// <param name="policy">缓存策略</param>
        /// <returns>缓存对象</returns>
        public static T AddOrGet<T>(string key, Func<T> cacheFactory, CacheItemPolicy policy)
            where T : class
        {
            object _cachedObject = Get<T>(key);

            if (_cachedObject == null)

            {
                var _objectToCache = cacheFactory();
                Add(key, _objectToCache, policy);
                _cachedObject = _objectToCache;
            }

            return (T)_cachedObject;
        }

        /// <summary>
        /// 清楚所有缓存
        /// </summary>
        public static void ClearCache()
        {
            foreach (var item in CacheStore)
            {
                CacheStore.Remove(item.Key);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存对象</returns>
        public static object Delete(string key)
        {
            return CacheStore.Remove(key);
        }

        /// <summary>
        /// 删除key开头匹配缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存对象</returns>
        public static IEnumerable<object> DeleteStartsWith(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            var _cacheList = new List<object>();

            foreach (var item in CacheStore)
            {
                if (item.Key.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    _cacheList.Add(CacheStore.Remove(item.Key));
                }
            }

            return _cacheList;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存对象</returns>
        public static T Get<T>(string key)
            where T : class
        {
            try

            {
                return (T)CacheStore.Get(key);
            }
            catch

            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有缓存集合信息
        /// </summary>
        /// <returns>缓存集合信息</returns>
        public static Dictionary<string, string> GetAllCacheInfo()
        {
            var _allCaches = new Dictionary<string, string>();

            foreach (var item in CacheStore)
            {
                _allCaches.Add(item.Key, GetInfo(item.Key));
            }

            return _allCaches;
        }

        /// <summary>
        /// 获取所有缓存key
        /// </summary>
        /// <returns>缓存key集合</returns>
        public static List<string> GetAllCacheKeys()
        {
            var _allCacheKeys = new List<string>();

            foreach (var item in CacheStore)
            {
                _allCacheKeys.Add(item.Key);
            }

            return _allCacheKeys;
        }

        /// <summary>
        /// 获取缓存对象信息
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>缓存对象信息</returns>
        public static string GetInfo(string key)
        {
            var _cacheObject = CacheStore.Get(key);
            StringBuilder _builder = new StringBuilder();

            if (_cacheObject != null)
            {
                HanlderXmlCacheObject(_cacheObject, _builder);
            }

            return _builder.ToString();
        }

        private static void HanlderXmlCacheObject(object cacheObject, StringBuilder builder)
        {
            if (cacheObject is IQueryable)
            {
                foreach (var item in (IQueryable)cacheObject)
                {
                    XmlSerializer _xmlSerializer = new XmlSerializer(item.GetType());

                    using (StringWriter writer = new StringWriter())
                    {
                        using (XmlTextWriter tw = new XmlTextWriter(writer))
                        {
                            tw.Formatting = Formatting.Indented;
                            tw.Indentation = 4;
                            _xmlSerializer.Serialize(tw, item);
                            builder.AppendLine(writer.ToString());
                            builder.AppendLine();
                        }
                    }
                }
            }
            else
            {
                XmlSerializer _xmlSerializer = new XmlSerializer(cacheObject.GetType());

                using (StringWriter writer = new StringWriter())
                {
                    using (XmlTextWriter tw = new XmlTextWriter(writer))
                    {
                        tw.Formatting = Formatting.Indented;
                        tw.Indentation = 4;
                        _xmlSerializer.Serialize(tw, cacheObject);
                        builder.AppendLine(writer.ToString());
                    }
                }
            }
        }

        #endregion Methods
    }
}