using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.ServiceTests;

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
        }

        [TestMethod()]
        public void CreateServiceTest1()
        {
            Assert.Fail();
        }
    }
}