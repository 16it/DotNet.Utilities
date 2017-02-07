using Castle.DynamicProxy;
using System;
using YanZhiwei.DotNet2.Utilities.ExtendException;

namespace YanZhiwei.DotNet.Core.Service
{
    internal class InvokeInterceptor : IInterceptor
    {
        public InvokeInterceptor()
        {
        }
        
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch(Exception exception)
            {
                if(exception is BusinessException)
                    throw;
                    
                var message = new
                {
                    exception = exception.Message,
                    exceptionContext = new
                    {
                        method = invocation.Method.ToString(),
                        arguments = invocation.Arguments,
                        returnValue = invocation.ReturnValue
                    }
                };
                //   Log4NetHelper.Error(LoggerType.ServiceExceptionLog, message, exception);
                throw;
            }
        }
    }
}