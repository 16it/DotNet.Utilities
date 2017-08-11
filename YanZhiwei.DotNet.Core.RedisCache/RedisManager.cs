namespace YanZhiwei.DotNet.Core.RedisCache
{
    using ServiceStack.CacheAccess;
    using ServiceStack.Redis;

    using YanZhiwei.DotNet.Core.Config;
    using YanZhiwei.DotNet.Core.Config.Model;

    /// <summary>
    /// RedisManager
    /// </summary>
    public class RedisManager
    {
        #region Fields

        private static PooledRedisClientManager prcm;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Redis配置文件信息
        /// </summary>
        private static RedisConfig redisConfigInfo
        {
            get
            {
                return CachedConfigContext.Instance.RedisConfig;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 获取缓存Client
        /// </summary>
        public static ICacheClient GetCacheClient()
        {
            if(prcm == null)
            {
                CreateManager();
            }

            return prcm.GetCacheClient();
        }

        /// <summary>
        /// 获取可写入的Client
        /// </summary>
        public static IRedisClient GetClient()
        {
            if(prcm == null)
            {
                CreateManager();
            }

            return prcm.GetClient();
        }

        /// <summary>
        /// 获取只读的Client
        /// </summary>
        public static IRedisClient GetReadOnlyClient()
        {
            if(prcm == null)
            {
                CreateManager();
            }

            return prcm.GetReadOnlyClient();
        }

        /// <summary>
        /// 创建PooledRedisClientManager
        /// </summary>
        private static void CreateManager()
        {
            string[] _writeServerList = redisConfigInfo.WriteServerList.Split(',');
            string[] _readServerList = redisConfigInfo.ReadServerList.Split(',');
            prcm = new PooledRedisClientManager(_readServerList, _writeServerList, new RedisClientManagerConfig
            {
                MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                AutoStart = redisConfigInfo.AutoStart,
            });
        }

        #endregion Methods
    }
}