using Autofac;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure.WebApi.DependencyManagement
{
    public class WebApiContainerManager : ContainerManager
    {
        public WebApiContainerManager(IContainer container) : base(container)
        {
        }
    }
}