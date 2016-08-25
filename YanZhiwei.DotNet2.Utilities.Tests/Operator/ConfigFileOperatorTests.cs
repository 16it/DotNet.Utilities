using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.Tests.Model;

namespace YanZhiwei.DotNet2.Utilities.Operator.Tests
{
    [TestClass()]
    public class ConfigFileOperatorTests
    {
        private ConfigFileOperator config = null;

        [TestInitialize]
        public void Instance()
        {
            config = new ConfigFileOperator(ProgramMode.WinForm);
        }

        [TestMethod()]
        public void SaveSectionTest()
        {
            Person _person = new Person();
            _person.Name = "YanZhiwei";
            _person.Address = "China";
            _person.Age = 1;
            config.SaveSection<Person>(_person, "Test");
            Person _actual = config.GetSection<Person>("Test");
            Assert.AreEqual("YanZhiwei", _actual.Name);
        }

        [TestMethod()]
        public void GetSettingTest()
        {
            config.AddOrUpdateSetting("name", "yzw");
            Assert.AreEqual("yzw", config.GetSetting("name"));
        }
    }
}