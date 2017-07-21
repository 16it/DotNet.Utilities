namespace YanZhiwei.DotNet.Framework.Data
{
    using DotNet.Framework.Contract;
    using DotNet2.Utilities.Collection;
    using DotNet3._5.Utilities.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using YanZhiwei.DotNet.EntityFramework.Utilities;
    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.ExtendException;
    using YanZhiwei.DotNet3._5.Utilities.CallContext;

    /// <summary>
    /// 实现Repository通用泛型数据访问模式
    /// </summary>
    /// <typeparam name="F">仓储主键类型</typeparam>
    /// <seealso cref="System.Data.Entity.DbContext" />
    /// <seealso cref="YanZhiwei.DotNet.Framework.Data.IDataRepository{F}" />
    /// <seealso cref="System.IDisposable" />
    public abstract class DbContextBase<F> : DbContext, IDataRepository<F>, IDisposable, IUnitOfWork
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public DbContextBase(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="auditLogger">IAuditable</param>
        public DbContextBase(string connectionString, IAuditable auditLogger)
        : this(connectionString)
        {
            AuditLogger = auditLogger;
        }

        /// <summary>
        /// 日志接口
        /// </summary>
        public IAuditable AuditLogger
        {
            get;
            set;
        }

        /// <summary>
        /// 获取 是否开启事务提交
        /// </summary>
        public virtual bool TransactionEnabled
        {
            get { return Database.CurrentTransaction != null; }
        }

        /// <summary>
        /// 显式开启数据上下文事务
        /// </summary>
        /// <param name="isolationLevel">指定连接的事务锁定行为</param>
        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (Database.CurrentTransaction == null)
            {
                Database.BeginTransaction(isolationLevel);
            }
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public virtual void Commit()
        {
            DbContextTransaction transaction = Database.CurrentTransaction;
            if (transaction != null)
            {
                try
                {
                    transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException is SqlException)
                    {
                        SqlException sqlEx = ex.InnerException.InnerException as SqlException;
                        string msg = DataBaseHelper.GetSqlExceptionMessage(sqlEx.Number);
                        throw new DataAccessException("提交数据更新时发生异常：" + msg, sqlEx);
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        public virtual void Delete<T>(T entity)
        where T : ModelBase<F>
        {
            try
            {
                this.Entry<T>(entity).State = EntityState.Deleted;
                this.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keyValues">查询依据的键</param>
        /// <returns>
        /// 实体类
        /// </returns>
        public virtual T Find<T>(params object[] keyValues)
        where T : ModelBase<F>
        {
            return this.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="conditions">委托.</param>
        /// <returns>
        /// 集合
        /// </returns>
        public virtual List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null)
        where T : ModelBase<F>
        {
            if (conditions == null)
                return this.Set<T>().ToList();
            else
                return this.Set<T>().Where(conditions).ToList();
        }

        /// <summary>
        /// 分页查找
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="S">泛型</typeparam>
        /// <param name="conditions">查找条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">分页集合</param>
        /// <returns>PagedList</returns>
        public virtual PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex)
        where T : ModelBase<F>
        {
            var queryList = conditions == null ? this.Set<T>() : this.Set<T>().Where(conditions) as IQueryable<T>;
            return queryList.OrderByDescending(orderBy).ToPagedList(pageIndex, pageSize);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns>
        /// 实体类
        /// </returns>
        public virtual T Insert<T>(T entity)
        where T : ModelBase<F>
        {
            try
            {
                this.Set<T>().Add(entity);
                this.SaveChanges();
                return entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        /// <summary>
        /// 显式回滚事务，仅在显式开启事务后有用
        /// </summary>
        public virtual void Rollback()
        {
            if (Database.CurrentTransaction != null)
            {
                Database.CurrentTransaction.Rollback();
            }
        }

        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库。
        /// </summary>
        /// <returns>
        /// 已写入基础数据库的对象的数目。
        /// </returns>
        public override int SaveChanges()
        {
            this.WriteAuditLog();
            return base.SaveChanges();
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="validateOnSaveEnabled">保存时验证实体有效性，涉及到按需更新</param>
        /// <returns>影响行数</returns>
        public int SaveChanges(bool validateOnSaveEnabled)
        {
            bool _originalValidateOnSaveEnabled = Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return this.SaveChanges();
            }
            finally
            {
                if (_originalValidateOnSaveEnabled)
                {
                    Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }

        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。
        /// </summary>
        /// <param name="cancellationToken">等待任务完成期间要观察的 <see cref="T:System.Threading.CancellationToken" />。</param>
        /// <returns>
        /// 表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。
        /// </returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.WriteAuditLog();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Sql语句查询
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IEnumerable</returns>
        public virtual IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// 存储过程
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>集合</returns>
        /// <exception cref="System.Exception">不支持的参数类型。</exception>
        public IList<T> ExecuteStoredProcedureList<T>(string commandText, params object[] parameters) where T : ModelBase<F>
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var _paramter = parameters[i] as DbParameter;
                    if (_paramter == null)
                        throw new Exception("不支持的参数类型。");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + _paramter.ParameterName;
                    if (_paramter.Direction == ParameterDirection.InputOutput || _paramter.Direction == ParameterDirection.Output)
                    {
                        commandText += " output";
                    }
                }
            }

            var _result = this.Database.SqlQuery<T>(commandText, parameters).ToList();

            bool _acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < _result.Count; i++)
                    _result[i] = AttachEntityToContext(_result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = _acd;
            }

            return _result;
        }

        /// <summary>
        /// 将实体类附加上上下文.
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns>实体类</returns>
        protected virtual T AttachEntityToContext<T>(T entity) where T : ModelBase<F>
        {
            var _alreadyAttached = Set<T>().Local.FirstOrDefault(x => x.ID.Equals(entity.ID));
            if (_alreadyAttached == null)
            {
                Set<T>().Attach(entity);
                return entity;
            }
            return _alreadyAttached;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns>
        /// 实体类
        /// </returns>
        public virtual T Update<T>(T entity)
        where T : ModelBase<F>
        {
            try
            {
                var set = this.Set<T>();
                set.Attach(entity);
                this.Entry<T>(entity).State = EntityState.Modified;
                this.SaveChanges();
                return entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        /// <summary>
        /// 日志拦截写入
        /// </summary>
        internal void WriteAuditLog()
        {
            if (this.AuditLogger == null)
                return;

            foreach (var dbEntry in this.ChangeTracker.Entries<ModelBase>().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                AuditableAttribute _auditableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), false).SingleOrDefault() as AuditableAttribute;

                if (_auditableAttr == null)
                    continue;

                string _operaterName = CommonCallContext.Current.Operater.Name;
                Task.Factory.StartNew(() =>
                {
                    TableAttribute _tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                    string _tableName = _tableAttr != null ? _tableAttr.Name : dbEntry.Entity.GetType().Name;
                    string _moduleName = dbEntry.Entity.GetType().FullName.Split('.').Skip(1).FirstOrDefault();
                    this.AuditLogger.WriteLog(dbEntry.Entity.ID, _operaterName, _moduleName, _tableName, dbEntry.State.ToString(), dbEntry.Entity);
                });
            }
        }
    }
}