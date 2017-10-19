using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using YanZhiwei.DotNet.Dapper.UtilitiesTests.Model;

namespace YanZhiwei.DotNet.Dapper.Utilities.Tests
{
    [TestClass()]
    public class DapperSQLiteOperatorTests
    {
        public static readonly string DbFile = "./TestDb.db3";
        public DapperSQLiteOperator SQLiteHelper = null;

        [TestInitialize]
        public void Init()
        {
            if (File.Exists(DbFile))
                File.Delete(DbFile);
            SQLiteHelper = new DapperSQLiteOperator(DbFile);
            CreateDatabase();
        }

        public void CreateDatabase()
        {
            string _sql = @"CREATE TABLE IF NOT EXISTS [Users] (
            [FristName] NVARCHAR(64) NOT NULL,
            [LastName] NVARCHAR(128) NOT NULL,
            [BirthDate] date NOT NULL,
            [DateCreated] TIMESTAMP DEFAULT CURRENT_TIMESTAMP )";

            SQLiteHelper.ExecuteNonQuery(_sql);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            //Users _singleUser = new Users();
            //_singleUser.BirthDate = DateTime.Now;
            //_singleUser.DateCreated = DateTime.Now;
            //_singleUser.FristName = "Yan";
            //_singleUser.LastName = "Zhiwei";

            //string _sql = @"insert into Users(BirthDate,DateCreated,FristName,LastName) values(@BirthDate,@DateCreated,@FristName,@LastName)";
            //int _actualSingle = SQLiteHelper.ExecuteNonQuery<Users>(_sql, _singleUser);
            //Assert.AreEqual(1, _actualSingle);

            List<Users> _mutilUsers = new List<Users>();
            for (int i = 0; i < 10; i++)
            {
                Users _user = new Users();
                _user.BirthDate = DateTime.Now;
                _user.DateCreated = DateTime.Now;
                _user.FristName = "Yan" + i;
                _user.LastName = "Zhiwei" + i;
                _mutilUsers.Add(_user);
            }
            int _actual = SQLiteHelper.ExecuteNonQuery<Users>(@"insert into Users(BirthDate,DateCreated,FristName,LastName) values(@BirthDate,@DateCreated,@FristName,@LastName)", _mutilUsers);
            Assert.AreEqual(10, _actual);
        }
    }
}