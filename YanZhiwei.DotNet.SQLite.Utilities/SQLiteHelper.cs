namespace YanZhiwei.DotNet.SQLite.Utilities
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Text;
    
    /// <summary>
    /// SQLite帮助类
    /// </summary>
    public class SQLiteHelper
    {
        #region Fields
        
        private string connectionString = string.Empty;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbpath">db路径</param>
        public SQLiteHelper(string dbpath)
        {
            connectionString = string.Format(@"Data Source={0}", dbpath);
        }
        
        #endregion Constructors
        
        #region Methods
        
        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using(SQLiteConnection sqlcon = new SQLiteConnection(connectionString))
            {
                using(SQLiteCommand sqlcmd = new SQLiteCommand(sql, sqlcon))
                {
                    if(parameters != null)
                        sqlcmd.Parameters.AddRange(parameters);
                        
                    using(SQLiteDataAdapter sqldap = new SQLiteDataAdapter(sqlcmd))
                    {
                        DataTable _table = new DataTable();
                        sqldap.Fill(_table);
                        return _table;
                    }
                }
            }
        }
        
        /// <summary>
        /// 执行sql语句，返回影响行数
        ///<para>eg: string sql = "INSERT INTO Test(Name,TypeName)values(@Name,@TypeName)";   </para>
        ///<para>SQLiteParameter[] parameters = new SQLiteParameter[]{   new SQLiteParameter("@Name",c+i.ToString()),  new SQLiteParameter("@TypeName",c.ToString())} </para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int _affectedRows = -1;
            using(SQLiteConnection sqlcon = new SQLiteConnection(connectionString))
            {
                sqlcon.Open();
                using(SQLiteCommand sqlcmd = new SQLiteCommand(sql, sqlcon))
                {
                    if(parameters != null)
                        sqlcmd.Parameters.AddRange(parameters);
                        
                    _affectedRows = sqlcmd.ExecuteNonQuery();
                }
            }
            return _affectedRows;
        }
        
        /// <summary>
        /// 执行sql语句，返回影响行数 带事物
        /// <para>eg:DataAccess.Instance.SQLHelper.ExecuteNonQueryWithTrans(new string[2] { _addSell, _updateProduct }) </para>
        /// </summary>
        /// <param name="sqlList">SQL语句集合</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQueryWithTrans(string[] sqlList)
        {
            int _affectedRows = -1;
            using(SQLiteConnection sqlcon = new SQLiteConnection(connectionString))
            {
                sqlcon.Open();
                DbTransaction _sqlTrans = sqlcon.BeginTransaction();
                
                try
                {
                    _affectedRows = 0;
                    
                    foreach(string sql in sqlList)
                    {
                        using(SQLiteCommand sqlcmd = new SQLiteCommand(sql, sqlcon))
                        {
                            _affectedRows += sqlcmd.ExecuteNonQuery();
                        }
                    }
                    
                    _sqlTrans.Commit();
                }
                catch(Exception)
                {
                    _sqlTrans.Rollback();
                    _affectedRows = -1;
                }
            }
            return _affectedRows;
        }
        
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            SQLiteConnection sqlcon = new SQLiteConnection(connectionString);
            using(SQLiteCommand sqlcmd = new SQLiteCommand(sql, sqlcon))
            {
                if(parameters != null)
                    sqlcmd.Parameters.AddRange(parameters);
                    
                sqlcon.Open();
                return sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Object</returns>
        public Object ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            using(SQLiteConnection sqlcon = new SQLiteConnection(connectionString))
            {
                using(SQLiteCommand sqlcmd = new SQLiteCommand(sql, sqlcon))
                {
                    if(parameters != null)
                        sqlcmd.Parameters.AddRange(parameters);
                        
                    sqlcon.Open();
                    return sqlcmd.ExecuteScalar(CommandBehavior.CloseConnection);
                }
            }
        }
        
        /// <summary>
        /// Inserts the row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public int InsertRow(DataRow row)
        {
            int _affectedRows = -1;
            using(SQLiteConnection sqlcon = new SQLiteConnection(connectionString))
            {
                try
                {
                    sqlcon.Open();
                    using(SQLiteCommand sqlcmd = CreateInsertCommand(row))
                    {
                        sqlcmd.Connection = sqlcon;
                        sqlcmd.CommandType = CommandType.Text;
                        _affectedRows = sqlcmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if(sqlcon.State != ConnectionState.Closed)
                    {
                        sqlcon.Close();
                        sqlcon.Dispose();
                    }
                }
            }
            return _affectedRows;
        }
        
        private string BuildInsertSQL(DataTable table)
        {
            StringBuilder _builder = new StringBuilder("INSERT INTO " + table.TableName + " (");
            StringBuilder _joinSql = new StringBuilder("VALUES (");
            bool _first = true;
            bool _identity = false;
            string _identityType = null;
            
            foreach(DataColumn column in table.Columns)
            {
                if(column.AutoIncrement)
                {
                    _identity = true;
                    
                    switch(column.DataType.Name)
                    {
                        case "Int16":
                            _identityType = "smallint";
                            break;
                            
                        case "SByte":
                            _identityType = "tinyint";
                            break;
                            
                        case "Int64":
                            _identityType = "bigint";
                            break;
                            
                        case "Decimal":
                            _identityType = "decimal";
                            break;
                            
                        default:
                            _identityType = "int";
                            break;
                    }
                }
                else
                {
                    if(_first)
                        _first = false;
                    else
                    {
                        _builder.Append(", ");
                        _joinSql.Append(", ");
                    }
                    
                    _builder.Append(column.ColumnName);
                    _joinSql.Append("@");
                    _joinSql.Append(column.ColumnName);
                }
            }
            
            _builder.Append(") ");
            _builder.Append(_joinSql.ToString());
            _builder.Append(")");
            
            if(_identity)
            {
                _builder.Append("; SELECT CAST(scope_identity() AS ");
                _builder.Append(_identityType);
                _builder.Append(")");
            }
            
            return _builder.ToString();
        }
        
        private SQLiteCommand CreateInsertCommand(DataRow row)
        {
            DataTable _table = row.Table;
            string _sql = BuildInsertSQL(_table);
            SQLiteCommand _command = new SQLiteCommand(_sql);
            _command.CommandType = CommandType.Text;
            
            foreach(DataColumn column in _table.Columns)
            {
                if(!column.AutoIncrement)
                {
                    string parameterName = "@" + column.ColumnName;
                    InsertParameter(_command, parameterName, column.ColumnName, row[column.ColumnName]);
                }
            }
            
            return _command;
        }
        
        private void InsertParameter(SQLiteCommand command,
                                     string parameterName,
                                     string sourceColumn,
                                     object value)
        {
            SQLiteParameter _parameter = new SQLiteParameter(parameterName, value);
            _parameter.Direction = ParameterDirection.Input;
            _parameter.ParameterName = parameterName;
            _parameter.SourceColumn = sourceColumn;
            _parameter.SourceVersion = DataRowVersion.Current;
            command.Parameters.Add(_parameter);
        }
        
        #endregion Methods
    }
}