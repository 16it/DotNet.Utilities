using Castle.DynamicProxy;

namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// ServiceFactory
    /// </summary>
    public interface IServiceBase
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <returns>类型</returns>
        T CreateService<T, F>() where T : class
                                where F : IInterceptor, new();
    }
}