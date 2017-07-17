using Castle.DynamicProxy;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.Service;
using YanZhiwei.DotNet3._5.Utilities.Service;

namespace YanZhiwei.DotNet.Core.ServiceTests
{
    public class SqlDataAccessRefService : RefService, IServiceBase
    {
        public T CreateService<T, F>()
            where T : class
            where F : IInterceptor, new()
        {
            string _interfaceName = typeof(T).Name;
            return CacheHelper.Get<T>(string.Format("Service_{0}", _interfaceName), () =>
            {
                return base.CreateService<T>();
            });
        }
    }
}