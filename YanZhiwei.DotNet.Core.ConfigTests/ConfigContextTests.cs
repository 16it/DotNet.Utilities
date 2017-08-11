using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.Config.Model;
using YanZhiwei.DotNet.Core.Config.Tests.Models;

namespace YanZhiwei.DotNet.Core.Config.Tests
{
    [TestClass()]
    public class ConfigContextTests
    {
        [TestMethod()]
        public void GetTest()
        {
            ConfigContext _configContext = new ConfigContext();
            DaoConfig _daoConfig = CachedConfigContext.Instance.Get<DaoConfig>();
            string _expect = @"Data Source=YANZHIWEI-IT-PC\SQLEXPRESS;Initial Catalog=GMSLog;Persist Security Info=True;User ID=sa;Password=sasa";
            Assert.AreEqual(_expect, _daoConfig.Log);


            RedisConfig _actualRedisConfig = CachedConfigContext.Instance.Get<RedisConfig>();

            RedisConfig _expectRedisConfig = new RedisConfig();
            _expectRedisConfig.AutoStart = true;
            _expectRedisConfig.LocalCacheTime = 180;
            _expectRedisConfig.MaxReadPoolSize = 36;
            _expectRedisConfig.MaxWritePoolSize = 36;
            _expectRedisConfig.ReadServerList = "127.0.0.1:6379";
            _expectRedisConfig.WriteServerList = "127.0.0.1:6379";

            Assert.AreEqual(_expectRedisConfig.ReadServerList, _actualRedisConfig.ReadServerList);
            Assert.AreEqual(_expectRedisConfig.WriteServerList, _actualRedisConfig.WriteServerList);
            Assert.AreEqual(_expectRedisConfig.LocalCacheTime, _actualRedisConfig.LocalCacheTime);
            //_configContext.Save(_redisConfig);
        }
    }
}