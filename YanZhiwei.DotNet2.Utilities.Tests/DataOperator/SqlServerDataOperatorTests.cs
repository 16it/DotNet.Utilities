using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Collection;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.Utilities.DataOperator.Tests
{
    [TestClass()]
    public class SqlServerDataOperatorTests
    {
        private SqlServerDataOperator SqlHelper = null;
        
        [TestInitialize]
        public void InitConnection()
        {
            string _sqlConnectString = @"server=YANZHIWEI-PC\SQLEXPRESS;database=AdventureWorks2014;uid=sa;pwd=sasa;";
            SqlHelper = new SqlServerDataOperator(_sqlConnectString);
        }
        
        [TestMethod()]
        public void ExecutePageQueryTest()
        {
            PagedList<Product> _pageResult = SqlHelper.ExecutePageQuery<Product>("[Production].[Product]", "*", "Name", OrderType.Asc, string.Empty, 10, 1);
            Assert.AreEqual(10, _pageResult.Count);
        }
    }
}