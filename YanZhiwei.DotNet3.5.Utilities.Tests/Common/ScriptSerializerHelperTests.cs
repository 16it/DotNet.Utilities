using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Result;
using YanZhiwei.DotNet3._5.UtilitiesTests.Model;

namespace YanZhiwei.DotNet3._5.Utilities.Common.Tests
{
    [TestClass()]
    public class ScriptSerializerHelperTests
    {
        [TestMethod()]
        public void JsonSerializeTest()
        {
            //Person _personA = new Person()
            //{
            //    Name = "YanZhiweiA",
            //    Age = 10,
            //    Address = "shanghaiA",
            //    Login = DateTime.Now,
            //    Birth = new DateTime(2012, 10, 10, 1, 1, 1)
            //};
            //Person _personB = new Person()
            //{
            //    Name = "YanZhiweiB",
            //    Age = 11,
            //    Address = "shanghaiB",
            //    Login = DateTime.Now,
            //    Birth = new DateTime(2012, 10, 10, 1, 1, 1)
            //};
            //IList<Person> _personList = new List<Person>();
            //_personList.Add(_personA);
            //_personList.Add(_personB);
            //CNDateTimeConverter _datetimeConvert = new CNDateTimeConverter("yyyy-MM-dd");
            //string _actual = SerializeHelper.JsonSerialize<IList<Person>>(_personList, _datetimeConvert);
            //string _expect = "[{\"Name\":\"YanZhiweiA\",\"Age\":10,\"Address\":\"shanghaiA\",\"Birth\":{\"DateTime\":\"2012-10-10\"},\"Login\":{\"DateTime\":\"2015-05-07\"},\"OptRecordList\":null},{\"Name\":\"YanZhiweiB\",\"Age\":11,\"Address\":\"shanghaiB\",\"Birth\":{\"DateTime\":\"2012-10-10\"},\"Login\":{\"DateTime\":\"2015-05-07\"},\"OptRecordList\":null}]";
            //Assert.AreEqual<string>(_expect, _actual);

            string _jsonString = SerializeHelper.JsonSerialize(OperatedResult<TokenInfo>.Success(new TokenInfo() { Access_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNDkyNjc5NjU4Ljg1OTE2Nzh9.pwJV_9N5MLV8aWO9NF001OdnlnriWhDetQ2SI2xJWto", Expires_in = 604800 }));
        }

        [TestMethod()]
        public void JsonDeserializeTest()
        {
            Person _personA = new Person()
            {
                Name = "YanZhiweiA",
                Age = 10,
                Address = "shanghaiA"
            };
            Person _personB = new Person()
            {
                Name = "YanZhiweiB",
                Age = 11,
                Address = "shanghaiB"
            };
            List<Person> _expected = new List<Person>();
            _expected.Add(_personA);
            _expected.Add(_personB);
            string _jsonString = "[{'Name':'YanZhiweiA','Age':10,'Address':'shanghaiA'},{'Name':'YanZhiweiB','Age':11,'Address':'shanghaiB'}]";
            List<Person> _result = SerializeHelper.JsonDeserialize<List<Person>>(_jsonString);
            bool _actual = _expected.SequenceEqual(_result, new PersonCompare());
            Assert.IsTrue(_actual);

            _jsonString = @"{'State':true,'Message':null,'Data':{'Access_token':'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ0ZXN0IiwiaWF0IjoxNDkyNjc5NjU4Ljg1OTE2Nzh9.pwJV_9N5MLV8aWO9NF001OdnlnriWhDetQ2SI2xJWto','Expires_in':604800}}";

            OperatedResult<TokenInfo> _actualTokenInfo = SerializeHelper.JsonDeserialize<OperatedResult<TokenInfo>>(_jsonString);
        }

        [TestMethod()]
        public void ConvertTimeJsonTest()
        {
            string _actual = SerializeHelper.ParseJsonDateTime(@"[{'getTime':'\/Date(1419564257428)\/'}]", "yyyyMMdd hh:mm:ss");
            Assert.AreEqual("[{'getTime':'20141226 11:24:17'}]", _actual);
        }
    }

    public class PersonCompare : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            return (x.Age == y.Age) && (x.Address == y.Address) && (x.Name == y.Name);
        }

        public int GetHashCode(Person obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}