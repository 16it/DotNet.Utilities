using Castle.DynamicProxy;
using System;
using System.Diagnostics;
using YanZhiwei.DotNet2.Utilities.ExtendException;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet.Core.ServiceTests
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
                Debug.WriteLine(DateTime.Now.FormatDate(1) + " Proceed");
            }
            catch (Exception exception)
            {
                if (exception is BusinessException)
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
                throw;
            }
        }
    }
}