using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet3._5.Utilities.Common.Tests
{
    [TestClass()]
    public class DisplayNameHelperTests
    {
        [TestMethod()]
        public void GetTest()
        {
            string _actual = DisplayNameHelper.Get<Address>("City");
            Assert.AreEqual("城市", _actual);
            _actual = DisplayNameHelper.Get<Address>("AddressID");
            Assert.AreEqual("AddressID", _actual);
            _actual = DisplayNameHelper.Get<Address>(c => c.AddressID);
            Assert.AreEqual("AddressID", _actual);
            _actual = DisplayNameHelper.Get<Address>(c => c.City);
            Assert.AreEqual("城市", _actual);
        }
    }
}