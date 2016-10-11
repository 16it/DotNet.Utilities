namespace YanZhiwei.DotNet.NPOI2.Utilities
{
    using System.Data;
    using System.IO;
    
    using DotNet2.Utilities.Operator;
    
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    
    /// <summary>
    /// NPOIExcel 操作辅助类
    /// </summary>
    /// 时间：2016/9/9 11:49
    /// 备注：
    public class NPOIExcel
    {
        #region Methods
        
        /// <summary>
        /// 将EXCEL文件导入到DataTable
        /// </summary>
        /// <param name="filePath">EXCEL路径</param>
        /// <param name="sheetIndex">Sheet索引</param>
        /// <param name="headIndex">列索引</param>
        /// <param name="rowIndex">行起始索引</param>
        /// <returns></returns>
        /// 时间：2016/10/11 17:07
        /// 备注：
        public static DataTable ToDataTable(string filePath, ushort sheetIndex, ushort headIndex, ushort rowIndex)
        {
            ValidateOperator.Begin().NotNull(filePath, "需要导入到EXCEL文件路径").IsFilePath(filePath).CheckFileExists(filePath);
            DataTable _table = new DataTable();
            IWorkbook _hssfworkbook;
            using(FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _hssfworkbook = WorkbookFactory.Create(file);
            }
            ISheet _sheet = _hssfworkbook.GetSheetAt(sheetIndex);
            AddDataColumns(_sheet, headIndex, _table);
            bool _supportFormula = string.Compare(Path.GetExtension(filePath), ".xlsx", true) == 0;
            
            for(int i = (_sheet.FirstRowNum + rowIndex); i <= _sheet.LastRowNum; i++)
            {
                IRow _row = _sheet.GetRow(i);
                bool _emptyRow = true;
                
                if(_row == null) continue;
                
                object[] _itemArray = new object[_row.LastCellNum];
                
                for(int j = _row.FirstCellNum; j < _row.LastCellNum; j++)
                {
                    if(_row.GetCell(j) == null) continue;
                    
                    switch(_row.GetCell(j).CellType)
                    {
                        case CellType.Numeric:
                            if(DateUtil.IsCellDateFormatted(_row.GetCell(j)))       //日期类型
                            {
                                _itemArray[j] = _row.GetCell(j).DateCellValue;
                            }
                            else//其他数字类型
                            {
                                _itemArray[j] = _row.GetCell(j).NumericCellValue;
                            }
                            
                            break;
                            
                        case CellType.Blank:
                            _itemArray[j] = string.Empty;
                            break;
                            
                        case CellType.Formula:
                            IFormulaEvaluator _eva = null;
                            
                            if(_supportFormula)
                            {
                                _eva = new XSSFFormulaEvaluator(_hssfworkbook);
                            }
                            else
                            {
                                _eva = new HSSFFormulaEvaluator(_hssfworkbook);
                            }
                            
                            if(_eva.Evaluate(_row.GetCell(j)).CellType == CellType.Numeric)
                            {
                                _itemArray[j] = _eva.Evaluate(_row.GetCell(j)).NumberValue;
                            }
                            else
                            {
                                _itemArray[j] = _eva.Evaluate(_row.GetCell(j)).StringValue;
                            }
                            
                            break;
                            
                        default:
                            _itemArray[j] = _row.GetCell(j).StringCellValue;
                            break;
                    }
                    
                    if(_itemArray[j] != null && !string.IsNullOrEmpty(_itemArray[j].ToString().Trim()))
                    {
                        _emptyRow = false;
                    }
                }
                
                if(!_emptyRow)
                {
                    _table.Rows.Add(_itemArray);
                }
            }
            
            return _table;
        }
        
        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool ToExcel(DataTable table, string sheetName, string title, string filePath)
        {
            ValidateOperator.Begin().NotNull(table, "需要导出到EXCEL数据源")
            .NotNullOrEmpty(title, "EXCEL标题")
            .NotNullOrEmpty(filePath, "EXCEL导出路径")
            .IsFilePath(filePath);
            using(FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                IWorkbook _workBook = new HSSFWorkbook();
                sheetName = string.IsNullOrEmpty(sheetName) == true ? "sheet1" : sheetName;
                ISheet _sheet = _workBook.CreateSheet(sheetName);
                //处理表格标题
                IRow _row = _sheet.CreateRow(0);
                _row.CreateCell(0).SetCellValue(title);
                _sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                _row.Height = 500;
                ICellStyle _cellStyle = _workBook.CreateCellStyle();
                IFont _font = _workBook.CreateFont();
                _font.FontName = "微软雅黑";
                _font.FontHeightInPoints = 17;
                _cellStyle.SetFont(_font);
                _cellStyle.VerticalAlignment = VerticalAlignment.Center;
                _cellStyle.Alignment = HorizontalAlignment.Center;
                _row.Cells[0].CellStyle = _cellStyle;
                //处理表格列头
                _row = _sheet.CreateRow(1);
                
                for(int i = 0; i < table.Columns.Count; i++)
                {
                    _row.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                    _row.Height = 350;
                    _sheet.AutoSizeColumn(i);
                }
                
                //处理数据内容
                for(int i = 0; i < table.Rows.Count; i++)
                {
                    _row = _sheet.CreateRow(2 + i);
                    _row.Height = 250;
                    
                    for(int j = 0; j < table.Columns.Count; j++)
                    {
                        _row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                        _sheet.SetColumnWidth(j, 256 * 15);
                    }
                }
                
                //写入数据流
                _workBook.Write(fileStream);
                return true;
            }
        }
        
        private static void AddDataColumns(ISheet sheet, ushort headIndex, DataTable table)
        {
            IRow _headRow = sheet.GetRow(headIndex);
            
            if(_headRow != null)
            {
                int _colCount = _headRow.LastCellNum;
                
                for(int i = 0; i < _colCount; i++)
                {
                    table.Columns.Add(_headRow.GetCell(i).StringCellValue);
                }
            }
        }
        
        #endregion Methods
    }
}