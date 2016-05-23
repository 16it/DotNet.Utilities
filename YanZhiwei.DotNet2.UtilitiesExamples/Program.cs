using System;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet2.UtilitiesExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //ProcessHelperExample.ExecBatCommand();
                //FileHelperExample.CopyLocalBigFile();
                // ObjectIdExample.Demo();
                ConsoleApplicationHelper.DetectShutdown(() =>
                {
                    Console.WriteLine("DetectShutdown");
                });
                ConsoleApplicationHelper.DetectKeyPress(input =>
                {
                    Console.WriteLine("input:" + input);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}