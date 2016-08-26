using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Tests.Model;

namespace YanZhiwei.DotNet2.Utilities.DataOperator.Tests
{
    [TestClass()]
    public class ModelHelperTests
    {
        [TestMethod()]
        public void CompletelyEqualTest()
        {
            Person[] _personAList = new Person[10];
            Person[] _personBList = new Person[10];

            for(int i = 0; i < 10; i++)
            {
                Person _tmp = new Person();
                _tmp.Age = 1;
                _tmp.Name = string.Format("YanZhiwei{0}", i);
                _tmp.Address = "shanghai";
                _personAList[i] = _tmp;
                _personBList[i] = _tmp;
            }

            Assert.IsTrue(ModelHelper.CompletelyEqual<Person>(_personAList, _personBList));
        }
    }
}