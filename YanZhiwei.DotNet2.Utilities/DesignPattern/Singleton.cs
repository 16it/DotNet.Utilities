namespace YanZhiwei.DotNet2.Utilities.DesignPattern
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// 泛型单列
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <seealso cref="YanZhiwei.DotNet2.Utilities.DesignPattern.Singleton" />
    public class Singleton<T> : Singleton
    {
        static T instance;
        
        /// <summary>
        /// 泛型实例
        /// </summary>
        public static T Instance
        {
            get
            {
                return instance;
            }
            
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }
    
    /// <summary>
    /// 单例列表
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class SingletonList<T> : Singleton<IList<T>>
    {
        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }
        
        /// <summary>
        /// 泛型实例
        /// </summary>
        public new static IList<T> Instance
        {
            get
            {
                return Singleton<IList<T>>.Instance;
            }
        }
    }
    
    /// <summary>
    /// 单例字典
    /// </summary>
    /// <typeparam name="TKey">字典Key类型</typeparam>
    /// <typeparam name="TValue">字典Value类型.</typeparam>
    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        static SingletonDictionary()
        {
            Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }
        
        /// <summary>
        /// 泛型实例
        /// </summary>
        public new static IDictionary<TKey, TValue> Instance
        {
            get
            {
                return Singleton<Dictionary<TKey, TValue>>.Instance;
            }
        }
    }
    
    /// <summary>
    /// 单例字典
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet2.Utilities.DesignPattern.Singleton" />
    public class Singleton
    {
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }
        
        static readonly IDictionary<Type, object> allSingletons;
        
        /// <summary>
        /// 所有字典单例
        /// </summary>
        public static IDictionary<Type, object> AllSingletons
        {
            get
            {
                return allSingletons;
            }
        }
    }
}