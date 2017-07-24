using Microsoft.Practices.Unity;
using System;

namespace YanZhiwei.DotNet.Core.Service.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Unity容器管理
    /// </summary>
    public class ContainerManager
    {
        private readonly UnityContainer _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">UnityContainer</param>
        public ContainerManager(UnityContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// IUnityContainer
        /// </summary>
        public virtual IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        public virtual object GetService(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        /// <summary>
        ///解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overrides">The overrides.</param>
        /// <returns></returns>
        public virtual T GetService<T>(params ParameterOverride[] overrides) where T : class
        {
            return Container.Resolve<T>(overrides);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="overrides">The overrides.</param>
        /// <returns></returns>
        public virtual T GetService<T>(string name, params ParameterOverride[] overrides) where T : class
        {
            return Container.Resolve<T>(name, overrides);
        }

        /// <summary>
        ///解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Resolve<T>() where T : class
        {
            return Container.Resolve<T>();
        }
    }
}