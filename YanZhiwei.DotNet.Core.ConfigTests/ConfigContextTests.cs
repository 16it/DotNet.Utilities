using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.Config.Tests.Models;

namespace YanZhiwei.DotNet.Core.Config.Tests
{
    [TestClass()]
    public class ConfigContextTests
    {
        [TestMethod()]
        public void GetTest()
        {
            ConfigContext _configContext = new ConfigContext();
            DaoConfig _daoConfig = CachedConfigContext.Instance.Get<DaoConfig>();
            string _expect = @"Data Source=YANZHIWEI-IT-PC\SQLEXPRESS;Initial Catalog=GMSLog;Persist Security Info=True;User ID=sa;Password=sasa";
            Assert.AreEqual(_expect, _daoConfig.Log);
        }
    }
}