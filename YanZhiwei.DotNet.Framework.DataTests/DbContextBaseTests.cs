using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using YanZhiwei.DotNet.Framework.DataTests;
using YanZhiwei.DotNet2.Utilities.Collection;

namespace YanZhiwei.DotNet.Framework.Data.Tests
{
    [TestClass()]
    public class DbContextBaseTests
    {
        [TestMethod()]
        public void DeleteTest()
        {
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                Address _fined = dbContext.Find<Address>(1);
                dbContext.Delete<Address>(_fined);
                _fined = dbContext.Find<Address>(1);
                Assert.IsNull(_fined);
            }
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
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                List<Address> _finedAll = dbContext.FindAll<Address>();
                Assert.IsNotNull(_finedAll);
            }
        }
        
        [TestMethod()]
        public void FindAllByPageTest()
        {
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                PagedList<Address> _finedAll = dbContext.FindAllByPage<Address, int>(null, c => c.ID, 10, 1);
                Assert.AreEqual(_finedAll.Count, 10);
            }
        }
        
        [TestMethod()]
        public void InsertTest()
        {
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                Address _item = new Address();
                _item.ID = 1;
                _item.PostalCode = "78011";
                _item.AddressLine1 = "Tianxin";
                _item.City = "Zhuzhou";
                _item.StateProvinceID = 79;
                _item.PostalCode = "72222";
                _item.rowguid = Guid.NewGuid();
                Address _actual = dbContext.Insert<Address>(_item);
                Assert.IsNotNull(_actual);
            }
        }
        
        [TestMethod()]
        public void SqlQueryTest()
        {
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                string _sql = @"SELECT  [AddressID] as ID
                                       ,[AddressLine1]
                                       ,[AddressLine2]
                                       ,[City]
                                       ,[StateProvinceID]
                                       ,[PostalCode]
                                       ,[rowguid]
                                       ,[ModifiedDate] as CreateTime
                                       FROM[Person].[Address]
                                       where AddressID = @AddressID";
                DbParameter _paramter = new SqlParameter("@AddressID", 32529);
                Address _actual = dbContext.SqlQuery<Address>(_sql, _paramter).FirstOrDefault();
                Assert.IsNotNull(_actual);
            }
        }
        
        [TestMethod()]
        public void UpdateTest()
        {
            using(var dbContext = new AdventureWorks2014DbContext())
            {
                Address _actual = dbContext.Find<Address>(32529);
                string _expect = _actual.AddressLine1 + DateTime.Now.ToString("yyyyMMddHHmmss");
                _actual.AddressLine1 = _expect;
                dbContext.Update<Address>(_actual);
                _actual = dbContext.Find<Address>(32529);
                Assert.AreEqual(_actual.AddressLine1, _expect);
            }
        }
    }
}