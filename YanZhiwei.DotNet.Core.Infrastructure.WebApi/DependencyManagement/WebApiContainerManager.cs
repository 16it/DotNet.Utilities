using Autofac;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure.WebApi.DependencyManagement
{
    /// <summary>
    /// WebApiContainer
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement.ContainerManager" />
    public class WebApiContainerManager : ContainerManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiContainerManager"/> class.
        /// </summary>
        /// <param name="container">IContainer</param>
        public WebApiContainerManager(IContainer container) : base(container)
        {
        }
    }
}