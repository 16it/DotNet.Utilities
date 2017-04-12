using YanZhiwei.DotNet2.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.Common.Tests
{
    [TestClass()]
    public class FuzzyHelperTests
    {
        [TestMethod()]
        public void FuzzyEmailTest()
        {
            string _email = "churenyouzi@outlook.com";
            string _action = FuzzyHelper.FuzzyEmail(_email);
            string _expected = "ch*********@outlook.com";
            Assert.AreEqual(_action, _expected);
        }

        [TestMethod()]
        public void FuzzyMobieNumberTest()
        {
            string _phoneNumber = "18501600110";
            string _action = FuzzyHelper.FuzzyMobieNumber(_phoneNumber);
            string _expected = "185****0110";
            Assert.AreEqual(_action, _expected);
        }

        [TestMethod()]
        public void FuzzyUserNameTest()
        {
            string _userName = "朱重八";
            string _action = FuzzyHelper.FuzzyUserName(_userName);
            string _expected = "朱***八";
            Assert.AreEqual(_action, _expected);
        }
    }
}