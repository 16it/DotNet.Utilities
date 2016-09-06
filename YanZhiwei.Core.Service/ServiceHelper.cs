using Castle.DynamicProxy;

namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// ServiceHelper
    /// </summary>
    public class ServiceHelper
    {
        /// <summary>
        /// 默认引用服务方式
        /// </summary>
        public ServiceHelper() : this(new RefServiceFactory())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service">ServiceFactory</param>
        public ServiceHelper(ServiceFactory service)
        {
            serviceFactory = service;
        }

        /// <summary>
        /// 暂时使用引用服务方式，可以改造成注入，或使用WCF服务方式
        /// </summary>
        private ServiceFactory serviceFactory
        {
            get;
            set;
        }

        /// <summary>
        /// 创建服务根据BLL接口
        /// </summary>
        public T CreateService<T, F>()
        where T : class
            where F : IInterceptor, new()
        {
            var _service = serviceFactory.CreateService<T, F>();
            ProxyGenerator _generator = new ProxyGenerator();
            T _dynamicProxy = _generator.CreateInterfaceProxyWithTargetInterface<T>(
                                  _service, new F());
            return _dynamicProxy;
        }
    }
}