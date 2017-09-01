namespace YanZhiwei.DotNet.Core.Log
{
    using log4net;
    using System;

    /// <summary>
    /// Ms Sql Server 日志服务
    /// </summary>
    public sealed class SQLServerLogService : LogServiceBase, ILogService
    {
        #region Fields

        private static readonly ILog Logger = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        static SQLServerLogService()
        {
            Logger = LogManager.GetLogger("MsSqlServerLogger");
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Debug<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Debug(_logDataJsonString);
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        public void Debug<T>(T logData, Exception ex)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Debug(_logDataJsonString, ex);
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Debug(string message)
        {
            if (Logger.IsDebugEnabled)
                Logger.Debug(message);
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Debug(string message, Exception ex)
        {
            if (Logger.IsDebugEnabled)
                Logger.Debug(message, ex);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Error<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Error(_logDataJsonString);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        public void Error<T>(T logData, Exception ex)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Error(_logDataJsonString, ex);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Error(string message)
        {
            if (Logger.IsErrorEnabled)
                Logger.Error(message);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Error(string message, Exception ex)
        {
            if (Logger.IsErrorEnabled)
                Logger.Error(message, ex);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Fatal(string message)
        {
            if (Logger.IsFatalEnabled)
                Logger.Fatal(message);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Fatal<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Fatal(_logDataJsonString);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        public void Fatal<T>(T logData, Exception ex)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Fatal(_logDataJsonString, ex);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Fatal(string message, Exception ex)
        {
            if (Logger.IsFatalEnabled)
                Logger.Fatal(message, ex);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Info<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Info(_logDataJsonString);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        public void Info<T>(T logData, Exception ex)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Info(_logDataJsonString, ex);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Info(string message)
        {
            if (Logger.IsInfoEnabled)
                Logger.Info(message);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Info(string message, Exception ex)
        {
            if (Logger.IsInfoEnabled)
                Logger.Info(message, ex);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Warn<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Warn(_logDataJsonString);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        public void Warn<T>(T logData, Exception ex)
            where T : class
        {
            string _logDataJsonString = GetLogDataJson(logData);
            Warn(_logDataJsonString, ex);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Warn(string message)
        {
            if (Logger.IsWarnEnabled)
                Logger.Warn(message);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Warn(string message, Exception ex)
        {
            if (Logger.IsWarnEnabled)
                Logger.Warn(message, ex);
        }

        #endregion Methods
    }
}