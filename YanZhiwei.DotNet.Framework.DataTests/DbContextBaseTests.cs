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
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                using (System.Data.Entity.DbContextTransaction tran = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Address _fined = dbContext.Find<Address>(22);
                        dbContext.Delete<Address>(_fined);
                        _fined = dbContext.Find<Address>(22);
                        Assert.IsNull(_fined);
                        dbContext.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        [TestMethod()]
        public void FindTest()
        {
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                Address _actual = dbContext.Find<Address>(1);
                Assert.IsNotNull(_actual);
                Assert.AreEqual(_actual.ID, 1);
            }
        }

        [TestMethod()]
        public void FindAllTest()
        {
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                List<Address> _finedAll = dbContext.FindAll<Address>();
                Assert.IsNotNull(_finedAll);
            }
        }

        [TestMethod()]
        public void FindAllByPageTest()
        {
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                PagedList<Address> _finedAll = dbContext.FindAllByPage<Address, int>(null, c => c.ID, 10, 1);
                Assert.AreEqual(_finedAll.Count, 10);
            }
        }

        [TestMethod()]
        public void InsertTest()
        {
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                Address _item = new Address
                {
                    PostalCode = "78019",
                    AddressLine1 = "Tianxin",
                    City = "Changsha",
                    StateProvinceID = 78
                };
                _item.PostalCode = "72229";
                _item.rowguid = Guid.NewGuid();
                Address _actual = dbContext.Insert<Address>(_item);
                Assert.IsNotNull(_actual);
            }
        }

        [TestMethod()]
        public void SqlQueryTest()
        {
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
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
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                Address _actual = dbContext.Find<Address>(32529);
                string _expect = _actual.AddressLine1 + DateTime.Now.ToString("yyyyMMddHHmmss");
                _actual.AddressLine1 = _expect;
                dbContext.Update<Address>(_actual);
                _actual = dbContext.Find<Address>(32529);
                Assert.AreEqual(_actual.AddressLine1, _expect);
            }
        }

        [TestMethod()]
        public void CommitTest()
        {
            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                dbContext.BeginTransaction();

                try
                {
                    Address _item = new Address
                    {
                        PostalCode = "78019",
                        AddressLine1 = "Tianxin",
                        City = "Xiangtan",
                        StateProvinceID = 78
                    };
                    _item.PostalCode = "72228";
                    _item.rowguid = Guid.NewGuid();
                    dbContext.Insert<Address>(_item);
                    Address _fined = dbContext.Find<Address>(22);
                    dbContext.Delete<Address>(_fined);
                    dbContext.Commit();
                }

                catch (Exception)
                {
                    dbContext.Rollback();
                }

                finally
                {
                    Address _xiangtan = dbContext.FindAll<Address>(ent => ent.City == "Xiangtan").FirstOrDefault();
                    Assert.IsNull(_xiangtan);
                }
            }

            using (AdventureWorks2014DbContext dbContext = new AdventureWorks2014DbContext())
            {
                dbContext.BeginTransaction();

                try
                {
                    dbContext.BeginTransaction();
                    Address _jiaxing = new Address
                    {
                        PostalCode = "78019",
                        AddressLine1 = "Tianxin",
                        City = "Jiaxing",
                        StateProvinceID = 78
                    };
                    _jiaxing.PostalCode = "72228";
                    _jiaxing.rowguid = Guid.NewGuid();
                    dbContext.Insert<Address>(_jiaxing);
                    Address _hangzhou = new Address
                    {
                        PostalCode = "78019",
                        AddressLine1 = "Tianxin",
                        City = "HangZhou",
                        StateProvinceID = 78
                    };
                    _hangzhou.PostalCode = "72228";
                    _hangzhou.rowguid = Guid.NewGuid();
                    dbContext.Insert<Address>(_hangzhou);
                    dbContext.Commit();
                }

                catch (Exception)
                {
                    dbContext.Rollback();
                }

                finally
                {
                    Address _hangZhou = dbContext.FindAll<Address>(ent => ent.City == "HangZhou").FirstOrDefault();
                    Assert.IsNotNull(_hangZhou);
                    Address _jiaxing = dbContext.FindAll<Address>(ent => ent.City == "Jiaxing").FirstOrDefault();
                    Assert.IsNotNull(_jiaxing);
                }
            }
        }
    }
}