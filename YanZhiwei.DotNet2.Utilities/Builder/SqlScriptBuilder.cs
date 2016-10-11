namespace YanZhiwei.DotNet2.Utilities.Builder
{
    /// <summary>
    /// SQL脚本构建
    /// </summary>
    /// 时间：2016/10/11 16:22
    /// 备注：
    public class SqlScriptBuilder
    {
        #region Methods
        
        /// <summary>
        /// 查询记录总数
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="tableName">表名</param>
        /// <returns>Sql</returns>
        /// 时间：2016-01-05 13:11
        /// 备注：
        public static string JoinQueryTotalSql(string sql, string tableName)
        {
            string _sqltotalCount = string.Format("select count(*) from {0}", tableName);
            return string.Format("{0};{1}", sql, _sqltotalCount);
        }
        
        /// <summary>
        /// 添加筛选条件
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="sqlWhere">筛选条件</param>
        /// <returns>sql</returns>
        /// 时间：2016-01-06 15:19
        /// 备注：
        public static string JoinQueryWhereSql(string sql, string sqlWhere)
        {
            if(!string.IsNullOrEmpty(sqlWhere))
                sql = string.Format("{0} and ( {1} )", sql, sqlWhere);
                
            return sql;
        }
        
        #endregion Methods
    }
}