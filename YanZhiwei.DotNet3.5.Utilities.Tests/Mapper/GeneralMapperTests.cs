using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YanZhiwei.DotNet2.Utilities.DataOperator;

namespace YanZhiwei.DotNet3._5.Utilities.Mapper.Tests
{
    [TestClass()]
    public class GeneralMapperTests
    {
        [TestMethod()]
        public void ToListTest()
        {
            SqlServerDataOperator _sqlHelper = new SqlServerDataOperator(@"server=YANZHIWEI-PC\SQLEXPRESS;database=AdventureWorks2014;uid=sa;pwd=sasa;");
            string _sql = @"SELECT [AddressID]
                                  ,[AddressLine1]
                                  ,[AddressLine2]
                                  ,[City]
                                  ,[StateProvinceID]
                                  ,[PostalCode]
                                  ,[rowguid]
                                  ,[ModifiedDate]
                                  FROM [Person].[Address]";
            DataTable _result = _sqlHelper.ExecuteDataTable(_sql, null);
            IEnumerable<Address> _actual = GeneralMapper.ToList<Address>(_result);
            Assert.IsTrue(_actual.Count() > 0);
        }
        
        [TestMethod()]
        public void ToDataTableTest()
        {
            SqlServerDataOperator _sqlHelper = new SqlServerDataOperator(@"server=YANZHIWEI-PC\SQLEXPRESS;database=AdventureWorks2014;uid=sa;pwd=sasa;");
            string _sql = @"SELECT [AddressID]
                                  ,[AddressLine1]
                                  ,[AddressLine2]
                                  ,[City]
                                  ,[StateProvinceID]
                                  ,[PostalCode]
                                  ,[rowguid]
                                  ,[ModifiedDate]
                                  FROM [Person].[Address]";
            List<Address> _result = _sqlHelper.ExecuteReader<Address>(_sql, null);
            DataTable _actual = GeneralMapper.ToDataTable<Address>(_result);
            Assert.IsTrue(_actual.Rows.Count > 0);
        }
    }
}