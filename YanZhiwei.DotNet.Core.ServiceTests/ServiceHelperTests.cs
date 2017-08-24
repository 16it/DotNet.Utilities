using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YanZhiwei.DotNet.Core.Infrastructure;
using YanZhiwei.DotNet.Core.ServiceTests;
using YanZhiwei.DotNet.Core.ServiceTests.AdventureWorksService;
using YanZhiwei.DotNet.Core.ServiceTests.Events;
using YanZhiwei.DotNet.Core.ServiceTests.Model;
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
        public void DeleteUserTest()
        {
            var services = EngineContext.Current.Resolve<IUserService>();
            services.Delete(new User());
        }

        [TestMethod()]
        public void CreateServiceTest()
        {
            ServiceHelper _serverHelper = new ServiceHelper(new SqlDataAccessRefService());
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
            ServiceHelper _wcfServerHelper = new ServiceHelper(new AdventureWorksServiceProxy());
            int[] _productIDList = _wcfServerHelper.CreateService<IProductsService, InvokeInterceptor>().GetProductIDList();
            Assert.IsTrue(_productIDList.Length > 0);
            Assert.AreEqual(WCFCallContext.Current.Operater.Name, "churenyouzi");

            Product _findedProduct = _wcfServerHelper.CreateService<IProductsService, InvokeInterceptor>().GetProduct(1);
            Assert.AreEqual(_findedProduct.Name, "Adjustable Race");
        }
    }
}