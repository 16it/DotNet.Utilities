namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    using System;
    using System.Windows.Forms;
    
    /// <summary>
    /// 涉及窗体的帮助类
    /// </summary>
    /// 日期：2015-10-12 10:15
    /// 备注：
    public static class FormHelper
    {
        #region Methods
        
        /// <summary>
        /// 关闭登陆窗口，有别于hide方式，是直接close方式；
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="loginForm">登陆窗口</param>
        /// 日期：2015-10-12 10:04
        /// 备注：
        public static void CloseLoginForm<T>(this T loginForm)
        where T : Form, new()
        {
            if(loginForm != null)
            {
                loginForm.DialogResult = DialogResult.OK;
                loginForm.Close();
            }
        }
        
        /// <summary>
        /// 弹出模式对话框
        /// </summary>
        /// <typeparam name="T">窗体泛型</typeparam>
        /// <typeparam name="P">参数泛型</typeparam>
        /// <param name="paramter">参数</param>
        public static void ShowDialogForm<T, P>(P paramter)
        where T : Form, new()
        where P : class
        {
            T _winForm = (T)Activator.CreateInstance(typeof(T), paramter);
            _winForm.ShowDialog();
        }
        
        /// <summary>
        /// 弹出模式对话框
        /// </summary>
        /// <typeparam name="T">窗体泛型</typeparam>
        /// <typeparam name="P1">传参泛型</typeparam>
        /// <typeparam name="P2">传参泛型</typeparam>
        /// <param name="paramter1">参数1</param>
        /// <param name="paramter2">参数2</param>
        public static void ShowDialogForm<T, P1, P2>(P1 paramter1, P2 paramter2)
        where T : Form, new()
        {
            T _winForm = (T)Activator.CreateInstance(typeof(T), paramter1, paramter2);
            _winForm.ShowDialog();
        }
        
        /// <summary>
        /// 弹出模式对话框
        /// </summary>
        /// <typeparam name="T">窗体泛型</typeparam>
        public static void ShowDialogForm<T>()
        where T : Form, new()
        {
            T _winForm = new T();
            _winForm.ShowDialog();
        }
        
        /// <summary>
        /// 设置应用登陆界面
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="U">泛型</typeparam>
        /// 日期：2015-10-12 9:56
        /// 备注：
        public static void ShowLoginForm<T, U>()
        where T : Form, new()
        where U : Form, new()
        {
            ShowLoginForm<T, U>(null);
        }
        
        /// <summary>
        /// 设置应用登陆界面
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="U">泛型</typeparam>
        /// <param name="mainFormFactory">委托，参数主界面对象</param>
        /// 日期：2015-10-12 9:55
        /// 备注：
        public static void ShowLoginForm<T, U>(Action<U> mainFormFactory)
        where T : Form, new()
        where U : Form, new()
        {
            using(T winlogin = new T())
            {
                if(winlogin.ShowDialog() == DialogResult.OK)
                {
                    U _winMain = new U();
                    
                    if(mainFormFactory != null)
                    {
                        mainFormFactory(_winMain);
                    }
                    
                    Application.Run(_winMain);
                }
            }
        }
        
        #endregion Methods
    }
}