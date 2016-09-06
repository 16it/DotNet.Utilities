using Castle.DynamicProxy;

namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// ServiceFactory
    /// </summary>
    public abstract class ServiceFactory
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <returns>类型</returns>
        public abstract T CreateService<T, F>()
        where T : class
            where F : IInterceptor, new();
    }
}