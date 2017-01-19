using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.SS.UserModel;
using System.IO;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.NPOI2.Utilities.Tests
{
    [TestClass()]
    public class NOPIHelperTests
    {
        private ICellStyle CreateCellStly(IWorkbook _excel)
        
        {
            IFont _font = _excel.CreateFont();
            _font.FontHeightInPoints = 10;
            _font.FontName = "宋体";
            _font.Boldweight = (short)FontBoldWeight.Bold;
            ICellStyle _cellStyle = _excel.CreateCellStyle();
            _cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
            _cellStyle.FillPattern = FillPattern.SolidForeground;
            _cellStyle.BorderBottom = BorderStyle.Thin;
            _cellStyle.BorderLeft = BorderStyle.Thin;
            _cellStyle.BorderRight = BorderStyle.Thin;
            _cellStyle.BorderTop = BorderStyle.Thin;
            _cellStyle.VerticalAlignment = VerticalAlignment.Center;
            _cellStyle.Alignment = HorizontalAlignment.Center;
            _cellStyle.SetFont(_font);
            return _cellStyle;
        }
        
        [TestMethod()]
        public void SetRowStyleTest()
        {
            string _path = string.Format(@"{0}NOPI.xlsx", UnitTestHelper.GetExecuteDirectory());
            IWorkbook _workbook = NOPIHelper.GetExcelWorkbook(_path);
            
            using(FileStream file = new FileStream(_path, FileMode.Create))
            {
                IRow _row = _workbook.GetSheetAt(0).GetRow(1);
                NOPIHelper.SetRowStyle(_row, CreateCellStly(_workbook));
                _workbook.Write(file);
            }
        }
    }
}