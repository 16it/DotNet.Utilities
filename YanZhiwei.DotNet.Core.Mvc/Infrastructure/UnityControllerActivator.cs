using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace YanZhiwei.DotNet.Core.Mvc.Infrastructure
{
    /// <summary>
    /// UnityControllerActivator
    /// </summary>
    /// <seealso cref="System.Web.Mvc.IControllerActivator" />
    public sealed class UnityControllerActivator : IControllerActivator
    {
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityControllerActivator"/> class.
        /// </summary>
        /// <param name="container">IUnityContainer</param>
        public UnityControllerActivator(IUnityContainer container)
        {
            unityContainer = container;
        }

        /// <summary>
        /// 在类中实现时创建控制器。
        /// </summary>
        /// <param name="requestContext">请求上下文。</param>
        /// <param name="controllerType">控制器类型。</param>
        /// <returns>
        /// 创建的控制器。
        /// </returns>
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return (IController)unityContainer.Resolve(controllerType);
        }
    }
}