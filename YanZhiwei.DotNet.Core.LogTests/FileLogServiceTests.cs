using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace YanZhiwei.DotNet.Core.Log.Tests
{
    [TestClass()]
    public class FileLogServiceTests
    {
        private static string LogFolder = string.Empty;
        private ILogService LogHelper = null;

        [TestInitialize]
        public void Init()
        {
            LogFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

            if (Directory.Exists(LogFolder))
            {
                Directory.Delete(LogFolder);
            }

            LogHelper = new FileLogService();
        }

        [TestMethod()]
        public void InfoTest()
        {
            LogHelper.Info("Hello World.");
            string _logFilePath = string.Format(@"{0}\INFO\{1}\{2}.log", LogFolder, DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("yyyy-MM-dd"));
            Assert.IsTrue(File.Exists(_logFilePath));
        }
    }
}