using Microsoft.Practices.Unity;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace YanZhiwei.DotNet.Core.WebApi.Infrastructure
{
    /// <summary>
    /// 以将IoC与ASP.NET Web API的HttpController激活系统进行集成最为直接的方式为自定义一个HttpControllerActivator。
    /// </summary>
    /// <seealso cref="System.Web.Http.Dispatcher.IHttpControllerActivator" />
    public class UnityHttpControllerActivator : IHttpControllerActivator

    {
        /// <summary>
        /// IUnityContainer
        /// </summary>
        public readonly IUnityContainer Container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">IUnityContainer</param>
        public UnityHttpControllerActivator(IUnityContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// 创建一个 <see cref="T:System.Web.Http.Controllers.IHttpController" /> 对象。
        /// </summary>
        /// <param name="request">消息请求。</param>
        /// <param name="controllerDescriptor">HTTP 控制器描述符。</param>
        /// <param name="controllerType">控制器的类型。</param>
        /// <returns>
        ///   <see cref="T:System.Web.Http.Controllers.IHttpController" /> 对象。
        /// </returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController)Container.Resolve(controllerType);
        }
    }
}