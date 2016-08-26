using System;
using System.Windows.Forms;
using YanZhiwei.DotNet.Log4Net.Utilities;
using YanZhiwei.DotNet2.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet.SerialPortTemplate
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationHelper.CapturedException((ex, mode) =>
            {
                switch(mode)
                {
                    case ExceptionType.Unhandled:
                        Log4NetHelper.WriteFatal("发生未捕获的异常:" + ex.Message);
                        break;

                    case ExceptionType.Thread:
                        Log4NetHelper.WriteFatal("发生线程异常:" + ex.Message);
                        break;
                }
            });
            ApplicationHelper.ApplyOnlyOneInstance(t =>
            {
                if(t)
                {
                    Log4NetHelper.WriteInfo("启动程序");
                    Application.Run(new FormMain());
                }
                else
                {
                    MessageHelper.ShowError("已经有程序在运行。");
                }
            });
        }
    }
}