namespace YanZhiwei.DotNet2.Utilities.Common
{
    using System;
    using System.Data;
    
    /// <summary>
    /// DataTable 帮助类
    /// </summary>
    /// 时间：2016-01-05 13:13
    /// 备注：
    public static class DataTableHelper
    {
        #region Methods
        
        /// <summary>
        /// 判断DataTable是否是NULL或者Row行数等于零
        /// </summary>
        /// <param name="datatable">DataTable</param>
        /// <returns>否是NULL或者Row行数等于零</returns>
        /// 时间：2016-01-05 13:15
        /// 备注：
        public static bool IsNullOrEmpty(this DataTable datatable)
        {
            return datatable == null || datatable.Rows.Count == 0;
        }
        
        /// <summary>
        /// 创建Datatable，规范：列名|列类型,列名|列类型,列名|列类型
        /// <para>举例：CustomeName|string,Gender|bool,Address</para>
        /// </summary>
        /// <param name="columnsInfo">创建表的字符串规则信息</param>
        /// <returns>DataTable</returns>
        public static DataTable CreateTable(string columnsInfo)
        {
            DataTable _dtNew = new DataTable();
            string[] _columnsList = columnsInfo.Split(',');
            string _columnName;
            string _columnType;
            string[] _singleColumnInfo;
            
            foreach(string s in _columnsList)
            {
                _singleColumnInfo = s.Split('|');
                _columnName = _singleColumnInfo[0];
                
                if(_singleColumnInfo.Length == 2)
                {
                    _columnType = _singleColumnInfo[1];
                    _dtNew.Columns.Add(new DataColumn(_columnName, Type.GetType(TransColumnType(_columnType))));
                }
                else
                {
                    _dtNew.Columns.Add(new DataColumn(_columnName));
                }
            }
            
            return _dtNew;
        }
        
        /// <summary>
        /// 检查特定列名称是否存在
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="columnsName">需要存在的列名称</param>
        /// <returns>是否全部包含</returns>
        public static bool CheckedColumnsName(this DataTable table, string[] columnsName)
        {
            bool _result = table != null;
            
            if(table != null)
            {
                foreach(string item in columnsName)
                {
                    if(!table.Columns.Contains(item))
                    {
                        _result = false;
                        break;
                    }
                }
            }
            
            return _result;
        }
        /// <summary>
        /// 转义数据类型
        /// </summary>
        /// <param name="columnType">列类型</param>
        /// <returns>转义后实际数据类型</returns>
        private static string TransColumnType(string columnType)
        {
            string _currentType = string.Empty;
            
            switch(columnType.ToLower())
            {
                case "int":
                    _currentType = "System.Int32";
                    break;
                    
                case "string":
                    _currentType = "System.String";
                    break;
                    
                case "decimal":
                    _currentType = "System.Decimal";
                    break;
                    
                case "double":
                    _currentType = "System.Double";
                    break;
                    
                case "dateTime":
                    _currentType = "System.DateTime";
                    break;
                    
                case "bool":
                    _currentType = "System.Boolean";
                    break;
                    
                case "image":
                    _currentType = "System.Byte[]";
                    break;
                    
                case "object":
                    _currentType = "System.Object";
                    break;
                    
                default:
                    _currentType = "System.String";
                    break;
            }
            
            return _currentType;
        }
        
        #endregion Methods
    }
}