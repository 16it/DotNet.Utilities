namespace YanZhiwei.DotNet.EntLib4.Utilities
{
    using System;
    
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
    
    using YanZhiwei.DotNet2.Utilities.Operator;
    
    /// <summary>
    /// 企业库 缓存帮助类
    /// </summary>
    public class EntLibCacheManger
    {
        #region Fields
        
        /*
         *在Caching Application Block中，主要提供以下四种保存缓存数据的途径，
         *分别是：内存存储（默认）、独立存储（Isolated Storage）、
         *数据库存储（DataBase Cache Storage）和自定义存储（Custom Cache Storage）。
         *In-Memory：保存在内存中。
         *Isolated Storage Cache Store：系统将缓存的信息保存在独立文件中（C:\Users\<<user name>>\AppData\Local\IsolatedStorage）。
         *Data Cache Storage：将缓存数据保存在数据库中。（需要运行CreateCachingDatabase.sql脚本）
         *Custom Cache Storage：自己扩展的处理器。我们可以将数据保存在注册表中或文本文件中。
        
         *
         * 缓存等级，在企业库的缓存模块中已经提供了4个缓存等级：Low，Normal，High和NotRemovable，在超出最大缓存数量后会自动根据缓存等级来移除对象。
        *  过期方式，企业库默认提供4种过期方式
        * AbsoluteTime：绝对是时间过期，传递一个时间对象指定到时过期
        * SlidingTime：缓存在最后一次访问之后多少时间后过期，默认为2分钟，有2个构造函数可以指定一个过期时间或指定一个过期时间和一个最后使用时
        * ExtendedFormatTime ：指定过期格式，以特定的格式来过期，通过ExtendedFormat.cs类来包装过期方式，具体可参照ExtendedFormat.cs，源代码中已经给出了很多方式
        * FileDependency：依赖于文件过期，当所依赖的文件被修改则过期，这个我觉得很有用，因为在许多网站，如论坛、新闻系统等都需要大量的配置，可以将配置文件信息进行缓存，将依赖项设为配置文件，这样当用户更改了配置文件后通过ICacheItemRefreshAction.Refresh可以自动重新缓存。
         *
         * new ExtendedFormatTime("0 0 * * *") stands for Minutes, Hours, Days, Months, DaysOfWeeks.  Spaces between 0 and * are there for parsing purpose.
         * Extended format syntax :
         * Minute - 0-59
         * Hour - 0-23
         * Day of month - 1-31
         * Month - 1-12
         * Day of week - 0-6 (Sunday is 0)
         * Wildcards - * means run every
        
         * Examples:
         * * * * * * - expires every minute
         * 5 * * * * - expire 5th minute of every hour
         * * 21 * * * - expire every minute of the 21st hour of every day
         * 31 15 * * * - expire 3:31 PM every day
         * 7 4 * * 6 - expire Saturday 4:07 AM
         * 15 21 4 7 * - expire 9:15 PM on 4 July
         * Therefore 6 6 6 6 1 means:
         * have we crossed/entered the 6th minute AND
         * have we crossed/entered the 6th hour AND
         * have we crossed/entered the 6th day AND
         * have we crossed/entered the 6th month AND
         * have we crossed/entered A MONDAY?
         */
        private static readonly object looker = new object();
        
        private ICacheManager cacheMgr = null;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntLibCacheManger()
        {
            cacheMgr = CacheFactory.GetCacheManager();
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheMangeName">缓存名称</param>
        public EntLibCacheManger(string cacheMangeName)
        {
            cacheMgr = CacheFactory.GetCacheManager(cacheMangeName);
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// ICacheManager对象
        /// </summary>
        public ICacheManager CacheManager
        {
            get
            {
                lock(looker)
                {
                    return cacheMgr;
                }
            }
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 添加绝对时间缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="absoluteTime">缓存过期绝对时间</param>
        /// <param name="itemRefreshFactory">缓存过期委托</param>
        public void AddAbsoluteTime(string key, object value, DateTime absoluteTime, ICacheItemRefreshAction itemRefreshFactory)
        {
            CacheManager.Add(key, value, CacheItemPriority.Normal, itemRefreshFactory, new AbsoluteTime(absoluteTime) { });
        }
        
        /// <summary>
        /// 添加绝对时间缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="absoluteTime">缓存过期绝对时间</param>
        public void AddAbsoluteTime(string key, object value, DateTime absoluteTime)
        {
            AddAbsoluteTime(key, value, absoluteTime, null);
        }
        
        /// <summary>
        /// 添加绝对时间缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="absoluteTime">缓存过期绝对时间</param>
        public void AddAbsoluteTime(string key, object value, TimeSpan absoluteTime)
        {
            AddAbsoluteTime(key, value, absoluteTime, null);
        }
        
        /// <summary>
        /// 添加绝对时间缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="absoluteTime">缓存过期绝对时间</param>
        /// <param name="itemRefreshFactory">缓存过期委托</param>
        public void AddAbsoluteTime(string key, object value, TimeSpan absoluteTime, ICacheItemRefreshAction itemRefreshFactory)
        {
            CacheManager.Add(key, value, CacheItemPriority.Normal, itemRefreshFactory, new AbsoluteTime(absoluteTime) { });
        }
        
        /// <summary>
        /// 添加特定的格式来过期缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="extendedFormatString">过期时间</param>
        /// <param name="itemRefreshFactory">缓存过期委托</param>
        public void AddExtendedFormatTime(string key, object value, string extendedFormatString, ICacheItemRefreshAction itemRefreshFactory)
        {
            cacheMgr.Add(key, value, CacheItemPriority.Normal, itemRefreshFactory, new ExtendedFormatTime(extendedFormatString) { });
        }
        
        /// <summary>
        /// 添加特定的格式来过期缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="extendedFormatString">过期时间</param>
        public void AddExtendedFormatTime(string key, object value, string extendedFormatString)
        {
            AddExtendedFormatTime(key, value, extendedFormatString);
        }
        
        /// <summary>
        /// 添加缓存文件依赖
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">文件路径</param>
        public void AddFileDependency(string key, object value, string filePath)
        {
            ValidateOperator.Begin().CheckFileExists(filePath);
            AddFileDependency(key, value, filePath, null);
        }
        
        /// <summary>
        /// 添加缓存文件依赖
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="itemRefreshFactory">缓存过期委托</param>
        public void AddFileDependency(string key, object value, string filePath, ICacheItemRefreshAction itemRefreshFactory)
        {
            ValidateOperator.Begin().CheckFileExists(filePath);
            FileDependency _fileDependency = new FileDependency(filePath);
            CacheManager.Add(key, value, CacheItemPriority.Normal, itemRefreshFactory, _fileDependency);
        }
        
        /// <summary>
        /// 添加滑动过期缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="slidingTime">滑动过期时间</param>
        public void AddSlidingTime(string key, object value, TimeSpan slidingTime)
        {
            AddSlidingTime(key, value, slidingTime, null);
        }
        
        /// <summary>
        /// 添加滑动过期缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="slidingTime">滑动过期时间</param>
        /// <param name="itemRefreshFactory">缓存过期委托</param>
        public void AddSlidingTime(string key, object value, TimeSpan slidingTime, ICacheItemRefreshAction itemRefreshFactory)
        {
            CacheManager.Add(key, value, CacheItemPriority.Normal, itemRefreshFactory, new SlidingTime(slidingTime) { });
        }
        
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T GetData<T>(string key)
        {
            T _result = default(T);
            
            if(CacheManager.Contains(key))
            {
                _result = (T)CacheManager.GetData(key);
            }
            
            return _result;
        }
        
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            if(CacheManager.Contains(key))
            {
                CacheManager.Remove(key);
            }
        }
        
        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public void RemoveAll()
        {
            if(CacheManager != null)
            {
                CacheManager.Flush();
            }
        }
        
        #endregion Methods
    }
}