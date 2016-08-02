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

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public void Delete<T>(string keyId)
        {
            IRedisTypedClient<T> _search = Redis.As<T>();
            T _finded = _search.GetById(keyId);
            if (_finded != null)
            {
                _search.DeleteById(keyId);
            }
        }

        /// <summary>
        /// 通过Key,Id获得泛型值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity<T>(string key, long id)
        {
            IRedisTypedClient<T> _search = Redis.As<T>();
            string entityname = typeof(T).Name;
            key = string.Format("{0}:{1}:{2}", key, entityname, id);
            return _search.GetValue(key);
        }

        /// <summary>
        /// 获得泛型类型T的所有值
        /// </summary>
        /// <returns></returns>
        public List<T> GetEntitys<T>()
        {
            IRedisTypedClient<T> _search = Redis.As<T>();
            return _search.GetAll().ToList();
        }

        /// <summary>
        /// 通过ket获得泛型类型的所有值
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public List<T> GetEntitys<T>(string keyId)
        {
            string entityname = typeof(T).Name;
            keyId = string.Format("{0}:{1}:{2}", keyId, entityname, "*");
            var search = Redis.SearchKeys(keyId).ToList();
            List<T> entitys = Redis.GetValues<T>(search);
            return entitys;
        }

        /// <summary>
        /// 通过key获得string类型
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public string GetKey(string keyId)
        {
            if (Redis.ContainsKey(keyId))
                return Redis.Get<string>(keyId);
            else
                return null;
        }

        /// <summary>
        /// 获得泛型类型的下一个序列
        /// </summary>
        /// <returns></returns>
        public long GetNextSequence<T>()
        {
            var entity = Redis.As<T>();
            return entity.GetNextSequence();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<T> GetPagedList<T>(string keyId, int start, int end)
        {
            string entityname = typeof(T).Name;
            string ids = string.Format("{0}:{1}", "ids", entityname);
            var search = Redis.GetRangeFromSortedList(ids, start, end);

            for (int i = 0; i < search.Count; i++)
            {
                string s = search[i];
                search[i] = string.Format("{0}:{1}:{2}", keyId, entityname, s);
            }
            List<T> entitys = Redis.GetValues<T>(search);
            return entitys;
        }

        /// <summary>
        /// 通过Id搜索
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public T SearchEneitys<T>(string keyId)
        {
            var entity = Redis.As<T>();
            return entity.GetById(keyId);
        }

        /// <summary>
        /// 搜索实体
        /// </summary>
        /// <param name=""></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public IEnumerable<T> SearchEntitys<T>(Func<T, bool> keySelector)
        {
            var entity = Redis.As<T>();
            return entity.GetAll().Where(keySelector);
        }

        /// <summary>
        /// 根据key Set存储
        /// </summary>
        /// <param name="keyid"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public void SetEntity<T>(string keyid, T val)
        {
            var entitys = Redis.As<T>();

            long id = (long)val.GetType().GetProperty("Id").GetValue(val, null);
            string entityname = typeof(T).Name.ToLower();
            keyid = string.Format("{0}:{1}:{2}", keyid, entityname, id);
            entitys.SetValue(keyid, val);
        }

        /// <summary>
        /// 存储值为string类型
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetKey(string keyId, string value)
        {
            if (Redis.ContainsKey(keyId))
            {
                Redis.Del(keyId);
            }
            Redis.Add<string>(keyId, value);
        }

        /// <summary>
        /// store 泛型类型数组
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public void StoreEntity<T>(T[] vals)
        {
            var entity = Redis.As<T>();
            entity.StoreAll(vals);
        }

        /// <summary>
        /// store泛型类型列表
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public void StoreEntity<T>(List<T> vals)
        {
            var entity = Redis.As<T>();
            entity.StoreAll(vals);
        }

        /// <summary>
        /// store 泛型值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void StroteEntity<T>(T value)
        {
            var entity = Redis.As<T>();
            entity.Store(value);
        }

        /// <summary>
        /// 通过key更新
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public void UpdateEntity<T>(string keyId, T val)
        {
            var entitys = Redis.As<T>();
            //获得id
            long id = (long)val.GetType().GetProperty("Id").GetValue(val, null);
            string entityname = typeof(T).Name;
            keyId = string.Format("{0}:{1}:{2}", keyId, entityname, id);

            entitys.SetValue(keyId, val);
        }
    }
}