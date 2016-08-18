namespace YanZhiwei.DotNet.Core.Cache.Model
{
    using Core.Model;

    /// <summary>
    /// 缓存项
    /// </summary>
    public class WrapCacheConfigItem
    {
        #region Properties

        /// <summary>
        /// 缓存配置项
        /// </summary>
        public CacheConfigItem CacheConfigItem
        {
            get;
            set;
        }

        /// <summary>
        /// 缓存接口
        /// </summary>
        public ICacheProvider CacheProvider
        {
            get;
            set;
        }

        /// <summary>
        /// 缓存提供者配置项
        /// </summary>
        public CacheProviderItem CacheProviderItem
        {
            get;
            set;
        }

        #endregion Properties
    }
}