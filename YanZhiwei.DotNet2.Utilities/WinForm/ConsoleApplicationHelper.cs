using System;
using System.Runtime.InteropServices;

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

        #region 控制台应用程序关闭时的消息捕获

        /// <summary>
        /// 当用户关闭Console时，系统会发送次消息
        /// </summary>
        /// 时间：2016-05-23 11:13
        /// 备注：
        private const int CTRL_CLOSE_EVENT = 2;

        /// <summary>
        /// 控制台控制委托
        /// </summary>
        /// <param name="dwCtrlType">Type of the dw control.</param>
        /// <returns></returns>
        /// 时间：2016-05-23 11:13
        /// 备注：
        public delegate bool ConsoleCtrlHanlder(int dwCtrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHanlder detectShutdown, bool Add);

        #endregion 控制台应用程序关闭时的消息捕获

        /// <summary>
        /// 监听用户关闭控制台应用程序
        /// </summary>
        /// <param name="shutDownConsoleFactory">委托</param>
        /// 时间：2016-05-23 13:09
        /// 备注：
        public static void DetectShutdown(Action shutDownConsoleFactory)
        {
            ConsoleCtrlHanlder _shutDownHanlder = new ConsoleCtrlHanlder(ctrlType =>
            {
                switch (ctrlType)
                {
                    case CTRL_CLOSE_EVENT:
                        shutDownConsoleFactory();
                        break;
                }

                return false;
            });
            SetConsoleCtrlHandler(_shutDownHanlder, true);
        }
    }
}