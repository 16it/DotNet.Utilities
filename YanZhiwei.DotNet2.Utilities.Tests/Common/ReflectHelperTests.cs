using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.Common.Tests
{
    [TestClass()]
    public class ReflectHelperTests
    {
        [TestMethod()]
        public void DictionaryFromTypeTest()
        {
            Student _student = new Student();
            _student.Name = "yanzhiwei";
            _student.Age = 10;
            var _actual = _student.DictionaryFromType<Student>();
            Assert.IsNotNull(_actual);
        }

        [TestMethod()]
        public void GetPropertyInfoTest()
        {
            var _actual = ReflectHelper.GetPropertyInfo<Student>();
            Assert.IsNotNull(_actual);
        }
    }

    public class Student
    {
        public short Age
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}