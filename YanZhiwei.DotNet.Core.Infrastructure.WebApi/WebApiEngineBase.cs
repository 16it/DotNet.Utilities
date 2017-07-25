using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;
using YanZhiwei.DotNet.Core.Infrastructure.Mapper;
using YanZhiwei.DotNet.Core.Infrastructure.WebApi.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure.WebApi
{
    public class WebApiEngineBase : IEngine
    {
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
        /// 在WebApi环境中初始化组件和插件
        /// </summary>
        public void Initialize(HttpConfiguration config)
        {
            //依赖注入
            RegisterDependencies(config);
            //依赖注入映射配置
            RegisterMapperConfiguration(config);
            //运行初始化任务
            RunStartupTasks(config);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {

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

        /// <summary>
        /// 依赖注入
        /// </summary>
        protected virtual void RegisterDependencies(HttpConfiguration config)
        {
            var _builder = new ContainerBuilder();
            //依赖注入
            var _typeFinder = new WebAppTypeFinder();
            _builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            _builder.RegisterInstance(_typeFinder).As<ITypeFinder>().SingleInstance();
            //根据程序集依赖注入
            var _drTypes = _typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var _drInstances = new List<IDependencyRegistrar>();

            foreach (var drType in _drTypes)
            {
                _drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }

            //排序
            _drInstances = _drInstances.AsQueryable().OrderBy(t => t.Order).ToList();

            foreach (var dependencyRegistrar in _drInstances)
            {
                dependencyRegistrar.Register(_builder, _typeFinder);
            }

            var _container = _builder.Build();
            this._containerManager = new WebApiContainerManager(_container);

            var _webApiResolver = new AutofacWebApiDependencyResolver(_container);
            config.DependencyResolver = _webApiResolver;
        }

        /// <summary>
        /// 映射注入依赖
        /// </summary>
        protected virtual void RegisterMapperConfiguration(HttpConfiguration config)
        {
            //Mvc TypeFinder
            WebAppTypeFinder _webTypeFinder = new WebAppTypeFinder();
            //查询映射类型AutoMapper
            var _mcTypes = _webTypeFinder.FindClassesOfType<IMapperConfiguration>();
            var _mcInstances = new List<IMapperConfiguration>();

            foreach (var mcType in _mcTypes)
            {
                _mcInstances.Add((IMapperConfiguration)Activator.CreateInstance(mcType));
            }

            //排序
            _mcInstances = _mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //获取AutoMapper映射配置表达式
            var _configurationActions = new List<Action<IMapperConfigurationExpression>>();

            foreach (var mc in _mcInstances)
            {
                _configurationActions.Add(mc.GetConfiguration());
            }

            //依赖注入
            AutoMapperConfiguration.Init(_configurationActions);
        }

        /// <summary>
        /// 运行启动任务
        /// </summary>
        protected virtual void RunStartupTasks(HttpConfiguration config)
        {
            var _typeFinder = _containerManager.Resolve<ITypeFinder>();
            var _startUpTaskTypes = _typeFinder.FindClassesOfType<IStartupTask>();
            var _startUpTasks = new List<IStartupTask>();

            foreach (var startUpTaskType in _startUpTaskTypes)
            {
                _startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            }

            //排序
            _startUpTasks = _startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();

            foreach (var startUpTask in _startUpTasks)
            {
                startUpTask.Execute();
            }
        }

        #endregion Methods
    }
}