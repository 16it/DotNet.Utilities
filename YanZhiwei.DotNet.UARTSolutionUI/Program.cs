using System;
using System.Windows.Forms;
using YanZhiwei.DotNet.UARTSolution;
using YanZhiwei.DotNet2.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet.UARTSolutionUI
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
                        ServiceContext.Current.LogException(ex, "发生未捕获的异常:" + ex.Message);
                        break;
                        
                    case ExceptionType.Thread:
                        ServiceContext.Current.LogException(ex, "发生线程异常:" + ex.Message);
                        break;
                }
            });
            ApplicationHelper.ApplyOnlyOneInstance(t =>
            {
                if(t)
                {
                    ServiceContext.Current.LogInfo("程序启动");
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