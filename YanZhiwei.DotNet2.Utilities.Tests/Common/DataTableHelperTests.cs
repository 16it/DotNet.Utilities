using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace YanZhiwei.DotNet2.Utilities.Common.Tests
{
    [TestClass()]
    public class DataTableHelperTests
    {
        [TestMethod()]
        public void CheckedColumnsNameTest()
        {
            DataTable _testTable = DataTableHelper.CreateTable("姓名,单位,备注");
            bool _actual = _testTable.CheckedColumnsName(new string[2] { "姓名", "备注" });
            Assert.IsTrue(_actual);
        }
    }
}