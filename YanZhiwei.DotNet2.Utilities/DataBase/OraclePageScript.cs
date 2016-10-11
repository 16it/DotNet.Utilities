using YanZhiwei.DotNet2.Utilities.Builder;
using YanZhiwei.DotNet2.Utilities.Enum;

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
        /// 利用[ROW_NUMBER() over]分页，生成sql语句
        /// </summary>
        /// <param name="tableName">表名称『eg:Orders』</param>
        /// <param name="columns">需要显示列『*:所有列；或者：eg:OrderID,OrderDate,ShipName,ShipCountry』</param>
        /// <param name="orderColumn">依据排序的列『eg:OrderID』</param>
        /// <param name="sqlWhere">筛选条件『eg:Order=1』</param>
        /// <param name="orderType">升序降序『1：desc;其他:asc』</param>
        /// <param name="pSize">每页页数『需大于零』</param>
        /// <param name="pIndex">页数『从壹开始算』</param>
        /// <returns>生成分页sql脚本</returns>
        public static string JoinPageSQLByRowNumber(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            int _pageStart = pSize * (pIndex - 1) + 1;
            int _pageEnd = pSize * pIndex + 1;
            string _sql = string.Format(@"select {1} from (SELECT A.*, ROWNUM RN FROM (SELECT * FROM {0} order by {2} {3}) A ) WHERE RN BETWEEN {4} AND {5} "
                                        , tableName
                                        , columns
                                        , orderColumn
                                        , orderType == OrderType.Desc ? "desc" : "asc"
                                        , _pageStart
                                        , _pageEnd);
            _sql = SqlScriptBuilder.JoinQueryWhereSql(_sql, sqlWhere);
            _sql = SqlScriptBuilder.JoinQueryTotalSql(_sql, tableName);
            return _sql;
        }
        
        #endregion Methods
    }
}