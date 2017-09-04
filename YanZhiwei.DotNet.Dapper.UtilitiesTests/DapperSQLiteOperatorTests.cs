using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace YanZhiwei.DotNet.Dapper.Utilities.Tests
{
    [TestClass()]
    public class DapperSQLiteOperatorTests
    {
        public static readonly string DbFile = "./TestDb.db3";
        public DapperDataOperator SQLiteHelper = null;

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
            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            [Username] NVARCHAR(64) NOT NULL,
            [Email] NVARCHAR(128) NOT NULL,
            [Password] NVARCHAR(128) NOT NULL,
            [DateCreated] TIMESTAMP DEFAULT CURRENT_TIMESTAMP )";

            int _acutal = SQLiteHelper.ExecuteNonQuery(_sql);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            var sql = @"INSERT INTO Users(Username, Email, Password)
            VALUES(@Username, @Email, @Password)";

            int _acutal = SQLiteHelper.ExecuteNonQuery(sql, new
            User
            {
                DateCreated = DateTime.Now,
                Email = "churenyouzi@outlook.com",
                Password = "123456",
                Username = "churenyouzi"
            });
            Assert.IsTrue(_acutal == 1);
        }
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DateCreated { get; set; }
}