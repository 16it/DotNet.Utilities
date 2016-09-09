namespace YanZhiwei.DotNet.MyXls.Utilities
{
    using org.in2bits.MyXls;
    using System;
    
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
            CheckedHanlder.CheckedExcelFileParamter(excelPath, true);
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
            CheckedHanlder.CheckedExcelFileParamter(excelPath, true);
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
            CheckedHanlder.CheckedExcelFileParamter(excelPath, true);
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
        
        #endregion Methods
    }
}