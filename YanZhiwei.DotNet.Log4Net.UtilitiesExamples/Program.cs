using System;
using YanZhiwei.DotNet.Log4Net.Utilities;

namespace YanZhiwei.DotNet.Log4Net.UtilitiesExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始：" + DateTime.Now.ToLongTimeString());
                //  Log4NetHelper.SetLogger("AdoNetLogger");
                Log4NetHelper.WriteDebug("Debug 你好");
                Log4NetHelper.WriteError("ERROR");
                Log4NetHelper.WriteFatal("Fatal");
                Log4NetHelper.WriteWarn("Warn");
                Log4NetHelper.WriteInfo("Info");
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