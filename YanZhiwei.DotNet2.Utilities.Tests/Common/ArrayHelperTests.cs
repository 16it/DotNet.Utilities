using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.Common.Tests
{
    [TestClass()]
    public class ArrayHelperTests
    {
        [TestMethod()]
        public void AddRangeTest()
        {
            CollectionAssert.AreEqual(new int[7] { 1, 2, 3, 4, 5, 6, 7 }, ArrayHelper.AddRange(new int[5] { 1, 2, 3, 4, 5 }, new int[2] { 6, 7 }));
        }

        [TestMethod()]
        public void AddTest()
        {
            CollectionAssert.AreEqual(new int[6] { 1, 2, 3, 4, 5, 6 }, ArrayHelper.Add(new int[5] { 1, 2, 3, 4, 5 }, 6));
        }

        [TestMethod()]
        public void CopyTest()
        {
            CollectionAssert.AreEqual(new int[3] { 1, 2, 3 }, ArrayHelper.Copy(new int[5] { 1, 2, 3, 4, 5 }, 0, 3));
            CollectionAssert.AreEqual(new int[5] { 1, 2, 3, 4, 5 }, ArrayHelper.Copy(new int[5] { 1, 2, 3, 4, 5 }, 0, 5));
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Assert.IsTrue(ArrayHelper.CompletelyEqual(new int[5] { 1, 2, 3, 4, 5 }, new int[5] { 1, 2, 3, 4, 5 }));
        }

        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            Assert.IsTrue(ArrayHelper.IsEmpty(new int[0]));
        }
    }
}