using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet3._5.Utilities.Common;

namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// 直接引用提供服务
    /// </summary>
    public class RefServiceFactory : ServiceFactory
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:51
        /// 备注：
        public override T CreateService<T, F>()
        {
            //第一次通过反射创建服务实例，然后缓存住
            string _interfaceName = typeof(T).Name;
            return CacheHelper.Get<T>(string.Format("Service_{0}", _interfaceName), () =>
            {
                return AssemblyHelper.FindTypeByInterface<T>();
            });
        }
    }
}