using Autofac;
using Autofac.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using YanZhiwei.DotNet2.Utilities.ExtendException;

namespace YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 管理依赖注入的容器，实例化服务
    /// 它相当于一个仓库。ContainerManager 通过在构造函数中传入IContainer 的实例，
    /// 为 _container赋值。同时，ContainerManager 也提供了Resolve方法，
    /// 为在 _container中注册的服务提供实例。
    /// </summary>
    public class ContainerManager
    {
        private readonly IContainer _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">IContainer</param>
        public ContainerManager(IContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// 获取一个IOC容器
        /// </summary>
        public virtual IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// 是否依赖注册
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// 分解
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>分解服务</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 分解
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>分解服务</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// 分解
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>分解服务</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// 分解选项
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>已分解的服务</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// 分解未注册的服务
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>分解服务</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// 分解未注册的服务
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <returns>分解服务</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null)
                            throw new FrameworkException("未知的依赖");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (FrameworkException)
                {
                }
            }
            throw new FrameworkException("未找到所有依赖项满足的构造函数.");
        }

        /// <summary>
        /// 获取生命周期
        /// </summary>
        /// <returns>Scope</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                //if (HttpContext.Current != null) mvc情况
                //    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                //当返回此类生存期范围时，应该确保它将被一次性处理(例如在计划任务中)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                //当返回此类生存期范围时，应该确保它将被一次性处理(例如在计划任务中)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }

        /// <summary>
        /// 尝试解析
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="scope">生命周期; 通过null自动解析当前生命周期</param>
        /// <param name="instance">解析服务</param>
        /// <returns>服务是否已成功解析</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }
    }
}