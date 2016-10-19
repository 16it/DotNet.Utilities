using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace YanZhiwei.DotNet2.Utilities.WebForm.Tests
{
    [TestClass()]
    public class WebRequestHelperTests
    {
        [TestMethod()]
        public void GetQueryStringTest()
        {
            IDictionary<string, string> _paramter = new Dictionary<string, string>();
            _paramter["name"] = "yanzhiwei";
            _paramter["token"] = "aadcdeaurewra";
            string _queryString = WebRequestHelper.GetQueryString(_paramter);
            Assert.AreEqual("name=yanzhiwei&token=aadcdeaurewra", _queryString);
        }
    }
}