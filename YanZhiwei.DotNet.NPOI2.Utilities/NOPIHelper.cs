using NPOI.SS.UserModel;

namespace YanZhiwei.DotNet.NPOI2.Utilities
{
    /// <summary>
    /// NOPI 操作辅助类
    /// </summary>
    /// 时间：2016/10/17 10:47
    /// 备注：
    public static class NOPIHelper
    {
        /// <summary>
        /// 样式创建
        /// eg:
        ///private ICellStyle CreateCellStly(HSSFWorkbook _excel)
        ///{
        ///    IFont _font = _excel.CreateFont();
        ///    _font.FontHeightInPoints = 11;
        ///    _font.FontName = "宋体";
        ///    _font.Boldweight = (short)FontBoldWeight.Bold;
        ///    ICellStyle _cellStyle = _excel.CreateCellStyle();
        ///    //_cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
        ///    //_cellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
        ///    _cellStyle.SetFont(_font);
        ///    return _cellStyle;
        ///}
        /// 为行设置样式
        /// </summary>
        /// <param name="row">IRow</param>
        /// <param name="cellStyle">ICellStyle</param>
        public static void SetRowStyle(this IRow row, ICellStyle cellStyle)
        {
            if(row != null && cellStyle != null)
            {
                for(int u = row.FirstCellNum; u < row.LastCellNum; u++)
                {
                    ICell _cell = row.GetCell(u);
                    
                    if(_cell != null)
                        _cell.CellStyle = cellStyle;
                }
            }
        }
        
        /// <summary>
        /// 当value大于零的时候才插入值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static void SetCellValueOnlyThanZero(this ICell cell, int value)
        {
            if(cell == null) return;
            
            if(value > 0)
            {
                cell.SetCellValue(value);
            }
        }
        
        /// <summary>
        /// 当value大于零的时候才插入值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static void SetCellValueOnlyThanZero(this ICell cell, double value)
        {
            if(cell == null) return;
            
            if(value > 0)
            {
                cell.SetCellValue(value);
            }
        }
    }
}