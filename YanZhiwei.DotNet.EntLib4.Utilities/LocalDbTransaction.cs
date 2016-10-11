namespace YanZhiwei.DotNet.EntLib4.Utilities
{
    using DotNet2.Utilities.Operator;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data.Common;
    
    /// <summary>
    /// 本地事务
    /// </summary>
    public class LocalDbTransaction : IDisposable
    {
        #region Fields
        
        /// <summary>
        /// Database对象
        /// </summary>
        public readonly Database Db;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataBase">Database</param>
        public LocalDbTransaction(Database dataBase)
        {
            ValidateOperator.Begin().NotNull(dataBase, "Database");
            Db = dataBase;
            DbConnection _dbConnection = Db.CreateConnection();
            _dbConnection.Open();
            TransactionObj = _dbConnection.BeginTransaction();
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// DbTransaction 对象
        /// </summary>
        public DbTransaction TransactionObj
        {
            get;
            set;
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if(TransactionObj != null)
            {
                TransactionObj.Commit();
            }
        }
        
        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if(TransactionObj != null)
            {
                TransactionObj.Dispose();
                TransactionObj = null;
            }
        }
        
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if(TransactionObj != null)
            {
                TransactionObj.Rollback();
            }
        }
        
        #endregion Methods
    }
}