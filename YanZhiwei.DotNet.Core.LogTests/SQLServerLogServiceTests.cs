using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet.Core.Log.Tests
{
    public class Person
    {
        public string Name { get; set; }
        public ushort Age { get; set; }
    }

    [TestClass()]
    public class SQLServerLogServiceTests
    {
        private ILogService LogHelper = null;

        [TestInitialize]
        public void Init()
        {
            LogHelper = new SQLServerLogService();
        }

        [TestMethod()]
        public void DebugTest()
        {
            LogHelper.Debug("ms sqlserver ado.net test.");
            Person _logData = new Person();
            _logData.Age = 1;
            _logData.Name = "hell world.";
            LogHelper.Debug<Person>(_logData);

        }

        [TestMethod()]
        public void ErrorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ErrorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ErrorTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ErrorTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FatalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FatalTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FatalTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FatalTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InfoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InfoTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InfoTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InfoTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WarnTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WarnTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WarnTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WarnTest3()
        {
            Assert.Fail();
        }
    }
}