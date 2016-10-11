namespace YanZhiwei.DotNet.EntLib4.Utilities
{
    using System;
    using System.Data;
    using System.Data.Common;
    
    using Microsoft.Practices.EnterpriseLibrary.Data;
    
    /// <summary>
    /// 企业库 4.1 数据访问帮助类
    /// </summary>
    public class EntLibDataOperator
    {
        #region Fields
        
        private static readonly object looker = new object();
        
        private readonly Database db;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cfgDataBaseName">需要实例化DataBase名次.</param>
        public EntLibDataOperator(string cfgDataBaseName)
        {
            db = DatabaseFactory.CreateDatabase(cfgDataBaseName);
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public EntLibDataOperator()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// Database
        /// </summary>
        public Database dataBase
        {
            get
            {
                lock(looker)
                {
                    return db;
                }
            }
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 开启事务支持
        /// </summary>
        /// <returns>LocalDbTransaction</returns>
        public LocalDbTransaction BeginTranscation()
        {
            return new LocalDbTransaction(dataBase) { };
        }
        
        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string sql, DbParameter[] parameters)
        {
            return ExecuteBase<DataSet>(sql, parameters, (db, dbCmd) => db.ExecuteDataSet(dbCmd));
        }
        
        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters)
        {
            DataSet _result = ExecuteDataSet(sql, parameters);
            
            if(_result != null && _result.Tables.Count > 0)
                return _result.Tables[0];
                
            return null;
        }
        
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters)
        {
            return ExecuteBase<int>(sql, parameters, (db, dbCmd) => db.ExecuteNonQuery(dbCmd));
        }
        
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="localTranscation">LocalDbTransaction</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(LocalDbTransaction localTranscation, string sql, DbParameter[] parameters)
        {
            int _result = 0;
            DbTransaction _dbTranscation = localTranscation.TransactionObj;
            DbConnection _curConnection = _dbTranscation.Connection;
            Database _dataBase = localTranscation.DataBaseObj;
            
            if(_curConnection.State != ConnectionState.Open)
                _curConnection.Open();
                
            using(DbCommand dbCommand = _dataBase.GetSqlStringCommand(sql))
            {
                if(parameters != null)
                    dbCommand.Parameters.AddRange(parameters);
                    
                _result = _dataBase.ExecuteNonQuery(dbCommand, _dbTranscation);
            }
            return _result;
        }
        
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            return ExecuteBase<IDataReader>(sql, parameters, (db, dbCmd) => db.ExecuteReader(dbCmd));
        }
        
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回对象</returns>
        /// 时间：2016/10/11 13:27
        /// 备注：
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            return ExecuteBase<object>(sql, parameters, (db, dbCmd) => db.ExecuteScalar(dbCmd));
        }
        
        private T ExecuteBase<T>(string sql, DbParameter[] parameters, Func<Database, DbCommand, T> executeHanlder)
        {
            using(DbCommand dbCommand = dataBase.GetSqlStringCommand(sql))
            {
                if(parameters != null)
                    dbCommand.Parameters.AddRange(parameters);
                    
                return executeHanlder(dataBase, dbCommand);
            }
        }
        
        #endregion Methods
    }
}