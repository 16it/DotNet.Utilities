using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Tests
{
    [TestClass()]
    public class NetHelperTests
    {
        [TestMethod()]
        public void HttpGetTest()
        {
            string _url = "https://m.kuaidi100.com/query?type=huitongkuaidi&postid=70379905087149&id=1&valicode=&temp=0.5464363205134575";
            string _acturl = NetHelper.HttpGet(_url);
            Assert.IsNotNull(_acturl);
        }
    }
}