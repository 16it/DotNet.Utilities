namespace YanZhiwei.DotNet3._5.Utilities.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    
    /// <summary>
    ///永久缓存基类
    /// </summary>
    public abstract class ForeverCache<TImplement, TKey, TValue>
        where TImplement : class
    {
        #region Fields
        
        private static TImplement instance;
        
        private Dictionary<TKey, TValue> cacheContainer = new Dictionary<TKey, TValue>();
        
        #endregion Fields
        
        #region Properties
        
        /// <summary>
        /// 单列
        /// </summary>
        public static TImplement Singleton
        {
            get
            {
                if(instance != null)
                    return instance;
                    
                var temp = Activator.CreateInstance<TImplement>();
                Interlocked.CompareExchange(ref instance, temp, default(TImplement));
                return instance;
            }
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 根据KEY取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public TValue Get(TKey key)
        {
            TValue result;
            return cacheContainer.TryGetValue(key, out result) ? result : default(TValue);
        }
        
        /// <summary>
        /// 获取所有键值
        /// </summary>
        /// <returns>所有键值</returns>
        public List<TValue> GetAllValues()
        {
            return cacheContainer.Values.ToList();
        }
        
        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="data">初始化数据源</param>
        /// <param name="keySelector">选择器</param>
        public void InitCache(IEnumerable<TValue> data, Func<TValue, TKey> keySelector)
        {
            cacheContainer = data.ToDictionary(keySelector, ent => ent);
        }
        
        #endregion Methods
    }
}