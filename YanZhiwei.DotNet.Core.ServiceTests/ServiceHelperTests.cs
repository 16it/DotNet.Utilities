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
            ServiceHelper _serviceHelper = new ServiceHelper();
            string _acutal = _serviceHelper.CreateService<ISqlHelper, InvokeInterceptor>().HelloWorld();
            Assert.AreEqual("Hello World.", _acutal);
        }
    }
}