using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.DesignPatterns.Model;
using ServiceStack.Redis;
using System;

namespace YanZhiwei.DotNet.ServiceStackRedis.Utilities.Tests
{
    [TestClass()]
    public class RedisCacheMangerTests
    {
        private RedisCacheManger redisCacheHelper = null;
        
        [TestInitialize]
        public void Init()
        {
            redisCacheHelper = new RedisCacheManger("127.0.0.1", 6379, 0);
            redisCacheHelper.DeleteById<Person>("BB8F637E-0CB3-4193-BBCC-61C8BD83698F");
            redisCacheHelper.DeleteById<Person>("BB8F637E-0CB3-4193-BBCC-61C8BD836981");
            redisCacheHelper.Save();
        }
        
        [TestMethod()]
        public void CreateTransactionTest()
        {
            redisCacheHelper.CreateTransaction<Person>(trans =>
            {
                try
                {
                    Person _person = new Person();
                    _person.Age = 10;
                    _person.Name = "churenyouzi";
                    _person.Id = "BB8F637E-0CB3-4193-BBCC-61C8BD83698F".ToLower();
                    trans.QueueCommand(c => c.Store(_person));
                    throw new Exception("test");
                    _person.Age = 11;
                    _person.Name = "churenyouz1";
                    _person.Id = "BB8F637E-0CB3-4193-BBCC-61C8BD836981".ToLower();
                    trans.QueueCommand(c => c.Store(_person));
                    trans.Commit();
                }
                catch(Exception)
                {
                    trans.Rollback();
                }
            });
            redisCacheHelper.Save();
            bool _actual = redisCacheHelper.ContainsKey<Person>("BB8F637E-0CB3-4193-BBCC-61C8BD836981".ToLower());
            Assert.IsFalse(_actual);
            redisCacheHelper.CreateTransaction<Person>(trans =>
            {
                try
                {
                    Person _person = new Person();
                    _person.Age = 10;
                    _person.Name = "churenyouzi";
                    _person.Id = "BB8F637E-0CB3-4193-BBCC-61C8BD83698F".ToLower();
                    trans.QueueCommand(c => c.Store(_person));
                    _person.Age = 11;
                    _person.Name = "churenyouz1";
                    _person.Id = "BB8F637E-0CB3-4193-BBCC-61C8BD836981".ToLower();
                    trans.QueueCommand(c => c.Store(_person));
                    trans.Commit();
                }
                catch(Exception)
                {
                    trans.Rollback();
                }
            });
            redisCacheHelper.Save();
            _actual = redisCacheHelper.ContainsKey<Person>("BB8F637E-0CB3-4193-BBCC-61C8BD836981".ToLower());
            Assert.IsTrue(_actual);
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
            get;
            set;
        }
    }
}