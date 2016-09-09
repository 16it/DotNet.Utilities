using System;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.Log;
using YanZhiwei.DotNet.UARTSolution.Core;

namespace YanZhiwei.DotNet.UARTSolution
{
    /// <summary>
    /// 业务服务上下文
    /// </summary>
    public class ServiceContext
    {
        /// <summary>
        /// ServiceContext
        /// </summary>
        public static ServiceContext Current
        {
            get
            {
                return CacheHelper.GetItem<ServiceContext>("ServiceContext", () => new ServiceContext());
            }
        }
        
        /// <summary>
        /// 获取AccountService
        /// </summary>
        public IPacketDataService PacketDataService
        {
            get
            {
                return ProjectSerivce.RefService.CreateService<IPacketDataService, RefServiceInvokeInterceptor>();
            }
        }
        
        public virtual void LogException(Exception exception, string mssage)
        {
            var message = new
            {
                exception = exception.Message,
                exceptionContext = mssage,
            };
            Log4NetHelper.Error(LoggerType.ServiceExceptionLog, message, exception);
        }
        
        public virtual void LogInfo(string mssage)
        {
            Log4NetHelper.Error(LoggerType.ServiceExceptionLog, mssage);
        }
    }
}