using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.ServiceTests;
using YanZhiwei.DotNet.Core.ServiceTests.AdventureWorksService;

namespace YanZhiwei.DotNet.Core.Service.Tests
{
    [TestClass()]
    public class ServiceHelperTests
    {
        [TestMethod()]
        public void CreateServiceTest()
        {
            ServiceHelper _serverHelper = new ServiceHelper(new SqlDataAccessRefService());
            string _acutal = _serverHelper.CreateService<ISqlHelper, InvokeInterceptor>().HelloWorld();
            Assert.AreEqual("Hello World.", _acutal);

            _acutal = _serverHelper.CreateService<ISqlHelper, InvokeInterceptor>().HelloWorld();
            Assert.AreEqual("Hello World.", _acutal);

            ServiceHelper _wcfServerHelper = new ServiceHelper(new AdventureWorksServiceProxy());
            int[] _productIDList = _wcfServerHelper.CreateService<IProductsService, InvokeInterceptor>().GetProductIDList();
            Assert.IsTrue(_productIDList.Length > 0);

            Product _findedProduct = _wcfServerHelper.CreateService<IProductsService, InvokeInterceptor>().GetProduct(1);
            Assert.AreEqual(_findedProduct.Name, "Adjustable Race");
        }
    }
}