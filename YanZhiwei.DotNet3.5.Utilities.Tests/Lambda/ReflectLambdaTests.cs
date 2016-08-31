using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet3._5.UtilitiesTests.Model;

namespace YanZhiwei.DotNet3._5.Utilities.Lambda.Tests
{
    [TestClass()]
    public class ReflectLambdaTests
    {
        [TestMethod()]
        public void GetPropertyDisplayNameTest()
        {
            Person _person = new Person();
            string _actual = _person.GetPropertyDisplayName<Person>(c => c.Name);
            Assert.AreEqual("姓名", _actual);
            _actual = _person.GetPropertyDisplayName<Person>(c => c.Login);
            Assert.AreEqual("Login", _actual);
        }
    }
}