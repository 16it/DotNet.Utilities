using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YanZhiwei.DotNet.Core.Log.Tests
{
    [TestClass()]
    public class SQLServerLogServiceTests
    {
        ILogService LogHelper = null;
        [TestInitialize]
        public void Init()
        {
            LogHelper = new SQLServerLogService();
        }

        [TestMethod()]
        public void DebugTest()
        {
            LogHelper.Debug("ms sqlserver ado.net test.");
        }

        [TestMethod()]
        public void DebugTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DebugTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DebugTest3()
        {
            Assert.Fail();
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