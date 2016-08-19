namespace YanZhiwei.DotNet.Core.Log
{
    using Config;
    using DotNet2.Utilities.Common;
    using log4net;
    using log4net.Config;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Log4Net日志记录
    /// </summary>
    public class Log4NetHelper
    {
        #region Constructors

        static Log4NetHelper()
        {
            string _log4NetXmlConfg = CachedConfigContext.Current.ConfigService.GetConfig("Log4net");
            ValidateHelper.Begin().NotNullOrEmpty(_log4NetXmlConfg, "log4net配置文件");
            //using(MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(_log4NetXmlConfg)))
            //{
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_log4NetXmlConfg));
            XmlConfigurator.Configure(ms);
            // }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        /// <param name="ex">Exception</param>
        public static void Debug(LoggerType loggerType, object message, Exception ex)
        {
            ILog _logger = LogManager.GetLogger(loggerType.ToString());

            if(ex != null)
                _logger.Debug(SerializeObject(message), ex);
            else
                _logger.Debug(SerializeObject(message));
        }

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        public static void Debug(LoggerType loggerType, object message)
        {
            Debug(loggerType, message, null);
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        /// <param name="ex">Exception</param>
        public static void Error(LoggerType loggerType, object message, Exception ex)
        {
            var _logger = LogManager.GetLogger(loggerType.ToString());

            if(ex != null)
                _logger.Error(SerializeObject(message), ex);
            else
                _logger.Error(SerializeObject(message));
        }

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        public static void Error(LoggerType loggerType, object message)
        {
            Error(loggerType, message, null);
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        /// <param name="ex">Exception</param>
        public static void Fatal(LoggerType loggerType, object message, Exception ex)
        {
            var _logger = LogManager.GetLogger(loggerType.ToString());

            if(ex != null)
                _logger.Fatal(SerializeObject(message), ex);
            else
                _logger.Fatal(SerializeObject(message));
        }

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        public static void Fatal(LoggerType loggerType, object message)
        {
            Fatal(loggerType, message, null);
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        /// <param name="ex">Exception</param>
        public static void Info(LoggerType loggerType, object message, Exception ex)
        {
            var _logger = LogManager.GetLogger(loggerType.ToString());

            if(ex != null)
                _logger.Info(SerializeObject(message), ex);
            else
                _logger.Info(SerializeObject(message));
        }

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        public static void Info(LoggerType loggerType, object message)
        {
            Info(loggerType, message, null);
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        /// <param name="ex">Exception</param>
        public static void Warn(LoggerType loggerType, object message, Exception ex)
        {
            var _logger = LogManager.GetLogger(loggerType.ToString());

            if(ex != null)
                _logger.Warn(SerializeObject(message), ex);
            else
                _logger.Warn(SerializeObject(message));
        }

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="loggerType">日志类型</param>
        /// <param name="message">日志内容</param>
        public static void Warn(LoggerType loggerType, object message)
        {
            Warn(loggerType, message, null);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Json字符串</returns>
        private static object SerializeObject(object message)
        {
            if(message is string || message == null)
            {
                return message;
            }
            else
            {
                return Newtonsoft.Json.Utilities.JsonHelper.Serialize(message);
            }
        }

        #endregion Methods
    }
}