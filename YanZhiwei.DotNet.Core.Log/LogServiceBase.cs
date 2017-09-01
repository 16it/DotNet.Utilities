using log4net.Config;
using System.IO;
using System.Text;
using YanZhiwei.DotNet.Core.Config;
using YanZhiwei.DotNet.Newtonsoft.Json.Utilities;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet.Core.Log
{
    /// <summary>
    /// Log 服务抽象类
    /// </summary>
    public abstract class LogServiceBase
    {
        static LogServiceBase()
        {
            string _log4NetXmlConfg = CachedConfigContext.Instance.ConfigService.GetConfig("Log4net");
            ValidateOperator.Begin().NotNullOrEmpty(_log4NetXmlConfg, "log4net配置文件");

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_log4NetXmlConfg)))
            {
                XmlConfigurator.Configure(ms);
            }
        }

        /// <summary>
        /// 获取日志对象Json字符串
        /// </summary>
        /// <typeparam name="T">日志对象</typeparam>
        /// <param name="logData">日志对象</param>
        /// <returns>Json字符串</returns>
        protected virtual string GetLogDataJson<T>(T logData)
           where T : class
        {
            string _jsonString = string.Empty;

            if (logData != null)
                _jsonString = JsonHelper.Serialize(logData);

            return _jsonString;
        }
    }
}