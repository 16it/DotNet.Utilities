using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Dapper.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YanZhiwei.DotNet.Dapper.Utilities.Tests
{
    [TestClass()]
    public class DapperDataOperatorTests
    {
        DapperDataOperator OralceHelper = null;
        public void Init()
        {
            string _connectString = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.102.2.165)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=LC)));User Id=user01;Password=Orcl_4321;";
            OralceHelper = new DapperOracleOperator(_connectString);
        }
        [TestMethod()]
        public void ExecuteDataTableTest()
        {
            string _sql = @"SELECT YMD
		                          ,HMS
		                          ,POINT_ID
		                          ,FLAG
		                          ,VALUE
		                          ,STATUS
		                          ,IN_TIME
	                              FROM LMS_CENTER.XN_HISDAT_201502005
	                              WHERE ROWNUM<=500 ";
            DataTable _actual = OralceHelper.ExecuteDataTable<XN_HISDAT>(_sql, null);
            Assert.AreEqual(_actual.Rows.Count, 500);
        }
    }
    public class XN_HISDAT
    {
        public int YMD
        {
            get;
            set;
        }
        public int HMS
        {
            get;
            set;
        }
        
        public int POINT_ID
        {
            get;
            set;
        }
        public int FLAG
        {
            get;
            set;
        }
        public decimal VALUE
        {
            get;
            set;
        }
        
        public int STATUS
        {
            get;
            set;
        }
        public DateTime IN_TIME
        {
            get;
            set;
        }
    }
}