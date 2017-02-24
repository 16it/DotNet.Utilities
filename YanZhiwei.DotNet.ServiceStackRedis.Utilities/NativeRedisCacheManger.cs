namespace YanZhiwei.DotNet.ServiceStackRedis.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ServiceStack.Redis;

    using YanZhiwei.DotNet.ProtoBuf.Utilities;

    /// <summary>
    /// NativeRedis操作封装
    /// </summary>
    public class NativeRedisCacheManger : IDisposable
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
        public NativeRedisCacheManger(string[] readWriteHosts, string[] readOnlyHosts, long defaultDb)
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
        public NativeRedisCacheManger(string ip, int port, long defaultDb)
        {
            string[] _hosts = new string[] { string.Format("{0}:{1}", ip, port) };
            PRM = CreateManager(_hosts, _hosts, defaultDb);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取HASH类型数据
        /// </summary>
        /// <param name="hashId">HASH ID</param>
        /// <param name="key">HASH KEY</param>
        /// <returns>泛型</returns>
        public T HashGet<T>(string hashId, string key)
            where T : class
        {
            using(IRedisClient redis = PRM.GetClient())
            {
                IRedisNativeClient _redisNative = (IRedisNativeClient)redis;
                byte[] _findedBuffer = _redisNative.HGet(hashId, Encoding.UTF8.GetBytes(key));

                if(_findedBuffer != null)
                    return ProtoBufHelper.Deserialize<T>(_findedBuffer);
                else
                    return null;
            }
        }

        /// <summary>
        /// 添加HASH类型
        /// </summary>
        /// <param name="hashId">HASH ID</param>
        /// <param name="source">需要设置的数据</param>
        public void HashSet<T>(string hashId, IEnumerable<IGrouping<string, T>> source)
            where T : class
        {
            Parallel.ForEach(source, (item, loopState) =>
            {
                using(IRedisClient redis = PRM.GetClient())
                {
                    IRedisNativeClient _redisNative = (IRedisNativeClient)redis;
                    byte[] _key = Encoding.UTF8.GetBytes(item.Key.ToString());
                    byte[] _value = ProtoBufHelper.Serialize(item);
                    _redisNative.HSet(hashId, _key, _value);
                }
            });
        }

        private PooledRedisClientManager CreateManager(
            string[] readWriteHosts, string[] readOnlyHosts, long defaultDb = 0)
        {
            RedisClientManagerConfig _redisConfig = new RedisClientManagerConfig();
            _redisConfig.AutoStart = true;
            _redisConfig.MaxReadPoolSize = readOnlyHosts.Length * 60;
            _redisConfig.MaxWritePoolSize = readWriteHosts.Length * 60;
            _redisConfig.DefaultDb = defaultDb;
            PooledRedisClientManager _prm = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, _redisConfig);
            return _prm;
        }

        #endregion Methods
    }
}