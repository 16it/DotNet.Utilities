using log4net.Config;
using System;
using YanZhiwei.DotNet.Log4Net.Utilities;
using YanZhiwei.DotNet2.Utilities.DesignPattern;

namespace YanZhiwei.DotNet.Log4Net.UtilitiesExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //XmlConfigurator.Configure();
                //var date = DateTime.Now.AddDays(-10);
                //var task = new LogFileCleanupTask();
                //task.CleanUp(date);
                //Console.WriteLine("开始：" + DateTime.Now.ToLongTimeString());
                //  Log4NetHelper.SetLogger("AdoNetLogger");

                //Singleton<FileLogService>.CreateInstance().Debug("Debug 你好");
                Singleton<FileLogService>.CreateInstance().Error("ERROR");
                //Singleton<FileLogService>.CreateInstance().Fatal("Fatal");
                //Singleton<FileLogService>.CreateInstance().Warn("Warn");
                //Singleton<FileLogService>.CreateInstance().Info("Info");
                Console.WriteLine("end....");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("结束：" + DateTime.Now.ToLongTimeString());
                Console.ReadLine();
            }
        }
    }
}