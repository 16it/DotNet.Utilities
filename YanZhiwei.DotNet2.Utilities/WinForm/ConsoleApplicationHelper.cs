using System;

namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    /// <summary>
    /// 控制台应用程序辅助类
    /// </summary>
    /// 时间：2016-05-23 11:03
    /// 备注：
    public class ConsoleApplicationHelper
    {
        /// <summary>
        /// 监听控制台输入
        /// </summary>
        /// <param name="inputFactory">委托，参数：输入字符串</param>
        /// 时间：2016-05-23 11:05
        /// 备注：
        public static void DetectKeyPress(Action<string> inputFactory)
        {
            do
            {
                while (!Console.KeyAvailable)
                {
                    string _input = Console.ReadLine();
                    if (inputFactory != null)
                        inputFactory(_input);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}