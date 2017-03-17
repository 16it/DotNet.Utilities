using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace YanZhiwei.DotNet.Newtonsoft.Json.Utilities.Tests
{
    [TestClass()]
    public class JsonHelperTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            Address _item = new Address();
            _item.AddressID = 1;
            _item.PostalCode = "78019";
            _item.AddressLine1 = "Tianxin";
            _item.City = "Zhuzhou";
            _item.StateProvinceID = 78;
            _item.PostalCode = "72229";
            _item.rowguid = Guid.NewGuid();
            string _actual = JsonHelper.Serialize(_item);
            Assert.IsNotNull(_actual);
        }
    }
}