namespace YanZhiwei.DotNet.StackExchange.Redis.Utilities
{
    using ServiceStack.Redis;
    using ServiceStack.Redis.Generic;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Redis缓存帮助类
    /// </summary>
    public class RedisCacheManger : IDisposable
    {
        #region Fields

        /// <summary>
        /// The redis
        /// </summary>
        public RedisClient Redis = new RedisClient("127.0.0.1", 6379);

        /// <summary>
        /// 默认缓存过期时间单位秒
        /// </summary>
        public int secondsTimeOut = 30 * 60;

        //缓存池
        private PooledRedisClientManager prcm = new PooledRedisClientManager();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="openPooledRedis">是否开启缓冲池</param>
        public RedisCacheManger(bool openPooledRedis = false)
        {
            if (openPooledRedis)
            {
                prcm = CreateManager(new string[] { "127.0.0.1:6379" }, new string[] { "127.0.0.1:6379" });
                Redis = prcm.GetClient() as RedisClient;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 缓冲池
        /// </summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        /// <returns></returns>
        public static PooledRedisClientManager CreateManager(
            string[] readWriteHosts, string[] readOnlyHosts)
        {
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts,
                new RedisClientManagerConfig
                {
                    MaxWritePoolSize = readWriteHosts.Length * 5,
                    MaxReadPoolSize = readOnlyHosts.Length * 5,
                    AutoStart = true,
                });
        }

        /// <summary>
        /// 增加Key/Value存储
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">值</param>
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>
        /// <returns>增加是否成功</returns>
        public bool Add<T>(string key, T t, int timeout)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(key, secondsTimeOut);
            }
            return Redis.Add<T>(key, t);
        }

        /// <summary>
        /// 链表操作——添加单个实体到链表中
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="listId">链表Key</param>
        /// <param name="item">实体类</param>
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>
        public void AddList<T>(string listId, T item, int timeout = 0)
        {
            IRedisTypedClient<T> _typeClient = Redis.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(listId, secondsTimeOut);
            }
            var _redisList = _typeClient.Lists[listId];
            _redisList.Add(item);
            _typeClient.Save();
        }

        /// <summary>
        /// 根据IEnumerable数据添加链表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="listId">链表Key</param>
        /// <param name="data">集合</param>
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>
        public void AddList<T>(string listId, IEnumerable<T> data, int timeout = 0)
        {
            IRedisTypedClient<T> _typeClient = Redis.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(listId, secondsTimeOut);
            }
            var _redisList = _typeClient.Lists[listId];
            _redisList.AddRange(data);
            _typeClient.Save();
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (Redis != null)
            {
                Redis.Dispose();
                Redis = null;
            }
        }

        /// <summary>
        /// 获取Key/Value存储
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns>泛型</returns>
        public T Get<T>(string key)
        {
            return Redis.Get<T>(key);
        }

        /// <summary>
        /// 获取链表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="listId">链表Key</param>
        /// <returns>泛型集合</returns>
        public IEnumerable<T> GetList<T>(string listId)
        {
            IRedisTypedClient<T> _typeClient = Redis.As<T>();
            return _typeClient.Lists[listId];
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>移除是否成功</returns>
        public bool Remove(string key)
        {
            return Redis.Remove(key);
        }

        /// <summary>
        /// 在链表中删除单个实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="listId">链表Key</param>
        /// <param name="item">实体类</param>
        public void RemoveList<T>(string listId, T item)
        {
            IRedisTypedClient<T> _typeClient = Redis.As<T>();
            var _redisList = _typeClient.Lists[listId];
            _redisList.RemoveValue(item);
            _typeClient.Save();
        }

        /// <summary>
        /// 根据lambada表达式删除符合条件的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="keySelector"></param>
        public void RemoveList<T>(string listId, Func<T, bool> keySelector)
        {
            IRedisTypedClient<T> _typeClient = Redis.As<T>();
            var _redisList = _typeClient.Lists[listId];
            T _finded = _redisList.Where(keySelector).FirstOrDefault();
            if (_finded != null)
            {
                _redisList.RemoveValue(_finded);
                _typeClient.Save();
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">缓存建</param>
        /// <param name="t">缓存值</param>
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>
        /// <returns>是否设置成功</returns>
        public bool Set<T>(string key, T t, int timeout = 0)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(key, secondsTimeOut);
            }

            return Redis.Add<T>(key, t);
        }

        #endregion Methods
    }
}