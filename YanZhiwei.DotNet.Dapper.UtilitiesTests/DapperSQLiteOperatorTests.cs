using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using YanZhiwei.DotNet.Dapper.UtilitiesTests;

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

            SQLiteHelper.ExecuteNonQuery(_sql);

            _sql = @"CREATE TABLE IF NOT EXISTS [JdfEvents] (
            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
            [OrderCmd] smallint NOT NULL,
            [OrderSeqNo] smallint NOT NULL,
            [TimeStamps] TIMESTAMP NOT NULL,
            [ProtocolVer] varchar(4) NOT NULL,
            [SourceAddr] varchar(12) NOT NULL,
            [DescAddr] varchar(12) NOT NULL,
            [SystemAddr] smallint NOT NULL,
            [FullPackageDataHexString] varchar(2000) NOT NULL,
            [ComponentAddr] smallint NOT NULL,
            [ComponentType] smallint NOT NULL,
            [CtuCh] smallint NOT NULL,
            [NetworkNodeAddr] smallint NOT NULL,
            [PropertyName] NVARCHAR(128) NOT NULL,
            [PropertyValue] INTEGER NOT NULL,
            [HasPropertyValue] bit NOT NULL,
            [CreateTime] TIMESTAMP TIMESTAMP DEFAULT CURRENT_TIMESTAMP)";
            SQLiteHelper.ExecuteNonQuery(_sql);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            //var sql = @"INSERT INTO JdfEvents(Username, Email, Password)
            //VALUES(@Username, @Email, @Password)";

            //int _acutal = SQLiteHelper.ExecuteNonQuery(sql, new
            //User
            //{
            //    DateCreated = DateTime.Now,
            //    Email = "churenyouzi@outlook.com",
            //    Password = "123456",
            //    Username = "churenyouzi"
            //});
            //Assert.IsTrue(_acutal == 1);

            string _sql = @"INSERT INTO JdfEvents(OrderCmd, 
                                                  OrderSeqNo, 
                                                  TimeStamps,
                                                  ProtocolVer,
                                                  SourceAddr,
                                                  DescAddr,
                                                  SystemAddr,
                                                  FullPackageDataHexString,
                                                  ComponentAddr,
                                                  ComponentType,
                                                  CtuCh,
                                                  NetworkNodeAddr,
                                                  PropertyName,
                                                  PropertyValue,
                                                  HasPropertyValue)
            VALUES(@OrderCmd, @OrderSeqNo, @TimeStamps,@ProtocolVer,@SourceAddr,@DescAddr,@SystemAddr,
                   @FullPackageDataHexString,@ComponentAddr,@ComponentType,@CtuCh,@NetworkNodeAddr,@PropertyName,@PropertyValue,@HasPropertyValue)";

            EventModel _model = new EventModel();
            _model.OrderCmd = 0x99;
            _model.OrderSeqNo = 99;
            _model.TimeStamps = DateTime.Now.AddHours(-1);
            _model.ProtocolVer = "0101";
            _model.SourceAddr = "170102030405";
            _model.DescAddr = "170102030400";
            _model.SystemAddr = 0x01;
            _model.FullPackageDataHexString = "01 02 03 04 05 06 07 08 09";
            _model.ComponentAddr = 02;
            _model.ComponentType = 03;
            _model.CtuCh = 04;
            _model.NetworkNodeAddr = 05;
            _model.PropertyName = "启动";
            _model.PropertyValue = 0x01;
            _model.HasPropertyValue = true;
            SQLiteHelper.ExecuteNonQuery<EventModel>(_sql, _model);
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