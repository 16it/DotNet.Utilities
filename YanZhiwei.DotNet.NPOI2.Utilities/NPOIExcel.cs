using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data;
using System.IO;
using YanZhiwei.DotNet2.Utilities.Operator;
namespace YanZhiwei.DotNet.NPOI2.Utilities
{
    /// <summary>
    /// NPOIExcel 操作辅助类
    /// </summary>
    /// 时间：2016/9/9 11:49
    /// 备注：
    public class NPOIExcel
    {
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
    }
}