namespace YanZhiwei.DotNet2.Utilities.DataBase
{
    /// <summary>
    /// Oracle数据库分页脚本
    /// </summary>
    /// 时间：2016/10/11 15:21
    /// 备注：
    public class OraclePageScript
    {
        #region Methods

        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="keyField">主键</param>
        /// <param name="fieldStr">所有需要查询的字段(field1,field2...)</param>
        /// <param name="tableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">页宽</param>
        /// <returns>SQL语句</returns>
        public string JoinPageSQL(string keyField, string fieldStr, string tableName, string where, string order, int currentPage, int pageSize)
        {
            string _sql = null;

            if(currentPage == 1)
            {
                _sql = "select  " + currentPage * pageSize + " " + fieldStr + " from " + tableName + " " + where + " " + order + " ";
            }
            else
            {
                _sql = "select * from (";
                _sql += "select  " + currentPage * pageSize + " " + fieldStr + " from " + tableName + " " + where + " " + order + ") a ";
                _sql += "where " + keyField + " not in (";
                _sql += "select  " + (currentPage - 1) * pageSize + " " + keyField + " from " + tableName + " " + where + " " + order + ")";
            }

            return _sql;
        }

        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="field">字段名(非主键)</param>
        /// <param name="tableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">页宽</param>
        /// <returns>SQL语句</returns>
        public string JoinPageSQL(string field, string tableName, string where, string order, int currentPage, int pageSize)
        {
            string _sql = null;

            if(currentPage == 1)
            {
                _sql = "select rownum " + currentPage * pageSize + " " + field + " from " + tableName + " " + where + " " + order + " group by " + field;
            }
            else
            {
                _sql = "select * from (";
                _sql += "select rownum " + currentPage * pageSize + " " + field + " from " + tableName + " " + where + " " + order + " group by " + field + " ) a ";
                _sql += "where " + field + " not in (";
                _sql += "select rownum " + (currentPage - 1) * pageSize + " " + field + " from " + tableName + " " + where + " " + order + " group by " + field + ")";
            }

            return _sql;
        }

        #endregion Methods
    }
}