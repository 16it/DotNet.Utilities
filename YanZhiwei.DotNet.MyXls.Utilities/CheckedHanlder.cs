using System.Collections.Generic;
using System.Data;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet.MyXls.Utilities
{
    internal class CheckedHanlder
    {
        public static void CheckedExcelExportParamter<T>(IEnumerable<T> source, string sheetName)
        where T : class
        {
            ValidateOperator.Begin().NotNull(source, "需要导出到EXCEL数据集合").NotNullOrEmpty(sheetName, "sheetName");
        }
        
        public static void CheckedExcelExportParamter(DataTable source, string sheetName)
        {
            ValidateOperator.Begin().NotNull(source, "需要导出到EXCEL的数据表").NotNullOrEmpty(sheetName, "sheetName");
        }
        
        public static void CheckedExcelFileParamter(string excelPath, bool checkedExist)
        {
            if(checkedExist)
                ValidateOperator.Begin().NotNullOrEmpty(excelPath, "EXCEL路径").IsFilePath(excelPath).CheckFileExists(excelPath);
            else
                ValidateOperator.Begin().NotNullOrEmpty(excelPath, "EXCEL路径").IsFilePath(excelPath);
        }
    }
}