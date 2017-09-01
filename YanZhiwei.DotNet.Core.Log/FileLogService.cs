namespace YanZhiwei.DotNet.Core.Log
{
    using System;
    using System.IO;
    using System.Text;

    using log4net;
    using log4net.Config;

    using YanZhiwei.DotNet.Core.Config;
    using YanZhiwei.DotNet.Newtonsoft.Json.Utilities;
    using YanZhiwei.DotNet2.Utilities.Operator;

    /// <summary>
    /// 基于Log4Net的文件日志记录
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Log.ILogService" />
    public sealed class FileLogService : ILogService
    {
        #region Fields

        /// <summary>
        /// The debug logger name
        /// </summary>
        public const string DEBUGLoggerName = "DEBUG_FileLogger";

        /// <summary>
        /// The error logger name
        /// </summary>
        public const string ERRORLoggerName = "ERROR_FileLogger";

        /// <summary>
        /// The fatal logger name
        /// </summary>
        public const string FATALLoggerName = "FATAL_FileLogger";

        /// <summary>
        /// The information logger name
        /// </summary>
        public const string INFOLoggerName = "INFO_FileLogger";

        /// <summary>
        /// The warn logger name
        /// </summary>
        public const string WARNLoggerName = "WARN_FileLogger";

        /// <summary>
        /// The debug logger
        /// </summary>
        public static readonly ILog DebugLogger = null;

        /// <summary>
        /// The error logger
        /// </summary>
        public static readonly ILog ERRORLogger = null;

        /// <summary>
        /// The fatal logger
        /// </summary>
        public static readonly ILog FATALLogger = null;

        /// <summary>
        /// The information logger
        /// </summary>
        public static readonly ILog INFOLogger = null;

        /// <summary>
        /// The warn logger
        /// </summary>
        public static readonly ILog WARNLogger = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="FileLogService"/> class.
        /// </summary>
        static FileLogService()
        {
            string _log4NetXmlConfg = CachedConfigContext.Instance.ConfigService.GetConfig("Log4net");
            ValidateOperator.Begin().NotNullOrEmpty(_log4NetXmlConfg, "log4net配置文件");

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_log4NetXmlConfg)))
            {
                XmlConfigurator.Configure(ms);
            }

            DebugLogger = LogManager.GetLogger(DEBUGLoggerName);
            INFOLogger = LogManager.GetLogger(INFOLoggerName);
            WARNLogger = LogManager.GetLogger(WARNLoggerName);
            ERRORLogger = LogManager.GetLogger(ERRORLoggerName);
            FATALLogger = LogManager.GetLogger(FATALLoggerName);
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
            string _logDataJsonString = GettLogDataJson(logData);
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
            string _logDataJsonString = GettLogDataJson(logData);
            Debug(_logDataJsonString, ex);
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Debug(string message)
        {
            if (DebugLogger.IsDebugEnabled)
                DebugLogger.Debug(message);
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Debug(string message, Exception ex)
        {
            if (DebugLogger.IsDebugEnabled)
                DebugLogger.Debug(message, ex);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Error<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GettLogDataJson(logData);
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
            string _logDataJsonString = GettLogDataJson(logData);
            Error(_logDataJsonString, ex);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Error(string message)
        {
            if (ERRORLogger.IsErrorEnabled)
                ERRORLogger.Error(message);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Error(string message, Exception ex)
        {
            if (ERRORLogger.IsErrorEnabled)
                ERRORLogger.Error(message, ex);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Fatal(string message)
        {
            if (FATALLogger.IsFatalEnabled)
                FATALLogger.Fatal(message);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Fatal<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GettLogDataJson(logData);
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
            string _logDataJsonString = GettLogDataJson(logData);
            Fatal(_logDataJsonString, ex);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Fatal(string message, Exception ex)
        {
            if (FATALLogger.IsFatalEnabled)
                FATALLogger.Fatal(message, ex);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Info<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GettLogDataJson(logData);
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
            string _logDataJsonString = GettLogDataJson(logData);
            Info(_logDataJsonString, ex);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Info(string message)
        {
            if (INFOLogger.IsInfoEnabled)
                INFOLogger.Info(message);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Info(string message, Exception ex)
        {
            if (INFOLogger.IsInfoEnabled)
                INFOLogger.Info(message, ex);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        public void Warn<T>(T logData)
            where T : class
        {
            string _logDataJsonString = GettLogDataJson(logData);
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
            string _logDataJsonString = GettLogDataJson(logData);
            Warn(_logDataJsonString, ex);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Warn(string message)
        {
            if (WARNLogger.IsWarnEnabled)
                WARNLogger.Warn(message);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        public void Warn(string message, Exception ex)
        {
            if (WARNLogger.IsWarnEnabled)
                WARNLogger.Warn(message, ex);
        }

        private string GettLogDataJson<T>(T logData)
            where T : class
        {
            string _jsonString = string.Empty;

            try
            {
                if (logData != null)
                    _jsonString = JsonHelper.Serialize(logData);
            }
            catch (Exception ex)
            {
                Error(string.Format("处理日志的时候，对象:【{0}】序列化发生错误.", typeof(T).FullName), ex);
                _jsonString = string.Empty;
            }

            return _jsonString;
        }

        #endregion Methods
    }
}