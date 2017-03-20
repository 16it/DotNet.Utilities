using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}