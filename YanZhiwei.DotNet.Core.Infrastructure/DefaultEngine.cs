namespace YanZhiwei.DotNet.Core.Infrastructure
{
    using Autofac;
    using System;
    using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

    /// <summary>
    /// 默认
    /// </summary>
    public class DefaultEngine : EngineBase, IEngine
    {
        //1、InstancePerDependency
        //对每一个依赖或每一次调用创建一个新的唯一的实例。这也是默认的创建实例的方式。
        //官方文档解释：Configure thecomponent sothat everydependent componentor callto Resolve()gets anew, unique instance(default.)
        //2、InstancePerLifetimeScope
        //在一个生命周期域中，每一个依赖或调用创建一个单一的共享的实例，且每一个不同的生命周期域，实例是唯一的，不共享的。
        //官方文档解释：Configure thecomponent sothat everydependent componentor callto Resolve()within asingle ILifetimeScopegets thesame, shared instance.Dependentcomponentsin different lifetimescopes willget differentinstances.
        //3、InstancePerMatchingLifetimeScope
        //在一个做标识的生命周期域中，每一个依赖或调用创建一个单一的共享的实例。打了标识了的生命周期域中的子标识域中可以共享父级域中的实例。若在整个继承层次中没有找到打标识的生命周期域，则会抛出异常：DependencyResolutionException。
        //官方文档解释：Configure thecomponent sothat everydependent componentor callto Resolve() withina ILifetimeScopetagged withany ofthe providedtags valuegets thesame, shared instance. Dependent components in lifetime scopesthat arechildren ofthe taggedscope willshare theparent's instance. If no appropriately tagged scope can be found in the hierarchy an DependencyResolutionException is thrown.
        //4、InstancePerOwned
        //在一个生命周期域中所拥有的实例创建的生命周期中，每一个依赖组件或调用Resolve() 方法创建一个单一的共享的实例，并且子生命周期域共享父生命周期域中的实例。若在继承层级中没有发现合适的拥有子实例的生命周期域，则抛出异常：DependencyResolutionException。
        //官方文档解释：
        //Configure thecomponent sothat everydependent componentor callto Resolve() withina ILifetimeScopecreated byan ownedinstance getsthe same, shared instance. Dependent components in lifetime scopesthat arechildren ofthe ownedinstance scopewill sharethe parent's instance. If no appropriate owned instance scope can be found in the hierarchy an DependencyResolutionException is thrown.
        //5、SingleInstance
        //每一次依赖组件或调用Resolve() 方法都会得到一个相同的共享的实例。其实就是单例模式。
        //官方文档解释：Configure thecomponent sothat everydependent componentor callto Resolve() getsthe same, shared instance.
        //6、InstancePerHttpRequest
        //在一次Http请求上下文中, 共享一个组件实例。仅适用于asp.net mvc开发。

        #region Fields

        private ContainerManager _containerManager;

        #endregion Fields

        #region Properties

        /// <summary>
        /// ContainerManager
        /// </summary>
        public virtual ContainerManager ContainerManager
        {
            get
            {
                return _containerManager;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var _builder = new ContainerBuilder();
            AppDomainTypeFinder _appDomainTypeFinder = new AppDomainTypeFinder();
            base.RegisterDependencies(_builder, _appDomainTypeFinder);
            base.RegisterMapperConfiguration(_builder, _appDomainTypeFinder);
            var _container = _builder.Build();
            this._containerManager = new ContainerManager(_container);
            base.RunStartupTasks(_containerManager);
        }

        /// <summary>
        /// 分解依赖
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>类型</returns>
        public T Resolve<T>()
            where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        ///  分解依赖
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型</returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// 分解依赖
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion Methods
    }
}