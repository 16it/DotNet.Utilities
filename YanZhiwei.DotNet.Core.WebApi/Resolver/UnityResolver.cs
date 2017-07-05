using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet.Core.WebApi.Resolver
{
    /// <summary>
    /// 基于Unity的依赖关系的注入容器
    /// </summary>
    /// <seealso cref="System.Web.Http.Dependencies.IDependencyResolver" />
    public class UnityResolver : IDependencyResolver
    {
        /// <summary>
        /// 容器
        /// </summary>
        protected readonly IUnityContainer container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">IUnityContainer</param>
        public UnityResolver(IUnityContainer container)
        {
            ValidateOperator.Begin().NotNull(container, "container");
            this.container = container;
        }

        /// <summary>
        /// 从范围中检索服务。
        /// </summary>
        /// <param name="serviceType">要检索的服务。</param>
        /// <returns>
        /// 检索到的服务。
        /// </returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return container.IsRegistered(serviceType) == false ? null : container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// 从范围中检索服务集合。
        /// </summary>
        /// <param name="serviceType">要检索的服务集合。</param>
        /// <returns>
        /// 检索到的服务集合。
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// 开始解析范围。
        /// </summary>
        /// <returns>
        /// 依赖范围。
        /// </returns>
        public IDependencyScope BeginScope()
        {
            var _child = container.CreateChildContainer();
            return new UnityResolver(_child);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }
    }
}