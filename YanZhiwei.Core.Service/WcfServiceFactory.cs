using YanZhiwei.DotNet3._5.Utilities.Common;

namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// 通过Wcf提供服务
    /// </summary>
    public class WcfServiceFactory : ServiceFactory
    {
        public override T CreateService<T>()
        {
            //TODO
            //需实现WCF Uri来自配置文件
            var uri = string.Empty;
            var proxy = WcfServiceProxy.CreateServiceProxy<T>(uri);
            return proxy;
        }
    }
}