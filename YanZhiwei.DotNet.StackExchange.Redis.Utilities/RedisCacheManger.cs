namespace YanZhiwei.DotNet.StackExchange.Redis.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using ServiceStack.Redis;
    using ServiceStack.Redis.Generic;

    /// <summary>
    /// 基于ServiceStack.Redis 缓存帮助类
    /// </summary>
    /// 时间：2016/8/3 13:32
    /// 备注：
    public class RedisCacheManger
    {
        #region Fields

        /// <summary>
        /// IRedisClient
        /// </summary>
        public readonly IRedisClient RedisClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisClient">IRedisClient</param>
        public RedisCacheManger(IRedisClient redisClient)
        {
            RedisClient = redisClient;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        public void Delete<T>(T item)
        {
            IRedisTypedClient<T> _typedclient = RedisClient.As<T>();
            _typedclient.Delete(item);
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        public void DeleteAll<T>(T item)
        {
            IRedisTypedClient<T> _typedclient = RedisClient.As<T>();
            _typedclient.DeleteAll();
        }

        /// <summary>
        /// 根据keyId取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="id">KeyId</param>
        /// <returns>泛型</returns>
        public T Get<T>(string id)
        {
            IRedisTypedClient<T> _typedclient = RedisClient.As<T>();
            return _typedclient.GetById(id.ToLower());
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>集合</returns>
        public IList<T> GetAll<T>()
        {
            IRedisTypedClient<T> _typedclient = RedisClient.As<T>();
            return _typedclient.GetAll();
        }

        /// <summary>
        /// 条件获取
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keySelector">条件委托</param>
        /// <returns>集合</returns>
        public IEnumerable<T> GetAll<T>(Func<T, bool> keySelector)
        {
            IRedisTypedClient<T> _typedclient = RedisClient.As<T>();
            return _typedclient.GetAll().Where(keySelector);
        }

        /// <summary>
        /// 依据HashId条件查询
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="hash">HashId</param>
        /// <param name="value">关键码值</param>
        /// <param name="keySelector">条件委托</param>
        /// <returns>IQueryable</returns>
        public IQueryable<T> GetAll<T>(string hash, string value, Expression<Func<T, bool>> keySelector)
        {
            var _filtered = RedisClient.GetAllEntriesFromHash(hash).Where(c => c.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            var _ids = _filtered.Select(c => c.Key);
            return RedisClient.As<T>().GetByIds(_ids).AsQueryable().Where(keySelector);
        }

        /// <summary>
        /// 依据HashId获取数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="hash">HashId</param>
        /// <param name="value">关键码值</param>
        /// <returns>IQueryable</returns>
        public IQueryable<T> GetAll<T>(string hash, string value)
        {
            var _filtered = RedisClient.GetAllEntriesFromHash(hash).Where(c => c.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            var _ids = _filtered.Select(c => c.Key);
            return RedisClient.As<T>().GetByIds(_ids).AsQueryable();
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        public void Set<T>(T item)
        {
            IRedisTypedClient<T> _typedclient = RedisClient.As<T>();
            _typedclient.Store(item);
        }

        /// <summary>
        /// 设置Hash类型缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        /// <param name="hash">HashId</param>
        /// <param name="value">关键码值</param>
        /// <param name="keyName">关键码值属性</param>
        public void Set<T>(T item, string hash, string value, string keyName)
        {
            Type _type = item.GetType();
            PropertyInfo _prop = _type.GetProperty(keyName);
            RedisClient.SetEntryInHash(hash, _prop.GetValue(item, null).ToString(), value.ToLower());
            RedisClient.As<T>().Store(item);
        }

        /// <summary>
        /// 设置Hash类型缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="item">缓存项</param>
        /// <param name="hash">HashId</param>
        /// <param name="value">关键码值</param>
        /// <param name="keyName">关键码值属性</param>
        public void Set<T>(T item, List<string> hash, List<string> value, string keyName)
        {
            Type _type = item.GetType();
            PropertyInfo _prop = _type.GetProperty(keyName);

            for(int i = 0; i < hash.Count; i++)
            {
                RedisClient.SetEntryInHash(hash[i], _prop.GetValue(item, null).ToString(), value[i].ToLower());
            }

            RedisClient.As<T>().Store(item);
        }

        /// <summary>
        /// 设置集合缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="listItems">泛型集合</param>
        public void SetAll<T>(List<T> listItems)
        {
            var typedclient = RedisClient.As<T>();
            typedclient.StoreAll(listItems);
        }

        /// <summary>
        /// 设置Hash集合缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="hash">HashId</param>
        /// <param name="value">关键码值</param>
        /// <param name="keyName">关键码值属性</param>
        public void SetAll<T>(List<T> list, string hash, string value, string keyName)
        {
            foreach(var item in list)
            {
                Type _type = item.GetType();
                PropertyInfo _prop = _type.GetProperty(keyName);
                RedisClient.SetEntryInHash(hash, _prop.GetValue(item, null).ToString(), value.ToLower());
                RedisClient.As<T>().StoreAll(list);
            }
        }

        /// <summary>
        /// 设置Hash集合缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="hash">HashId</param>
        /// <param name="value">关键码值</param>
        /// <param name="keyName">关键码值属性</param>
        public void SetAll<T>(List<T> list, List<string> hash, List<string> value, string keyName)
        {
            foreach(var item in list)
            {
                Type _type = item.GetType();
                PropertyInfo _prop = _type.GetProperty(keyName);

                for(int i = 0; i < hash.Count; i++)
                {
                    RedisClient.SetEntryInHash(hash[i], _prop.GetValue(item, null).ToString(), value[i].ToLower());
                }

                RedisClient.As<T>().StoreAll(list);
            }
        }

        #endregion Methods

        #region Other

        //public long PublishMessage(string channel, object item)
        //{
        //    var ret = _redisClient.PublishMessage(channel, JsonConvert.SerializeObject(item));
        //    return ret;
        //}

        #endregion Other
    }
}