using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.InfrastructureTests;

namespace YanZhiwei.DotNet.Core.Infrastructure.Tests
{
    [TestClass()]
    public class DefaultEngineTests
    {
        [TestInitialize]
        public void Init()
        {
            EngineContext.Replace(new DefaultEngine());
            EngineContext.Initialize(false);
        }

        [TestMethod()]
        public void ResolveTest()
        {
            var _userService = EngineContext.Current.Resolve<IUserService>();
            var _actual = _userService.GetUserName("YanZhiwei");
            Assert.AreEqual("YanZhiwei", "YanZhiwei");
        }
    }
}