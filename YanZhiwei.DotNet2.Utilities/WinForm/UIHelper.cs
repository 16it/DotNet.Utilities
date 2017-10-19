namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// WinForm UI帮助类
    /// </summary>
    public static class UIHelper
    {
        #region Methods

        /// <summary>
        /// 控件线程安全
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="code">委托</param>
        public static void UIThread(this Control control, MethodInvoker code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }

            code.Invoke();
        }

        /// <summary>
        /// 控件先线程安全
        /// 参考:http://www.codeproject.com/Articles/37413/A-Generic-Method-for-Cross-thread-Winforms-Access#xx3867544xx
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="cont">Control</param>
        /// <param name="updateUIFactory">委托</param>
        public static void UIThread<T>(this T cont, Action<T> updateUIFactory)
        where T : Control
        {
            if (cont.InvokeRequired)
            {
                cont.Invoke(new Action<T, Action<T>>(UIThread), new object[] { cont, updateUIFactory });
            }
            else
            {
                updateUIFactory(cont);
            }
        }

        /// <summary>
        /// 异步线程安全更新UI
        /// </summary>
        /// <typeparam name="T">Control</typeparam>
        /// <param name="cont">Control</param>
        /// <param name="updateUIFactory">委托</param>
        public static void UIBeginThread<T>(this T cont, Action<T> updateUIFactory)
             where T : Control
        {
            if (cont.InvokeRequired)
            {
                cont.BeginInvoke(new Action<T, Action<T>>(UIBeginThread), new object[] { cont, updateUIFactory });
            }
            else
            {
                updateUIFactory(cont);
            }
        }

        #endregion Methods
    }
}