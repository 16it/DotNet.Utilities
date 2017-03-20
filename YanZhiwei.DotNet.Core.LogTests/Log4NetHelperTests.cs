using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace YanZhiwei.DotNet.Core.Log.Tests
{
    [TestClass()]
    public class Log4NetHelperTests
    {
        private static string logFilePath = string.Empty;
        
        [TestInitialize]
        public void Init()
        {
            string _configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            logFilePath = string.Format(@"{0}\{1}\{2}.log", _configFolder, DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("yyyy-MM-dd"));
            
            if(File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }
        
        [TestMethod()]
        public void InfoTest()
        {
            Log4NetHelper.Warn(LoggerType.WinExceptionLog, "Hello World.");
            Assert.IsTrue(File.Exists(logFilePath));
        }
    }
}