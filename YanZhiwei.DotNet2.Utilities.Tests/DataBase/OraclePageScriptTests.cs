using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.DataBase.Tests
{
    [TestClass()]
    public class OraclePageScriptTests
    {
        [TestMethod()]
        public void JoinPageSQLByRowNumberTest()
        {
            string _actual = OraclePageScript.JoinPageSQLByRowNumber("HR.COUNTRIES", "*", "COUNTRY_ID", string.Empty, Enum.OrderType.Desc, 10, 2);
            string _expect = @"select * from (SELECT A.*, ROWNUM RN FROM (SELECT * FROM HR.COUNTRIES order by COUNTRY_ID desc) A ) WHERE RN BETWEEN 11 AND 21 ;select count(*) from HR.COUNTRIES";
            Assert.AreEqual(_expect, _actual);
            _actual = OraclePageScript.JoinPageSQLByRowNumber("HR.COUNTRIES", "*", "COUNTRY_ID", "REGION_ID>=1", Enum.OrderType.Desc, 10, 2);
            _expect = @"select * from (SELECT A.*, ROWNUM RN FROM (SELECT * FROM HR.COUNTRIES order by COUNTRY_ID desc) A ) WHERE RN BETWEEN 11 AND 21  and ( REGION_ID>=1 );select count(*) from HR.COUNTRIES";
            Assert.AreEqual(_expect, _actual);
        }
    }
}