using Autofac;
using Autofac.Integration.Mvc;
using System.Web;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure.Mvc
{
    /// <summary>
    /// Mvc ContainerManager
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement.ContainerManager" />
    public sealed class MvcContainerManager : ContainerManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">IContainer</param>
        public MvcContainerManager(IContainer container) : base(container)
        {
        }

        /// <summary>
        /// 获取生命周期
        /// </summary>
        /// <returns>
        /// Scope
        /// </returns>
        public override ILifetimeScope Scope()
        {
            if (HttpContext.Current != null) //mvc情况
                return AutofacDependencyResolver.Current.RequestLifetimeScope;
            return base.Scope();
        }
    }
}