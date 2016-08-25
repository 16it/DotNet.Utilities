using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.Operator.Tests
{
    [TestClass()]
    public class INIOperatorTests
    {
        private INIOperator INIHelper = null;

        [TestInitialize()]
        public void Instance()
        {
            INIHelper = new INIOperator(@"D:\demo.ini");
        }

        [TestMethod()]
        public void GetTest()
        {
            INIHelper.Write("basic", "name", "yzw");
            Assert.AreEqual(INIHelper.Get("basic", "name"), "yzw");
        }
    }
}