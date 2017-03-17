using Newtonsoft.Json;
using System;
using System.Data.Entity;
using YanZhiwei.DotNet.Framework.Contract;
using YanZhiwei.DotNet.Framework.Data;
using YanZhiwei.DotNet.Newtonsoft.Json.Utilities;

namespace YanZhiwei.DotNet.Framework.DataTests
{
    public class LogDbContext : DbContextBase<int>, IAuditable
    {
        public LogDbContext() : base(@"Data Source=DESKTOP-N3GTH4E\SQLEXPRESS;Initial Catalog=AdventureWorks2014;Persist Security Info=True;User ID=sa;Password=sasa")
        {
        }
        
        public DbSet<AuditLog> AuditLogs
        {
            get;
            set;
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LogDbContext>(null);//从不创建数据库
            base.OnModelCreating(modelBuilder);
        }
        
        public void WriteLog(int modelId, string userName, string moduleName, string tableName, string eventType, ModelBase newValues)
        {
            try
            {
                AuditLog _item = new AuditLog();
                _item.ModelId = modelId;
                _item.UserName = userName;
                _item.ModuleName = moduleName;
                _item.TableName = tableName;
                _item.EventType = eventType;
                _item.NewValues =  JsonHelper.Serialize(newValues);
                this.AuditLogs.Add(_item);
                this.SaveChanges();
                //this.Dispose();
            }
            catch(Exception ex)
            {
                string aa = ex.Message;
            }
        }
    }
}