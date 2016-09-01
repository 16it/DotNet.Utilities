namespace YanZhiwei.DotNet.Framework.Data
{
    using Contract;
    using Core.Cache;
    using PetaPoco;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    ///  DAL基类，通用数据访问模式
    /// </summary>
    /// 时间：2016-03-30 11:15
    /// 备注：
    public class DbContextBase : Database, IDisposable
    {
        #region Fields

        public readonly string CacheKeyPrefix = null;

        private static Regex rxParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);

        private string _paramPrefix = "@";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbProviderFactory">数据源提供实例</param>
        /// <param name="auditLogger">数据添加，删除，修改拦截接口</param>
        /// 时间：2016-03-28 10:13
        /// 备注：
        public DbContextBase(string connectionString, DbProviderFactory dbProviderFactory, ISqlAuditable auditLogger, string cacheKeyPrefix)
        : base(connectionString, dbProviderFactory)
        {
            this.CacheKeyPrefix = cacheKeyPrefix;
            this.AuditLogger = auditLogger;
            this.DataChangedEvent += DbContextBase_DataChangedEvent; ;
        }

        /// <summary>
        /// 初始化构造函数，默认Sql Server数据，不数据拦截
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// 时间：2016-03-28 10:14
        /// 备注：
        public DbContextBase(string connectionString)
        : this(connectionString, DbProviderFactories.GetFactory("System.Data.SqlClient"), null, "DotNetFrameworkData")
        {
        }

        /// <summary>
        /// 初始化构造函数，默认Sql Server数据
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="auditLogger">数据添加，删除，修改拦截接口</param>
        /// 时间：2016-03-28 10:15
        /// 备注：
        public DbContextBase(string connectionString, ISqlAuditable auditLogger)
        : this(connectionString, DbProviderFactories.GetFactory("System.Data.SqlClient"), auditLogger, "DotNetFrameworkData")
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 数据拦截接口
        /// </summary>
        public ISqlAuditable AuditLogger
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 将结果转换为缓存的列表，如缓存存在，直接返回，否则从数据源查询，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">泛型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="args">sql参数</param>
        /// <returns>集合</returns>
        /// 时间：2016/6/22 16:50
        /// 备注：
        public List<TSource> ToCacheList<TSource>(string sql, params object[] args)
        {
            string _key = GetKey(0, 0, sql, args);
            List<TSource> _result = (List<TSource>)CacheHelper.Get(_key);

            if(_result != null)
            {
                return _result;
            }

            _result = this.Fetch<TSource>(sql, args);
            CacheHelper.Set(_key, _result);
            return _result;
        }

        /// <summary>
        /// 查询分页数据结果，如缓存存在，直接返回，否则从数据源查找分页结果，并存入缓存中再返回
        /// </summary>
        /// <typeparam name="TSource">泛型</typeparam>
        /// <param name="page">第几页</param>
        /// <param name="itemsPerPage">每页多少条数据</param>
        /// <param name="sql">sql语句</param>
        /// <param name="args">sql参数</param>
        /// <returns>分页对象</returns>
        /// 时间：2016/6/22 16:51
        /// 备注：
        public Page<TSource> ToPageCache<TSource>(long page, long itemsPerPage, string sql, params object[] args)
        {
            string _key = GetKey(page, itemsPerPage, sql, args);
            Page<TSource> _result = (Page<TSource>)CacheHelper.Get(_key);

            if(_result != null)
            {
                return _result;
            }

            _result = this.Page<TSource>(page, itemsPerPage, sql, args);
            CacheHelper.Set(_key, _result);
            return _result;
        }

        private void DbContextBase_DataChangedEvent(Type type, string sql)
        {
            if(type != null)
            {
                Type _entityType = type;
                AuditableAttribute _auditableAttr = _entityType.GetCustomAttributes(typeof(AuditableAttribute), false).SingleOrDefault() as AuditableAttribute;

                if(_auditableAttr == null) return;

                int _userId = ServiceCallContext.Current.Operater.UserId;
                Thread _task = new Thread(() =>
                {
                    var _tableAttr = _entityType.GetCustomAttributes(typeof(TableNameAttribute), true);
                    string _tableName = _tableAttr.Length == 0 ? _entityType.Name : (_tableAttr[0] as TableNameAttribute).Value;
                    this.AuditLogger.WriteLog(_userId, _tableName, sql, DateTime.UtcNow);
                });
                _task.Start();
            }
        }

        private string GetKey(long page, long itemsPerPage, string sql, object[] args)
        {
            string _key = null;

            if(args != null)
            {
                var _new_args = new List<object>();
                sql = ProcessParams(sql, args, _new_args);
                sql = rxParamsPrefix.Replace(sql, m => _paramPrefix + m.Value.Substring(1));
                sql = sql.Replace("@@", "@");
                int i = 0;

                foreach(object arg in args)
                {
                    sql = sql.Replace(string.Format("@{0}", i), arg.ToString());
                    i++;
                }
            }

            _key = string.Format("{0}.{1}{2}{3}", CacheKeyPrefix, page, itemsPerPage, sql);
            return _key;
        }

        #endregion Methods
    }
}