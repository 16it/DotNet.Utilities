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
        // 1. TransientLifetimeManager
        //瞬态生命周期，默认情况下，在使用RegisterType进行对象关系注册时如果没有指定生命周期管理器则默认使用这个生命周期管理器，这个生命周期管理器就如同其名字一样，当使用这种管理器的时候，每次通过Resolve或ResolveAll调用对象的时候都会重新创建一个新的对象。
        //需要注意的是，使用RegisterInstance对已存在的对象进行关系注册的时候无法指定这个生命周期，否则会报异常。

        // 2.ContainerControlledLifetimeManager
        //容器控制生命周期管理，这个生命周期管理器是RegisterInstance默认使用的生命周期管理器，也就是单件实例，UnityContainer会维护一个对象实例的强引用，每次调用的时候都会返回同一对象

        // 3.HierarchicalLifetimeManager
        //分层生命周期管理器，这个管理器类似于ContainerControlledLifetimeManager，
        //也是由UnityContainer来管理，也就是单件实例。不过与ContainerControlledLifetimeManager不 同的是，
        //这个生命周期管理器是分层的，因为Unity的容器时可以嵌套的，所以这个生命周期管理器就是针对这种情况，
        //当使用了这种生命周期管理器，父容器 和子容器所维护的对象的生命周期是由各自的容器来管理
        //Unity这种分级容器的好处就在于我们可以对于有不同生命周期的对象放在不同的容器中，如果一个子容器被释放，不会影响到其它子容器中的对象，但是如果根节点处父容器释放后，所有的子容器都将被释放。

        // 4.PerThreadLifetimeManager
        //每线程生命周期管理器，就是保证每个线程返回同一实例

        // 5.PerResolveLifetimeManager
        //这个生命周期是为了解决循环引用而重复引用的生命周期
        //而对应的这个生命周期管理就是针对这种情况而新增的，其类似于 TransientLifetimeManager，但是其不同在于，
        //如果应用了这种生命周期管理器，则在第一调用的时候会创建一个新的对象，而再次通过 循环引用访问到的时候就会返回先前创建的对象实例（单件实例），

        // 6.ExternallyControlledLifetimeManager
        //外部控制生命周期管理器，这个 生命周期管理允许你使用RegisterType和RegisterInstance来注册对象之间的关系，但是其只会对对象保留一个弱引用，其生命周期 交由外部控制，也就是意味着你可以将这个对象缓存或者销毁而不用在意UnityContainer，而当其他地方没有强引用这个对象时，其会被GC给销毁 掉。
        //在默认情况下，使用这个生命周期管理器，每次调用Resolve都会返回同一对象（单件实例），如果被GC回收后再次调用Resolve方法将会重新创建新的对象

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