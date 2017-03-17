using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Framework.DataTests;

namespace YanZhiwei.DotNet.Framework.Data.Tests
{
    [TestClass()]
    public class DbContextBaseTests
    {
        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void FindTest()
        {
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                Address _actual = dbContext.Find<Address>(1);
                Assert.IsNotNull(_actual);
                Assert.AreEqual(_actual.ID, 1);
            }
        }
        
        [TestMethod()]
        public void FindAllTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void FindAllByPageTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void InsertTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SaveChangesTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SqlQueryTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }
    }
}