using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YanZhiwei.DotNet.Core.Infrastructure;
using YanZhiwei.DotNet.Core.ServiceTests;
using YanZhiwei.DotNet.Core.ServiceTests.AdventureWorksService;
using YanZhiwei.DotNet.Core.ServiceTests.Events;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet3._5.Utilities.CallContext;

namespace YanZhiwei.DotNet.Core.Service.Tests
{
    [TestClass()]
    public class ServiceHelperTests
    {
        [TestInitialize]
        public void Init()
        {
            EngineContext.Replace(new DefaultEngine());
            EngineContext.Initialize(false);
        }

        [TestMethod]
        public void ModelCacheEventConsumer_InsertTest()
        {
            var _userService = EngineContext.Current.Resolve<IUserService>();
            string _actual = _userService.GetUserName("YanZhiwei");
            Assert.AreEqual("YanZhiwei", _actual);

            _actual = _userService.InsertUser("YanZhiwei1");
            Assert.AreEqual("YanZhiwei1", _actual);
        }

        [TestMethod]
        public void ModelCacheEventConsumer_DeleteTest()
        {
            var _userService = EngineContext.Current.Resolve<IUserService>();
            string _actual = _userService.GetUserName("YanZhiwei");
            Assert.AreEqual("YanZhiwei", _actual);

            _actual = _userService.DeleteUser("YanZhiwei2");
            Assert.AreEqual("YanZhiwei2", _actual);

            _actual = _userService.UpdateUser("YanZhiwei3");
            Assert.AreEqual("YanZhiwei3", _actual);
        }

        [TestMethod]
        public void ModelCacheEventConsumer_UpdateTest()
        {
            var _userService = EngineContext.Current.Resolve<IUserService>();
            string _actual = _userService.GetUserName("YanZhiwei");
            Assert.AreEqual("YanZhiwei", _actual);

            _actual = _userService.UpdateUser("YanZhiwei3");
            Assert.AreEqual("YanZhiwei3", _actual);
        }

        [TestMethod()]
        public void CreateServiceTest()
        {
            ServiceFactory _serverHelper = new ServiceFactory(new SqlDataAccessRefService());
            string _acutal = _serverHelper.CreateService<ISqlHelper, InvokeInterceptor>().HelloWorld();
            Assert.AreEqual("Hello World.", _acutal);

            _acutal = _serverHelper.CreateService<ISqlHelper, InvokeInterceptor>().HelloWorld();
            Assert.AreEqual("Hello World.", _acutal);

            WCFCallContext.Current.Operater = new Operater()
            {
                Name = "churenyouzi",
                Time = DateTime.Now,
                IP = "127.0.0.1",
                Token = Guid.NewGuid().ToString(),
                Method = "Test"
            };
            ServiceFactory _wcfServerHelper = new ServiceFactory(new AdventureWorksServiceProxy());
            int[] _productIDList = _wcfServerHelper.CreateService<IProductsService, InvokeInterceptor>().GetProductIDList();
            Assert.IsTrue(_productIDList.Length > 0);
            Assert.AreEqual(WCFCallContext.Current.Operater.Name, "churenyouzi");

            Product _findedProduct = _wcfServerHelper.CreateService<IProductsService, InvokeInterceptor>().GetProduct(1);
            Assert.AreEqual(_findedProduct.Name, "Adjustable Race");
        }
    }
}