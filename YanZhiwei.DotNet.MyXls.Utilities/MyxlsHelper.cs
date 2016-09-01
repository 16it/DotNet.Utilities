﻿namespace YanZhiwei.DotNet.MyXls.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    using DotNet2.Utilities.Common;
    using DotNet2.Utilities.Operator;

    using org.in2bits.MyXls;

    /// <summary>
    /// MyXls帮助类
    /// </summary>
    /// 时间：2015-12-08 10:40
    /// 备注：
    public static class MyxlsHelper
    {
        #region Methods

        /// <summary>
        /// 遍历excel数据
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="sheetIndex">Worksheets『从0开始』</param>
        /// <param name="startRowIndex">遍历起始行『从0开始』</param>
        /// <param name="startColIndex">遍历起始列『从0开始』</param>
        /// <param name="foreachRowFactory">遍历规则『委托』</param>
        public static void ForeachExcel(string excelPath, int sheetIndex, ushort startRowIndex, ushort startColIndex, Action<string, int, int, object> foreachRowFactory)
        {
            CheckedExcelFileParamter(excelPath, true);
            XlsDocument _excelDoc = new XlsDocument(excelPath);
            Worksheet _sheet = _excelDoc.Workbook.Worksheets[sheetIndex];
            int _colCount = _sheet.Rows[startRowIndex].CellCount;
            int _rowCount = _sheet.Rows.Count;

            for(ushort i = startRowIndex; i < _rowCount; i++)
            {
                for(ushort j = startColIndex; j <= _colCount; j++)
                {
                    string _colName = _sheet.Rows[i].GetCell(j).Value.ToString();
                    object _value = _sheet.Rows[i].GetCell(j).Value;
                    foreachRowFactory(_colName, j, i, _value);
                }
            }
        }

        /// <summary>
        /// 遍历Excel 数据行
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="sheetIndex">Worksheets『从0开始』</param>
        /// <param name="startRowIndex">遍历起始行『从0开始』</param>
        /// <param name="foreachRowFactory">遍历规则『委托』</param>
        /// 时间：2015-12-08 10:46
        /// 备注：
        public static void ForeachExcel(string excelPath, int sheetIndex, ushort startRowIndex, Action<Row> foreachRowFactory)
        {
            CheckedExcelFileParamter(excelPath, true);
            XlsDocument _excelDoc = new XlsDocument(excelPath);
            Worksheet _sheet = _excelDoc.Workbook.Worksheets[sheetIndex];
            int _colCount = _sheet.Rows[startRowIndex].CellCount;
            int _rowCount = _sheet.Rows.Count;

            for(ushort i = startRowIndex; i < _rowCount; i++)
            {
                Row _rows = _sheet.Rows[i];
                foreachRowFactory(_rows);
            }
        }

        /// <summary>
        /// 遍历Excel 数据行
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="startRowIndex">遍历起始行『从0开始』</param>
        /// <param name="foreachRowFactory">遍历规则『委托』</param>
        /// 时间：2015-12-08 10:46
        /// 备注：
        public static void ForeachExcel(string excelPath, string sheetName, ushort startRowIndex, Action<Row> foreachRowFactory)
        {
            CheckedExcelFileParamter(excelPath, true);
            XlsDocument _excelDoc = new XlsDocument(excelPath);
            Worksheet _sheet = _excelDoc.Workbook.Worksheets[sheetName];
            int _colCount = _sheet.Rows[startRowIndex].CellCount;
            int _rowCount = _sheet.Rows.Count;

            for(ushort i = startRowIndex; i < _rowCount; i++)
            {
                Row _rows = _sheet.Rows[i];
                foreachRowFactory(_rows);
            }
        }

        /// <summary>
        /// 将EXCEL导出到DataTable
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="sheetIndex">Worksheets『从0开始』</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable(string excelPath, int sheetIndex)
        {
            CheckedExcelFileParamter(excelPath, true);
            XlsDocument _excelDoc = new XlsDocument(excelPath);
            DataTable _table = new DataTable();
            Worksheet _sheet = _excelDoc.Workbook.Worksheets[sheetIndex];
            ushort _colCount = _sheet.Rows[1].CellCount;
            ushort _rowCount = (ushort)_sheet.Rows.Count;

            for(ushort j = 1; j <= _colCount; j++)
            {
                _table.Columns.Add(new DataColumn(j.ToString()));
            }

            for(ushort i = 1; i <= _rowCount; i++)
            {
                DataRow _row = _table.NewRow();

                for(ushort j = 1; j <= _colCount; j++)
                {
                    _row[j - 1] = _sheet.Rows[i].GetCell(j).Value;
                }

                _table.Rows.Add(_row);
            }

            return _table;
        }

        /// <summary>
        /// 将集合导出到excel
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="exceldbSource">数据源</param>
        /// <param name="excelPath">保存路径</param>
        public static void ToExecel<T>(IEnumerable<T> source, string excelPath, string sheetName)
        where T : class
        {
            CheckedExcelFileParamter(excelPath, false);
            CheckedExcelExportParamter(source, sheetName);

            int _recordCnt = source.Count();
            XlsDocument _xls = new XlsDocument();
            string _savePath = FileHelper.GetExceptName(excelPath);
            _xls.FileName = FileHelper.GetFileName(excelPath);

            XF _columnStyle = SetColumnStyle(_xls);

            Worksheet _sheet = _xls.Workbook.Worksheets.Add(sheetName);
            int _celIndex = 0, _rowIndex = 1;
            Cells _cells = _sheet.Cells;
            IDictionary<string, string> _fields = ReflectHelper.GetPropertyName<T>();
            string[] _colNames = new string[_fields.Count];
            _fields.Values.CopyTo(_colNames, 0);

            foreach(string col in _colNames)
            {
                _celIndex++;
                _cells.Add(1, _celIndex, col, _columnStyle);
            }

            foreach(T item in source)
            {
                _rowIndex++;
                _celIndex = 0;

                foreach(KeyValuePair<string, string> proItem in _fields)
                {
                    _celIndex++;
                    object _provalue = typeof(T).InvokeMember(proItem.Key, BindingFlags.GetProperty, null, item, null);
                    XF _cellStyle = SetCellStyle(_xls, _provalue.GetType());
                    _cells.Add(_rowIndex, _celIndex, _provalue.ToStringOrDefault(string.Empty), _cellStyle);
                }
            }

            _xls.Save(_savePath, true);
        }

        /// <summary>
        /// 将DataTable导出到Excel
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="excelPath">保存路径</param>
        /// <param name="sheetName">Sheet名字</param>
        /// 时间：2015-12-08 11:13
        /// 备注：
        public static void ToExecel(DataTable table, string excelPath, string sheetName)
        {
            CheckedExcelFileParamter(excelPath, false);
            CheckedExcelExportParamter(table, sheetName);
            int _recordCnt = table.Rows.Count;
            XlsDocument _xls = new XlsDocument();
            string _savePath = excelPath.Substring(0, excelPath.LastIndexOf(@"\") + 1);
            _xls.FileName = FileHelper.GetFileName(excelPath);
            XF _columnStyle = SetColumnStyle(_xls);
            Worksheet _sheet = _xls.Workbook.Worksheets.Add(sheetName);
            int _celIndex = 1, _rowIndex = 2;
            Cells _cells = _sheet.Cells;

            foreach(DataColumn column in table.Columns)
            {
                _cells.Add(1, _celIndex, column.ColumnName, _columnStyle);
                _celIndex++;
            }

            _celIndex = 1;

            foreach(DataRow t in table.Rows)
            {
                foreach(DataColumn column in table.Columns)
                {
                    XF _cellStyle = SetCellStyle(_xls, column.DataType);
                    Cell _cell = _cells.Add(_rowIndex, _celIndex, t[column.ColumnName], _cellStyle);
                    _celIndex++;
                }

                _celIndex = 1;
                _rowIndex++;
            }

            _xls.Save(_savePath, true);
        }

        private static void CheckedExcelExportParamter<T>(IEnumerable<T> source, string sheetName)
        where T : class
        {
            ValidateOperator.Begin().NotNull(source, "需要导出到EXCEL数据集合").NotNullOrEmpty(sheetName, "sheetName");
        }

        private static void CheckedExcelExportParamter(DataTable source, string sheetName)
        {
            ValidateOperator.Begin().NotNull(source, "需要导出到EXCEL的数据表").NotNullOrEmpty(sheetName, "sheetName");
        }

        private static void CheckedExcelFileParamter(string excelPath, bool checkedExist)
        {
            if(checkedExist)
                ValidateOperator.Begin().NotNullOrEmpty(excelPath, "EXCEL路径").IsFilePath(excelPath, "EXCEL路径").CheckFileExists(excelPath, "EXCEL文件");
            else
                ValidateOperator.Begin().NotNullOrEmpty(excelPath, "EXCEL路径").IsFilePath(excelPath, "EXCEL路径");
        }

        private static XF SetCellStyle(XlsDocument xls, Type dataType)
        {
            XF _cellStyle = xls.NewXF();
            _cellStyle.HorizontalAlignment = HorizontalAlignments.Centered;
            _cellStyle.VerticalAlignment = VerticalAlignments.Centered;
            _cellStyle.UseBorder = true;
            _cellStyle.LeftLineStyle = 1;
            _cellStyle.LeftLineColor = Colors.Black;
            _cellStyle.BottomLineStyle = 1;
            _cellStyle.BottomLineColor = Colors.Black;
            _cellStyle.UseProtection = false; // 默认的就是受保护的，导出后需要启用编辑才可修改
            _cellStyle.TextWrapRight = true; // 自动换行
            _cellStyle.Format = TransCellType(dataType);
            return _cellStyle;
        }

        /// <summary>
        /// 设置列样式
        /// </summary>
        /// <param name="xls">XlsDocument</param>
        /// <returns>XF</returns>
        /// 时间：2015-12-08 10:03
        /// 备注：
        private static XF SetColumnStyle(XlsDocument xls)
        {
            XF _columnStyle = xls.NewXF();
            _columnStyle.HorizontalAlignment = HorizontalAlignments.Centered;
            _columnStyle.VerticalAlignment = VerticalAlignments.Centered;
            _columnStyle.UseBorder = true;
            _columnStyle.TopLineStyle = 1;
            _columnStyle.TopLineColor = Colors.Grey;
            _columnStyle.BottomLineStyle = 1;
            _columnStyle.BottomLineColor = Colors.Grey;
            _columnStyle.LeftLineStyle = 1;
            _columnStyle.LeftLineColor = Colors.Grey;
            _columnStyle.Pattern = 1; // 单元格填充风格。如果设定为0，则是纯色填充(无色)，1代表没有间隙的实色
            _columnStyle.PatternBackgroundColor = Colors.White;
            _columnStyle.PatternColor = Colors.Silver;
            _columnStyle.Font.Bold = true;
            _columnStyle.Font.Height = 12 * 20;
            return _columnStyle;
        }

        private static string TransCellType(Type dataType)
        {
            if(dataType == typeof(DateTime))
                return StandardFormats.Date_Time;
            else
                return StandardFormats.Text;
        }

        #endregion Methods
    }
}