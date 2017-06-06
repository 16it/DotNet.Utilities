namespace YanZhiwei.DotNet.Core.Cache
{
    using Config;
    using DotNet2.Utilities.Operator;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using YanZhiwei.DotNet.Core.Config.Model;

    /// <summary>
    /// 缓存配置上下文
    /// </summary>
    public class CacheConfigContext
    {
        #region Fields

        /// <summary>
        /// 读写锁对象
        /// </summary>
        private static readonly object syncObject = new object();

        /// <summary>
        /// 首次加载所有的CacheProviders
        /// </summary>
        private static Dictionary<string, ICacheProvider> cacheProviders;

        /// <summary>
        /// 得到网站项目的入口程序模块名名字，用于CacheConfigItem.ModuleRegex
        /// </summary>
        /// <returns></returns>
        private static string moduleName;

        /// <summary>
        /// 根据Key，通过正则匹配从WrapCacheConfigItems里帅选出符合的缓存项目，然后通过字典缓存起来
        /// </summary>
        private static Dictionary<string, WrapCacheConfigItem> wrapCacheConfigItemDic;

        /// <summary>
        /// 首次加载所有的CacheConfig, wrapCacheConfigItem相对于cacheConfigItem把providername通过反射还原成了具体provider类
        /// </summary>
        private static List<WrapCacheConfigItem> wrapCacheConfigItems;

        #endregion Fields

        #region Properties

        public static string ModuleName
        {
            get
            {
                if (moduleName == null)
                {
                    lock (syncObject)
                    {
                        if (moduleName == null)
                        {
                            Assembly _entryAssembly = Assembly.GetEntryAssembly();

                            if (_entryAssembly != null)
                            {
                                moduleName = _entryAssembly.FullName;
                            }
                            else
                            {
                                moduleName = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Name;
                            }
                        }
                    }
                }

                return moduleName;
            }
        }

        /// <summary>
        /// CacheConfig
        /// </summary>
        internal static CacheConfig CacheConfig
        {
            get
            {
                return CachedConfigContext.Instance.CacheConfig;
            }
        }

        /// <summary>
        /// 首次加载所有的CacheProviders
        /// </summary>
        internal static Dictionary<string, ICacheProvider> CacheProviders
        {
            get
            {
                if (cacheProviders == null)
                {
                    lock (syncObject)
                    {
                        if (cacheProviders == null)
                        {
                            cacheProviders = new Dictionary<string, ICacheProvider>();

                            foreach (var i in CacheConfig.CacheProviderItems)
                            {
                                cacheProviders.Add(i.Name, (ICacheProvider)Activator.CreateInstance(Type.GetType(i.Type)));
                            }
                        }
                    }
                }

                return cacheProviders;
            }
        }

        /// <summary>
        /// 首次加载所有的CacheConfig, wrapCacheConfigItem相对于cacheConfigItem把providername通过反射还原成了具体provider类
        /// </summary>
        internal static List<WrapCacheConfigItem> WrapCacheConfigItems
        {
            get
            {
                if (wrapCacheConfigItems == null)
                {
                    lock (syncObject)
                    {
                        if (wrapCacheConfigItems == null)
                        {
                            wrapCacheConfigItems = new List<WrapCacheConfigItem>();

                            foreach (var i in CacheConfig.CacheConfigItems)
                            {
                                WrapCacheConfigItem _cacheWrapConfigItem = new WrapCacheConfigItem();
                                _cacheWrapConfigItem.CacheConfigItem = i;
                                _cacheWrapConfigItem.CacheProviderItem = CacheConfig.CacheProviderItems.SingleOrDefault(c => c.Name == i.ProviderName);
                                _cacheWrapConfigItem.CacheProvider = CacheProviders[i.ProviderName];
                                wrapCacheConfigItems.Add(_cacheWrapConfigItem);
                            }
                        }
                    }
                }

                return wrapCacheConfigItems;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 根据Key，通过正则匹配从WrapCacheConfigItems里帅选出符合的缓存项目，然后通过字典缓存起来
        /// </summary>
        /// <param name="key">根据Key获取缓存配置项</param>
        /// <returns>缓存配置项</returns>
        /// <exception cref="System.Exception"></exception>
        public static WrapCacheConfigItem GetCurrentWrapCacheConfigItem(string key)
        {
            if (wrapCacheConfigItemDic == null)
                wrapCacheConfigItemDic = new Dictionary<string, WrapCacheConfigItem>();

            if (wrapCacheConfigItemDic.ContainsKey(key))
                return wrapCacheConfigItemDic[key];

            WrapCacheConfigItem _currentWrapCacheConfigItem = WrapCacheConfigItems.Where(i =>
                    Regex.IsMatch(ModuleName, i.CacheConfigItem.ModuleRegex, RegexOptions.IgnoreCase) &&
                    Regex.IsMatch(key, i.CacheConfigItem.KeyRegex, RegexOptions.IgnoreCase))
                    .OrderByDescending(i => i.CacheConfigItem.Priority).FirstOrDefault();
            ValidateOperator.Begin().NotNull(_currentWrapCacheConfigItem, string.Format("依据'{0}'获取缓存配置项异常！", key));

            lock (syncObject)
            {
                if (!wrapCacheConfigItemDic.ContainsKey(key))
                    wrapCacheConfigItemDic.Add(key, _currentWrapCacheConfigItem);
            }

            return _currentWrapCacheConfigItem;
        }

        #endregion Methods
    }
}