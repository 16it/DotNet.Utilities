namespace YanZhiwei.DotNet2.Utilities.DataBase
{
    using System;
    using System.Text;

    using Builder;

    using Enum;

    /// <summary>
    /// Sql Server数据库分页脚本
    /// </summary>
    /// 创建时间:2015-05-22 11:48
    /// 备注说明:<c>null</c>
    public class SqlServerPageScript
    {
        #region Methods

        /// <summary>
        /// 多表分页
        /// </summary>
        /// <param name="sql">条件查询语句,删除Order By条件</param>
        /// <param name="orderByColumn">排序字段</param>
        /// <param name="orderType">排序方式，降序或升序</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>完整分页语句</returns>
        public static string MultiTablePageByRowNumber(string sql, string orderByColumn, OrderType orderType, int pageIndex, int pageSize)
        {
            string _sqlFullWhere = sql.Trim();
            string _sqlTotalCount = _sqlFullWhere;
            int _pageStart = pageSize * (pageIndex - 1) + 1;
            int _pageEnd = pageSize * pageIndex + 1;
            StringBuilder _builder = new StringBuilder();
            _builder.Append("SELECT * FROM ( ");
            int _selectIndex = _sqlFullWhere.IndexOf("Select", 0, StringComparison.OrdinalIgnoreCase);
            if (_selectIndex >= 0)
            {
                _sqlFullWhere = _sqlFullWhere.Insert(6, string.Format(" ROW_NUMBER() OVER(ORDER BY {0} {1}) AS ROW_NUMBER, ", orderByColumn, orderType));
                _builder.Append(_sqlFullWhere);
            }
            _builder.Append(" ) tp");
            _builder.AppendFormat(" WHERE tp.ROW_NUMBER >= {0}", _pageStart);
            _builder.AppendFormat(" AND tp.ROW_NUMBER < {0};", _pageEnd);

            if (_selectIndex >= 0)
            {
                int _whereIndex = _sqlTotalCount.IndexOf("from", _selectIndex, StringComparison.OrdinalIgnoreCase);
                if (_whereIndex >= 0)
                {
                    _sqlTotalCount = _sqlTotalCount.Remove(6, _whereIndex - 6);
                    _sqlTotalCount = _sqlTotalCount.Insert(6, " Count(*) ");
                }
                _builder.AppendFormat(" {0}{1}", Environment.NewLine, _sqlTotalCount);
            }

            return _builder.ToString();
        }

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
        public static string TablePageSQLByRowNumber(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            int _pageStart = pSize * (pIndex - 1) + 1;
            int _pageEnd = pSize * pIndex + 1;
            string _sql = string.Format("select * from  (select (ROW_NUMBER() over(order by {2} {3})) as ROWNUMBER,{1}  from {0} {6})as tp where ROWNUMBER >= {4} and ROWNUMBER< {5} ",
                                        tableName,
                                        columns,
                                        orderColumn,
                                        orderType == OrderType.Desc ? "desc" : "asc",
                                        _pageStart,
                                        _pageEnd,
                                        SqlScriptBuilder.JoinQueryWhereSql(sqlWhere));

            _sql += string.Format("; select count(*) from  {0} {1}",
                                  tableName,
                                  SqlScriptBuilder.JoinQueryWhereSql(sqlWhere));
            return _sql;
        }

        /// <summary>
        /// 利用[Top Max]分页，生成sql语句
        /// </summary>
        /// <param name="tableName">表名称『eg:Orders』</param>
        /// <param name="columns">需要显示列『*:所有列；或者：eg:OrderID,OrderDate,ShipName,ShipCountry』</param>
        /// <param name="orderColumn">依据排序的列『eg:OrderID』</param>
        /// <param name="sqlWhere">筛选条件『eg:Order=1』</param>
        /// <param name="orderType">升序降序『1：desc;其他:asc』</param>
        /// <param name="pSize">每页页数『需大于零』</param>
        /// <param name="pIndex">页数『从壹开始算』</param>
        /// <returns>生成分页sql脚本</returns>
        public static string TablePageSQLByTopMax(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            /*
             *eg:
             *1=>select top 30 orderID from Orders order by orderID asc
             *2=>(select max (orderID) from (select top 30 orderID from Orders order by orderID asc) as T) //查询前一页数据
             *3=> select top 15 OrderID,OrderDate,ShipName,ShipCountry from Orders where orderID>
                  ISNULL((select max (orderID) from (select top 30 orderID from Orders order by orderID asc) as T),0)
                  order by orderID asc
             */
            string _sql = string.Format("select top {4} {1} from {0} where {2}> ISNULL((select max ({2}) from (select top {5} {2} from {0} order by {2} {3}) as T),0) order by {2} {3}",
                                        tableName,
                                        columns,
                                        orderColumn,
                                        orderType == OrderType.Desc ? "desc" : "asc",
                                        pSize,
                                        (pIndex - 1) * pSize);
            _sql = SqlScriptBuilder.JoinQueryWhereSql(_sql, sqlWhere);
            _sql = SqlScriptBuilder.JoinQueryTotalSql(_sql, tableName);
            return _sql;
        }

        /// <summary>
        /// 利用[Top NotIn]分页，生成sql语句
        /// </summary>
        /// <param name="tableName">表名称『eg:Orders』</param>
        /// <param name="columns">需要显示列『*:所有列；或者：eg:OrderID,OrderDate,ShipName,ShipCountry』</param>
        /// <param name="orderColumn">依据排序的列『eg:OrderID』</param>
        /// <param name="sqlWhere">筛选条件『eg:Order=1』</param>
        /// <param name="orderType">升序降序『1：desc;其他:asc』</param>
        /// <param name="pSize">每页页数『需大于零』</param>
        /// <param name="pIndex">页数『从壹开始算』</param>
        /// <returns>生成分页sql脚本</returns>
        public static string TablePageSQLByTopNotIn(string tableName, string columns, string orderColumn, string sqlWhere, OrderType orderType, int pSize, int pIndex)
        {
            /*
             *eg:
             *1=>SELECT orderID FROM Orders ORDER BY orderID
             *2=>SELECT TOP 20 orderID FROM Orders ORDER BY orderID //查询前一页数据
             *3=> SELECT TOP 10 * FROM Orders WHERE (orderID NOT IN (SELECT TOP 20 orderID FROM Orders ORDER BY orderID)) ORDER BY orderID //在所有数据中，截去掉上一页数据(not in)，然后select top 10 即当前页数据
             */
            string _sql = string.Format("SELECT TOP {4} {1} FROM {0} WHERE ({2} NOT IN (SELECT TOP {5} {2} FROM {0} ORDER BY {2} {3})) ORDER BY {2} {3}",
                                        tableName,
                                        columns,
                                        orderColumn,
                                        orderType == OrderType.Desc ? "desc" : "asc",
                                        pSize,
                                        (pIndex - 1) * pSize);
            _sql = SqlScriptBuilder.JoinQueryWhereSql(_sql, sqlWhere);
            _sql = SqlScriptBuilder.JoinQueryTotalSql(_sql, tableName);
            return _sql;
        }

        #endregion Methods
    }
}