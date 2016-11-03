using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.Redis;

namespace YanZhiwei.DotNet.ServiceStackRedis.Utilities.Tests
{
    [TestClass()]
    public class RedisCacheMangerTests
    {
        private RedisCacheManger redisCacheHelper = null;
        
        [TestInitialize]
        public void Init()
        {
            RedisClient _client = new RedisClient("127.0.0.1", 6379, null, 0);
            redisCacheHelper = new RedisCacheManger(_client);
        }
        
        //[TestMethod()]
        //public void SetTest()
        //{
        //    using(IRedisClient RClient = redisCacheHelper.RedisClient)
        //    {
        //        RClient.Add("key", 1);
        //        using(IRedisTransaction IRT = RClient.CreateTransaction())
        //        {
        //            IRT.QueueCommand(r => r.Set("key", 20));
        //            IRT.QueueCommand(r => r.Increment("key", 1));
        //            IRT.Commit(); // 提交事务
        //        }
        //    }
        //}
        
        [TestMethod()]
        public void CreateTransactionTest()
        {
            Person _person = new Person();
            _person.Age = 10;
            _person.Name = "churenyouzi";
            redisCacheHelper.CreateTransaction(trans =>
            {
                trans.QueueCommand(r => r.Set<Person>(_person.Id, _person));
                // trans.QueueCommand(r => r.Increment("key", 1));
                trans.Commit(); // 提交事务
            });
            redisCacheHelper.Save();
            bool _acutal = redisCacheHelper.RedisClient.ContainsKey(_person.Id);
            Assert.IsTrue(_acutal);
        }
    }
    
    public class Person : IHasId<string>
    {
        public string Name
        {
            get;
            set;
        }
        
        public ushort Age
        {
            get;
            set;
        }
        
        public string Id
        {
            get
            {
                return "BB8F637E-0CB3-4193-BBCC-61C8BD83698F";
            }
        }
    }
}