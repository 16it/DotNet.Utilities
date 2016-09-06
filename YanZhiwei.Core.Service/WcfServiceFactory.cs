using YanZhiwei.DotNet3._5.Utilities.Common;

namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// 通过Wcf提供服务
    /// </summary>
    public class WcfServiceFactory : ServiceFactory
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public override T CreateService<T, F>()
        {
            var uri = string.Empty;
            var proxy = WcfServiceProxy.CreateServiceProxy<T>(uri);
            return proxy;
        }
    }
}