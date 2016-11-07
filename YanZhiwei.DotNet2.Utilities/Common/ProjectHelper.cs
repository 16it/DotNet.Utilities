namespace YanZhiwei.DotNet2.Utilities.Common
{
    using Enum;
    using Operator;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using WinForm;
    
    /// <summary>
    /// 项目帮助文件
    /// </summary>
    public class ProjectHelper
    {
        #region Methods
        
        /// <summary>
        /// 获取执行文件夹路径
        /// <para>eg:c:\\users\\yanzhiwei\\documents\\visual studio 2015\\Projects\\WebApplication2\\WebApplication2\\</para>
        /// </summary>
        /// <returns>执行文件夹路径</returns>
        public static string GetExecuteDirectory()
        {
            string _path = string.Empty;
            ProgramMode _mode = GetExecutionContext();
            
            switch(_mode)
            {
                case ProgramMode.WebForm:
                    _path = AppDomain.CurrentDomain.BaseDirectory.ToString();
                    break;
                    
                case ProgramMode.WinForm:
                    _path = ApplicationHelper.GetExecuteDirectory();
                    break;
            }
            
            return _path;
        }
        
        /// <summary>
        /// 获取程序执行上下文
        /// </summary>
        /// <returns>程序执行上下文</returns>
        public static ProgramMode GetExecutionContext()
        {
            if(Assembly.GetEntryAssembly() != null)
            {
                return ProgramMode.WinForm;
            }
            
            return ProgramMode.WebForm;
        }
        
        /// <summary>
        /// 加载非托管DLL
        /// </summary>
        /// <param name="lpLibFileName">DLL路径</param>
        /// <returns>IntPtr</returns>
        /// 时间：2016/11/7 13:33
        /// 备注：
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        public static extern IntPtr LoadLibrary(string lpLibFileName);
        
        /// <summary>
        /// 加载非托管DLL
        /// </summary>
        /// <param name="dllPath">DLL路径</param>
        /// <returns>加载是否成功</returns>
        /// 时间：2016/11/7 13:35
        /// 备注：
        public static bool LoadUnmanagedDLL(string dllPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(dllPath, "非托管DLL路径").IsFilePath(dllPath).CheckFileExists(dllPath);
            IntPtr _dllAddr = LoadLibrary(dllPath);
            return _dllAddr == IntPtr.Zero;
        }
        
        #endregion Methods
    }
}