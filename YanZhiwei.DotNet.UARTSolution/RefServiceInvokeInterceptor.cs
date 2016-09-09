using Castle.DynamicProxy;
using System;
using YanZhiwei.DotNet.Core.Log;
using YanZhiwei.DotNet2.Utilities.Exception;

namespace YanZhiwei.DotNet.UARTSolution
{
    /// <summary>
    /// 用于REF方式方法拦截
    /// </summary>
    internal class RefServiceInvokeInterceptor : IInterceptor
    {
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public RefServiceInvokeInterceptor()
        {
        }
        
        #endregion Constructors
        
        #region Methods
        
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
                    
                var _message = new
                {
                    exception = exception.Message,
                    exceptionContext = new
                    {
                        method = invocation.Method.ToString(),
                        arguments = invocation.Arguments,
                        returnValue = invocation.ReturnValue
                    }
                };
                Log4NetHelper.Error(LoggerType.ServiceExceptionLog, _message, exception);
                throw;
            }
        }
        
        #endregion Methods
    }
}