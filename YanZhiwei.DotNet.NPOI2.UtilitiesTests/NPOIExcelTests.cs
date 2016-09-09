using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using YanZhiwei.DotNet2.Utilities.DataOperator;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet.NPOI2.Utilities.Tests
{
    [TestClass()]
    public class NPOIExcelTests
    {
        private SqlServerDataOperator sqlHelper = null;
        
        [TestInitialize()]
        public void Init()
        {
            sqlHelper = new SqlServerDataOperator(@"server=YANZHIWEI-IT-PC\SQLEXPRESS;database=Northwind;uid=sa;pwd=sasa;");
        }
        
        [TestMethod()]
        public void ToExcelTest()
        {
            string _xlsPath = string.Format(@"D:\Employees_{0}.xls", DateTime.Now.FormatDate(12));
            DataTable _queryResult = sqlHelper.ExecuteDataTable(@"SELECT * FROM [Employees]", null);
            NPOIExcel.ToExcel(_queryResult, "Hello", "Employees", _xlsPath);
            bool _actual = File.Exists(_xlsPath);
            Assert.IsTrue(_actual);
        }
    }
}