using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YanZhiwei.DotNet.Core.CacheTests;
using YanZhiwei.DotNet.Core.CacheTests.Model;
using YanZhiwei.DotNet.Core.CacheTests.Service;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet.Core.Cache.Tests
{
    [TestClass()]
    public class CacheHelperTests
    {
        [TestInitialize]
        public void Init()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IEventPublisher>(new EventPublisher(), new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(IConsumer<>), typeof(ModelCacheEventConsumer), new ContainerControlledLifetimeManager());
        }

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

            IEventPublisher _eventPublish = new EventPublisher();
            UserService _userService = new UserService(_eventPublish);
            User _insertUser = new User();
            _insertUser.CreateTime = DateTime.Now;
            _insertUser.Email = "churenyouzi@gmail.com";
            _insertUser.IsActive = true;
            _insertUser.LoginName = "churenyouzi";
            _insertUser.Mobile = "13167781234";
            _insertUser.Password = "1234567890";

            _userService.Insert(_insertUser);
            //using (var dbContext = new AccountDbContext())
            //{
            //    User[] _finded = dbContext.Users.Where(ent => ent.IsActive == true).ToCacheArray("hello");
            //    _finded = dbContext.Users.Where(ent => ent.IsActive == true).ToCacheArray("hello");
            //    CollectionAssert.AllItemsAreNotNull(_finded);
            //}
        }

        [TestMethod()]
        public void ToPageCacheTest()
        {
            using (var dbContext = new AccountDbContext())
            {
                PageResult<User> _finded = dbContext.Users.ToPageCache(u => u.IsActive == true, new PageCondition(1, 10) { PrimaryKeyField = "ID" }, ent => ent, "word");
                _finded = dbContext.Users.ToPageCache(u => u.IsActive == true, new PageCondition(1, 10) { PrimaryKeyField = "ID" }, ent => ent, "word");
                CollectionAssert.AllItemsAreNotNull(_finded.Data);
            }
        }
    }
}