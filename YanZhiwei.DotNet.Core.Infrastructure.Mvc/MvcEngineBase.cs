namespace YanZhiwei.DotNet.Core.Infrastructure.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    
    using Autofac;
    using Autofac.Integration.Mvc;
    
    using AutoMapper;
    
    using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;
    using YanZhiwei.DotNet.Core.Infrastructure.Mapper;
    using YanZhiwei.DotNet.Core.Infrastructure.Mvc.DependencyManagement;
    
    /// <summary>
    /// 基于Mvc的Engine基类
    /// </summary>
    public class MvcEngineBase : IEngine
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
        /// 在nop环境中初始化组件和插件
        /// </summary>
        public void Initialize()
        {
            //依赖注入
            RegisterDependencies();
            //依赖注入映射配置
            RegisterMapperConfiguration();
            //运行初始化任务
            RunStartupTasks();
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
        protected virtual void RegisterDependencies()
        {
            var _builder = new ContainerBuilder();
            //依赖注入
            var _typeFinder = new WebAppTypeFinder();
            _builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            _builder.RegisterInstance(_typeFinder).As<ITypeFinder>().SingleInstance();
            //根据程序集依赖注入
            var _drTypes = _typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var _drInstances = new List<IDependencyRegistrar>();
            
            foreach(var drType in _drTypes)
            {
                _drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }
            
            //排序
            _drInstances = _drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            
            foreach(var dependencyRegistrar in _drInstances)
            {
                dependencyRegistrar.Register(_builder, _typeFinder);
            }
            
            var _container = _builder.Build();
            this._containerManager = new MvcContainerManager(_container);
            //用AutofacDependencyResolver替换MVC默认的DependencyResolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        }
        
        /// <summary>
        /// 映射注入依赖
        /// </summary>
        protected virtual void RegisterMapperConfiguration()
        {
            //Mvc TypeFinder
            WebAppTypeFinder _webTypeFinder = new WebAppTypeFinder();
            //查询映射类型AutoMapper
            var _mcTypes = _webTypeFinder.FindClassesOfType<IMapperConfiguration>();
            var _mcInstances = new List<IMapperConfiguration>();
            
            foreach(var mcType in _mcTypes)
            {
                _mcInstances.Add((IMapperConfiguration)Activator.CreateInstance(mcType));
            }
            
            //排序
            _mcInstances = _mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //获取AutoMapper映射配置表达式
            var _configurationActions = new List<Action<IMapperConfigurationExpression>>();
            
            foreach(var mc in _mcInstances)
            {
                _configurationActions.Add(mc.GetConfiguration());
            }
            
            //依赖注入
            AutoMapperConfiguration.Init(_configurationActions);
        }
        
        /// <summary>
        /// 运行启动任务
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            var _typeFinder = _containerManager.Resolve<ITypeFinder>();
            var _startUpTaskTypes = _typeFinder.FindClassesOfType<IStartupTask>();
            var _startUpTasks = new List<IStartupTask>();
            
            foreach(var startUpTaskType in _startUpTaskTypes)
            {
                _startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            }
            
            //排序
            _startUpTasks = _startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            
            foreach(var startUpTask in _startUpTasks)
            {
                startUpTask.Execute();
            }
        }
        
        #endregion Methods
    }
}