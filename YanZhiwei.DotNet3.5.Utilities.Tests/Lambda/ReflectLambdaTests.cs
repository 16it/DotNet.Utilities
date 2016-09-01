using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Common;
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

        [TestMethod()]
        public void GetPropertyValueTest()
        {
            Person _person = new Person();
            _person.Age = 10;
            int _actual = _person.GetPropertyValue<Person>(c => c.Age).ToInt32OrDefault(0);
            Assert.AreEqual(10, _actual);
        }

        [TestMethod()]
        public void SetPropertyValueTest()
        {
            Person _person = new Person();
            _person.SetPropertyValue<Person, byte>(10, c => c.Age);
            int _actual = _person.GetPropertyValue<Person>(c => c.Age).ToByteOrDefault(0);
            Assert.AreEqual(10, _actual);
        }
    }
}