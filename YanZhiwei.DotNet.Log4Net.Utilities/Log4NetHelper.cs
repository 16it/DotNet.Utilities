namespace YanZhiwei.DotNet.Log4Net.Utilities
{
    using System;

    using log4net;

    /// <summary>
    /// Log4Net帮助类
    /// </summary>
    public class Log4NetHelper
    {
        #region Fields

        private static ILog log = null;
        private static string logger = "FileLogger";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Log4NetHelper()
        {
            log = LogManager.GetLogger(logger);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///设置日志记录Logger
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        public static void SetLogger(string loggerName)
        {
            logger = loggerName;
            log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// 写入DEBUG信息
        /// </summary>
        /// <param name="debug">DEBUG信息</param>
        /// <param name="ex">Exception</param>
        public static void WriteDebug(string debug, Exception ex)
        {
            if(log.IsDebugEnabled)
            {
                log.Debug(debug, ex);
            }
        }

        /// <summary>
        ///写入DEBUG信息
        /// </summary>
        /// <param name="debug">DEBUG信息</param>
        public static void WriteDebug(string debug)
        {
            if(log.IsDebugEnabled)
            {
                log.Debug(debug);
            }
        }

        /// <summary>
        /// 写入Error信息
        /// </summary>
        /// <param name="error">Error信息</param>
        public static void WriteError(string error)
        {
            if(log.IsErrorEnabled)
            {
                log.Error(error);
            }
        }

        /// <summary>
        /// 写入Error信息
        /// </summary>
        /// <param name="Error">Error信息</param>
        /// <param name="ex">Exception</param>
        public static void WriteError(string Error, Exception ex)
        {
            if(log.IsErrorEnabled)
            {
                log.Error(Error, ex);
            }
        }

        /// <summary>
        /// 写入Fatal信息
        /// </summary>
        /// <param name="fatal">Fatal信息</param>
        public static void WriteFatal(string fatal)
        {
            if(log.IsFatalEnabled)
            {
                log.Fatal(fatal);
            }
        }

        /// <summary>
        ///  写入Fatal信息
        /// </summary>
        /// <param name="fatal">Fatal信息</param>
        /// <param name="ex">Exception</param>
        public static void WriteFatal(string fatal, Exception ex)
        {
            if(log.IsFatalEnabled)
            {
                log.Fatal(fatal, ex);
            }
        }

        /// <summary>
        /// 写入Info信息
        /// </summary>
        /// <param name="info">Info信息</param>
        public static void WriteInfo(string info)
        {
            if(log.IsInfoEnabled)
            {
                log.Info(info);
            }
        }

        /// <summary>
        /// 写入Info信息
        /// </summary>
        /// <param name="info">Info信息</param>
        /// <param name="ex">Exception</param>
        public static void WriteInfo(string info, Exception ex)
        {
            if(log.IsInfoEnabled)
            {
                log.Info(info, ex);
            }
        }

        /// <summary>
        /// 写入Info信息
        /// </summary>
        /// <param name="warn">Info信息</param>
        /// <param name="ex">Exception</param>
        public static void WriteWarn(string warn, Exception ex)
        {
            if(log.IsWarnEnabled)
            {
                log.Warn(warn, ex);
            }
        }

        /// <summary>
        /// 写入Info信息
        /// </summary>
        /// <param name="warn">Info信息</param>
        public static void WriteWarn(string warn)
        {
            if(log.IsWarnEnabled)
            {
                log.Warn(warn);
            }
        }

        #endregion Methods
    }
}