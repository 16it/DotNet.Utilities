namespace YanZhiwei.DotNet.ServiceStackRedis.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    
    using ServiceStack.DesignPatterns.Model;
    using ServiceStack.Redis;
    using ServiceStack.Redis.Generic;
    
    /// <summary>
    /// 基于ServiceStack Redis辅助类
    /// </summary>
    /// 时间：2016/11/10 15:13
    /// 备注：
    public class RedisCacheManger : IDisposable
    {
        #region Fields
        
        /// <summary>
        /// PooledRedisClientManager
        /// </summary>
        public readonly PooledRedisClientManager PRM;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="readWriteHosts">读写Redis主机</param>
        /// <param name="readOnlyHosts">只读Redis主机</param>
        /// <param name="defaultDb">Reids DataBase索引</param>
        /// 时间：2016/11/10 15:14
        /// 备注：
        public RedisCacheManger(string[] readWriteHosts, string[] readOnlyHosts, long defaultDb)
        {
            PRM = CreateManager(readWriteHosts, readOnlyHosts, defaultDb);
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">Reids 主机Ip</param>
        /// <param name="port">Redis 端口</param>
        /// <param name="defaultDb">Reids DataBase索引</param>
        /// 时间：2016/11/10 15:14
        /// 备注：
        public RedisCacheManger(string ip, int port, long defaultDb)
        {
            string[] _hosts = new string[] { string.Format("{0}:{1}", ip, port) };
            PRM = CreateManager(_hosts, _hosts, defaultDb);
        }
        
        #endregion Constructors
        
        #region Methods
        
        /// <summary>
        /// 检查Redis连接实例
        /// </summary>
        /// <returns>是否连接畅通</returns>
        /// 时间:2016/11/9 23:15
        /// 备注:
        public bool CheckedConnectInstance()
        {
            bool _result = true;
            
            try
            {
                using(IRedisClient redisClient = PRM.GetClient())
                {
                    redisClient.GetAllKeys();
                }
            }
            catch(RedisException)
            {
                _result = false;
            }
            
            return _result;
        }
        
        /// <summary>
        /// 判断TypeName是否存在
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>是否存在</returns>
        /// 时间：2016/8/4 15:38
        /// 备注：
        public bool Contains<T>()
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    return typedclient.TypeIdsSet.Count > 0;
                }
            }
        }
        
        /// <summary>
        /// 判断KEY是否存在
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">KeyId</param>
        /// <returns>是否存在</returns>
        /// 时间：2016/8/4 15:17
        /// 备注：
        public bool ContainsKey<T>(string id)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    string _key = string.Format("urn:{0}:{1}", typeof(T).Name.ToLower(), id);
                    return typedclient.ContainsKey(_key);
                }
            }
        }
        
        /// <summary>
        /// 创建Redis锁操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="redisLockFactroy">委托</param>
        /// 时间：2016/11/4 14:46
        /// 备注：
        public void CreateLock<T>(Action<IRedisTypedClient<T>> redisLockFactroy)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    using(typedclient.AcquireLock())
                    {
                        redisLockFactroy(typedclient);
                    }
                }
            }
        }
        
        /// <summary>
        /// 创建Redis事务操作
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="redisTransactionFactroy">委托</param>
        /// 时间：2016/11/4 14:47
        /// 备注：
        public void CreateTransaction<T>(Action<IRedisTypedTransaction<T>> redisTransactionFactroy)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    using(IRedisTypedTransaction<T> trans = typedclient.CreateTransaction())
                    {
                        redisTransactionFactroy(trans);
                    }
                }
            }
        }
        
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        public void Delete<T>(T item)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    typedclient.Delete(item);
                }
            }
        }
        
        /// <summary>
        /// 删除所有缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        public void DeleteAll<T>()
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    typedclient.DeleteAll();
                }
            }
        }
        
        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">KeyId</param>
        /// 时间：2016/8/3 14:20
        /// 备注：
        public void DeleteById<T>(string id)
        where T : IHasId<string>
        {
            T _finded = Get<T>(id);
            
            if(_finded != null)
            {
                Delete<T>(_finded);
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// 时间：2016/8/4 15:54
        /// 备注：
        public void Dispose()
        {
            if(PRM != null)
            {
                PRM.Dispose();
            }
        }
        
        /// <summary>
        /// 根据keyId取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">KeyId</param>
        /// <returns>泛型</returns>
        public T Get<T>(string id)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    return typedclient.GetById(id);
                }
            }
        }
        
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>集合</returns>
        public IList<T> GetAll<T>()
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    return typedclient.GetAll();
                }
            }
        }
        
        /// <summary>
        /// 条件获取
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keySelector">条件委托</param>
        /// <returns>集合</returns>
        public IEnumerable<T> GetAll<T>(Func<T, bool> keySelector)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    return typedclient.GetAll().Where(keySelector);
                }
            }
        }
        
        /// <summary>
        /// 依据HashId条件查询
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="hashId">HashId</param>
        /// <param name="dataKey">关键码值</param>
        /// <param name="keySelector">条件委托</param>
        /// <returns>IQueryable</returns>
        public IQueryable<T> GetAll<T>(string hashId, string dataKey, Expression<Func<T, bool>> keySelector)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                var _filtered = redisClient.GetAllEntriesFromHash(hashId).Where(c => c.Value.Equals(dataKey, StringComparison.InvariantCultureIgnoreCase));
                var _ids = _filtered.Select(c => c.Key);
                return redisClient.As<T>().GetByIds(_ids).AsQueryable().Where(keySelector);
            }
        }
        
        /// <summary>
        /// 依据HashId获取数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="hashId">HashId</param>
        /// <param name="dataKey">关键码值</param>
        /// <returns>IQueryable</returns>
        public IQueryable<T> GetAll<T>(string hashId, string dataKey)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                var _filtered = redisClient.GetAllEntriesFromHash(hashId).Where(c => c.Value.Equals(dataKey, StringComparison.InvariantCultureIgnoreCase));
                var _ids = _filtered.Select(c => c.Key);
                return redisClient.As<T>().GetByIds(_ids).AsQueryable();
            }
        }
        
        /// <summary>
        ///  同步将内存数据存储到硬盘
        /// </summary>
        public void Save()
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                redisClient.Save();
            }
        }
        
        /// <summary>
        /// 异步将内存数据存储到硬盘
        /// </summary>
        public void SaveAsync()
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                redisClient.SaveAsync();
            }
        }
        
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        public void Set<T>(T item)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    typedclient.Store(item);
                }
            }
        }
        
        /// <summary>
        /// 设置Hash类型缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        /// <param name="hashId">HashId</param>
        /// <param name="dataKey">关键码值</param>
        /// <param name="keyName">关键码值属性</param>
        public void Set<T>(T item, string hashId, string dataKey, string keyName)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                Type _type = item.GetType();
                PropertyInfo _prop = _type.GetProperty(keyName);
                redisClient.SetEntryInHash(hashId, _prop.GetValue(item, null).ToString(), dataKey.ToLower());
                redisClient.As<T>().Store(item);
            }
        }
        
        /// <summary>
        /// 设置集合缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="listItems">泛型集合</param>
        public void SetAll<T>(IEnumerable<T> listItems)
        where T : IHasId<string>
        {
            using(IRedisClient redisClient = PRM.GetClient())
            {
                using(IRedisTypedClient<T> typedclient = redisClient.As<T>())
                {
                    typedclient.StoreAll(listItems);
                }
            }
        }
        
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        /// 时间：2016/8/17 17:00
        /// 备注：
        public void Update<T>(T item)
        where T : IHasId<string>
        {
            if(ContainsKey<T>(item.Id))
            {
                Delete<T>(item);
            }
            
            Set<T>(item);
        }
        
        private PooledRedisClientManager CreateManager(
            string[] readWriteHosts, string[] readOnlyHosts, long defaultDb = 0)
        {
            RedisClientManagerConfig _redisConfig = new RedisClientManagerConfig();
            _redisConfig.AutoStart = true;
            _redisConfig.MaxReadPoolSize = readOnlyHosts.Length * 20;
            _redisConfig.MaxWritePoolSize = readWriteHosts.Length * 20;
            _redisConfig.DefaultDb = defaultDb;
            PooledRedisClientManager _prm = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, _redisConfig);
            return _prm;
        }
        
        #endregion Methods
    }
}