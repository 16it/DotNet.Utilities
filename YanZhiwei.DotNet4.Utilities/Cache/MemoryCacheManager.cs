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
                long cacheSize = 0;

                foreach (var v in CacheStore)
                {
                    using (Stream s = new MemoryStream())
                    {
                        if (v.Value is IQueryable)
                        {
                            long _listSize = 0;

                            foreach (var q in (IQueryable)v.Value)
                            {
                                BinaryFormatter _binaryFormatter = new BinaryFormatter();
                                _binaryFormatter.Serialize(s, q);
                                _listSize += s.Length;
                            }

                            cacheSize += _listSize;
                            continue;
                        }

                        BinaryFormatter _formatter = new BinaryFormatter();
                        _formatter.Serialize(s, v.Value);
                        cacheSize += s.Length;
                    }
                }

                return cacheSize;
            }
        }

        /// <summary>
        /// Returns the default expiration time for a memory
        /// </summary>
        public static DateTimeOffset DefaultExpirationTime
        {
            get
            {
                return DateTimeOffset.Now.AddMinutes(5);
            }
        }

        /// <summary>
        /// Gets the maximum amount cache available to the server.
        /// </summary>
        /// <returns>The amount of memory that can be used for cache.</returns>
        public static long MaximumCacheSize
        {
            get
            {
                return ((MemoryCache)(CacheStore)).CacheMemoryLimit;
            }
        }

        /// <summary>
        /// Gets the amount of memory the cache can use.
        /// </summary>
        /// <returns>The amount of memory that can be used for cache.</returns>
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
        /// Add an object to the cache.
        /// </summary>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="objectToCache">The object to add to the cache.</param>
        public static void Add(string key, object objectToCache)
        {
            //default expiration time is 30 minutes
            Add(key, objectToCache, new CacheItemPolicy()
            {
                AbsoluteExpiration = DefaultExpirationTime
            });
        }

        /// <summary>
        /// Add an object to the cache.
        /// </summary>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="objectToCache">The object to add to the cache.</param>
        /// <param name="policy">The policies for the cached object.</param>
        public static void Add(string key, object objectToCache, CacheItemPolicy policy)
        {
            CacheStore.Add(key, objectToCache, policy);
        }

        /// <summmary>
        /// Retrieves an object from cache or adds it if it does not exist.
        /// </summmary>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="objectToCache">The object to add to the cache.</param>
        /// <typeparam name="T">The type of object that is expected to retrieve from the function.</typeparam>
        /// <returns>A type of object T.</returns>
        public static T AddOrGetFromCache<T>(string key, object objectToCache)
            where T : class
        {
            return AddOrGetFromCache<T>(key, objectToCache, new CacheItemPolicy()

            {
                AbsoluteExpiration = DefaultExpirationTime
            });
        }

        /// <summmary>
        /// Retrieves an object from cache or adds it if it does not exist.
        /// </summmary>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="objectToCache">The object to add to the cache.</param>
        /// <param name="policy">A set of eviction and expiration details for a specific cache entry. </param>
        /// <typeparam name="T">The type of object that is expected to retrieve from the function.</typeparam>
        /// <returns>A type of object T.</returns>
        public static T AddOrGetFromCache<T>(string key, object objectToCache, CacheItemPolicy policy)
            where T : class
        {
            object cachedObject = GetFromCache<T>(key);

            if (cachedObject == null)

            {
                Add(key, objectToCache, policy);
                cachedObject = objectToCache;
            }

            return (T)cachedObject;
        }

        /// <summmary>
        /// Retrieves an object from cache or adds it if it does not exist.
        /// </summmary>
        /// <typeparam name="T">The type of object that is expected to retrieve from the function.</typeparam>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="func">A lambda expression that will be used to retrieve the object if not cached.</param>
        /// <returns>A type of object T.</returns>
        public static T AddOrGetFromCache<T>(string key, Func<T> func)
            where T : class
        {
            //default expiration time is 30 minutes

            return AddOrGetFromCache<T>(key, func, new CacheItemPolicy()

            {
                AbsoluteExpiration = DefaultExpirationTime
            });
        }

        /// <summmary>
        /// Retrieves an object from cache or adds it if it does not exist.
        /// </summmary>
        /// <typeparam name="T">The type of object that is expected to retrieve from the function.</typeparam>
        /// <param name="key">The key of the object to add.</param>
        /// <param name="func">A lambda expression that will be used to retrieve the object if not cached.</param>
        /// <param name="policy">The policies for the cached object.</param>
        /// <returns>A type of object T.</returns>
        public static T AddOrGetFromCache<T>(string key, Func<T> func, CacheItemPolicy policy)
            where T : class
        {
            object cachedObject = GetFromCache<T>(key);

            if (cachedObject == null)

            {
                var objectToCache = func();
                Add(key, objectToCache, policy);
                cachedObject = objectToCache;
            }

            return (T)cachedObject;
        }

        /// <summary>
        /// Clears the cache of all objects.
        /// </summary>
        public static void ClearCache()
        {
            foreach (var v in CacheStore)
            {
                CacheStore.Remove(v.Key);
            }
        }

        /// <summary>
        /// Deletes an object from the cache.
        /// </summary>
        /// <param name="key">The key for the object to remove.</param>
        /// <returns>The object that was removed, or null if not removed.</returns>
        public static object DeleteCacheEntry(string key)
        {
            return CacheStore.Remove(key);
        }

        /// <summary>
        /// Deletes an object from the cache that starts with the specified key.
        /// </summary>
        /// <param name="key">The key for the object to remove.</param>
        /// <returns>The object that was removed, or null if not removed.</returns>
        public static IEnumerable<object> DeleteCacheEntryStartsWith(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;

            var objects = new List<object>();

            foreach (var entry in CacheStore)
            {
                if (entry.Key.StartsWith(key))
                {
                    objects.Add(CacheStore.Remove(entry.Key));
                }
            }

            return objects;
        }

        /// <summary>
        /// Gets a list of key value pairs containing the key and value for the cache.
        /// </summary>
        /// <returns>A dictionary of strins containing key and value pairs for the cache.</returns>
        public static Dictionary<string, string> GetAllCacheInfo()
        {
            var list = new Dictionary<string, string>();

            foreach (var v in CacheStore)
            {
                list.Add(v.Key, GetEntryInfo(v.Key));
            }

            return list;
        }

        /// <summary>
        /// Gets all keys in the Cache.
        /// </summary>
        /// <returns>A list containing all keys found in the cache.</returns>
        public static List<string> GetAllCacheKeys()
        {
            var list = new List<string>();

            foreach (var v in CacheStore)
            {
                list.Add(v.Key);
            }

            return list;
        }

        /// <summary>
        /// Gets a serialized form of the object in cache.
        /// </summary>
        /// <param name="key">The key of the cached object.</param>
        /// <returns>A serialized string of the cached object.</returns>
        public static string GetEntryInfo(string key)
        {
            var value = CacheStore.Get(key);
            var infoString = new StringBuilder("");

            if (value != null)
            {
                if (value is IQueryable)
                {
                    foreach (var q in (IQueryable)value)
                    {
                        XmlSerializer srl = new XmlSerializer(q.GetType());

                        using (StringWriter writer = new StringWriter())
                        {
                            using (XmlTextWriter tw = new XmlTextWriter(writer))
                            {
                                tw.Formatting = Formatting.Indented;
                                tw.Indentation = 4;
                                srl.Serialize(tw, q);
                                infoString.AppendLine(writer.ToString());
                                infoString.AppendLine();
                            }
                        }
                    }
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(value.GetType());

                    using (StringWriter writer = new StringWriter())
                    {
                        using (XmlTextWriter tw = new XmlTextWriter(writer))
                        {
                            tw.Formatting = Formatting.Indented;
                            tw.Indentation = 4;
                            serializer.Serialize(tw, value);
                            infoString.AppendLine(writer.ToString());
                        }
                    }
                }
            }

            return infoString.ToString();
        }

        /// <summary>
        /// Get an object from cache.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The key of the object to retrieve.</param>
        /// <returns>A type of </returns>
        public static T GetFromCache<T>(string key)
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

        #endregion Methods
    }
}