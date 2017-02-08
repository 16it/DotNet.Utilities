using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet.QQWry.Utilities.Tests
{
    [TestClass()]
    public class QQWryLocatorTests
    {
        [TestMethod()]
        public void QueryTest()
        {
            QQWryLocator _qqWry = new QQWryLocator(@"D:\OneDrive\软件\开发\qqwry\qqwry.dat");
            IPLocation _ip = _qqWry.Query("116.226.81.32");
            Assert.AreEqual("上海市 电信", string.Format("{0} {1}", _ip.Country, _ip.Local));
        }
    }
}