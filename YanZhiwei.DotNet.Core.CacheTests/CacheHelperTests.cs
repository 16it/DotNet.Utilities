using YanZhiwei.DotNet.Core.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.CacheTests;
using YanZhiwei.DotNet.Core.CacheTests.Model;
using System.Linq;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet.Core.Cache.Tests
{
    [TestClass()]
    public class CacheHelperTests
    {
        [TestMethod()]
        public void SetTest()
        {
            CacheHelper.Set("Name", "YanZhiwei");
            Assert.AreEqual("YanZhiwei", CacheHelper.Get("Name"));
            CacheHelper.Set("LoginInfoName", "YanZhiwei");
            Assert.AreEqual("YanZhiwei", CacheHelper.Get("LoginInfoName"));
        }

        [TestMethod()]
        public void ToCacheArrayTest()
        {
            using (var dbContext = new AccountDbContext())
            {
                User[] _finded = dbContext.Users.Where(ent => ent.IsActive == true).ToCacheArray("hello");
                _finded = dbContext.Users.Where(ent => ent.IsActive == true).ToCacheArray("hello");
                CollectionAssert.AllItemsAreNotNull(_finded);
            }
        }

        [TestMethod()]
        public void ToPageCacheTest()
        {
            using (var dbContext = new AccountDbContext())
            {
                PageResult<User> _finded = dbContext.Users.ToPageCache(u => u.IsActive == true, new PageCondition(1, 10) { PrimaryKeyField= "ID" }, ent => ent, "word");
                _finded = dbContext.Users.ToPageCache(u => u.IsActive == true, new PageCondition(1, 10) { PrimaryKeyField = "ID" }, ent => ent, "word");
                CollectionAssert.AllItemsAreNotNull(_finded.Data);
            }
        }
    }
}