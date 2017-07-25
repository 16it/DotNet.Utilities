using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using System;
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
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;
                return base.Scope();
            }
            catch (Exception)
            {
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}