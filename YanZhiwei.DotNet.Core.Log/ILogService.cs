using System;

namespace YanZhiwei.DotNet.Core.Log
{
    /// <summary>
    /// 日志记录接口
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Fatal(string message);

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        void Fatal<T>(T logData) where T : class;

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        void Fatal<T>(T logData, Exception ex) where T : class;

        /// <summary>
        /// Fatal记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Fatal(string message, Exception ex);

        /// <summary>
        /// Error记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        void Error<T>(T logData) where T : class;

        /// <summary>
        /// Error记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        void Error<T>(T logData, Exception ex) where T : class;

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Error(string message);

        /// <summary>
        /// Error记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Error(string message, Exception ex);

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        void Warn<T>(T logData) where T : class;

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        void Warn<T>(T logData, Exception ex) where T : class;

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Warn(string message);

        /// <summary>
        /// Warn记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Warn(string message, Exception ex);

        /// <summary>
        /// Info记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        void Info<T>(T logData) where T : class;

        /// <summary>
        /// Info记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        void Info<T>(T logData, Exception ex) where T : class;

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Info(string message);

        /// <summary>
        /// Info记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Info(string message, Exception ex);

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        void Debug<T>(T logData) where T : class;

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <typeparam name="T">记录日志类型</typeparam>
        /// <param name="logData">日志数据</param>
        /// <param name="ex">异常信息</param>
        void Debug<T>(T logData, Exception ex) where T : class;

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        void Debug(string message);

        /// <summary>
        /// Debug记录
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="ex">异常信息</param>
        void Debug(string message, Exception ex);
    }
}