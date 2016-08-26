using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet2.Utilities.DataOperator.Tests
{
    [TestClass()]
    public class ValidateHelperTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InRangeTest()
        {
            ValidateOperator.Begin().InRange(2, 3, 8, "2");
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void IsNumberTest()
        {
            ValidateOperator.Begin().IsNumber("yanzhiwei", "不是数字.");
        }
    }
}