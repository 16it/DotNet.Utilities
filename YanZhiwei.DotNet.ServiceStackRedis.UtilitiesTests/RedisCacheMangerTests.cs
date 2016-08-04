using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.ServiceStackRedis.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace YanZhiwei.DotNet.ServiceStackRedis.Utilities.Tests
{

    //参考：http://blog.csdn.net/wanlong360599336/article/details/46771477
    //http://www.cnblogs.com/lori/p/4026196.html
    //http://www.cnblogs.com/yangecnu/p/Introduct-Redis-in-DotNET-Part2.html
    [TestClass()]
    public class RedisCacheMangerTests
    {
        readonly RedisClient redis = new RedisClient("localhost");
        
        [TestInitialize]
        public void OnBeforeEachTest()
        {
            redis.FlushAll();
        }
        
        [TestMethod()]
        public void RedisCacheMangerTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void DeleteAllTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void DeleteByIdTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void GetAllTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void GetAllTest1()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void GetAllTest2()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void GetAllTest3()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SetTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SetTest1()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SetTest2()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SetAllTest()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SetAllTest1()
        {
            Assert.Fail();
        }
        
        [TestMethod()]
        public void SetAllTest2()
        {
            Assert.Fail();
        }
    }
}