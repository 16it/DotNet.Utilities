namespace YanZhiwei.DotNet2.Utilities.Common
{
    using Interfaces.DataAccess;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;

    /// <summary>
    /// Access 帮助类
    /// </summary>
    public sealed class AccessHelper : ISQLHelper
    {
        #region Fields

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connectString = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"> access路径 </param>
        public AccessHelper(string path)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">access路径</param>
        /// <param name="password">access密码</param>
        public AccessHelper(string path, string password)
        {
            connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Jet OLEDB:Database Password= " + password;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="sql">读取sql语句</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql, DbParameter[] parameters)
        {
            using(OleDbConnection sqlcon = new OleDbConnection(this.connectString))
            {
                using(OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    if(parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using(OleDbDataAdapter sqldap = new OleDbDataAdapter(sqlcmd))
                    {
                        DataTable _table = new DataTable();
                        sqldap.Fill(_table);
                        return _table;
                    }
                }
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">查询，修改，删除sql语句</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>操作影响行数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] parameters)
        {
            int _affectedRows = -1;
            using(OleDbConnection sqlcon = new OleDbConnection(this.connectString))
            {
                sqlcon.Open();
                using(OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    if(parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    _affectedRows = sqlcmd.ExecuteNonQuery();
                }
            }
            return _affectedRows;
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql">读取sql语句</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            OleDbConnection sqlcon = new OleDbConnection(this.connectString);
            using(OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
            {
                if(parameters != null)
                {
                    sqlcmd.Parameters.AddRange(parameters);
                }

                sqlcon.Open();
                return sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">查询第一行第一列数据值</param>
        /// <param name="parameters">OleDbParameter参数；eg: new OleDbParameter("@categoryName","Test2")</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            using(OleDbConnection sqlcon = new OleDbConnection(this.connectString))
            {
                using(OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    if(parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    sqlcon.Open();
                    return sqlcmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// ExecuteReader 存储过程
        /// </summary>
        /// <param name="proName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>IDataReader</returns>
        public IDataReader StoreExecuteDataReader(string proName, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}