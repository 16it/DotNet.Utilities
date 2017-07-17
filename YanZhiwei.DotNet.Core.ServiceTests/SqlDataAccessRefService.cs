using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.Service;

namespace YanZhiwei.DotNet.Core.ServiceTests
{
    public class SqlDataAccessRefService : RefService
    {
        public override T CreateService<T, F>()
        {
            string _interfaceName = typeof(T).Name;
            return CacheHelper.Get<T>(string.Format("Service_{0}", _interfaceName), () =>
            {
                return base.CreateService<T, F>();
            });
        }
    }
}