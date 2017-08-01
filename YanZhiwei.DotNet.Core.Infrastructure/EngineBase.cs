using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;
using YanZhiwei.DotNet.Core.Infrastructure.Mapper;

namespace YanZhiwei.DotNet.Core.Infrastructure
{
    /// <summary>
    /// Engine基类
    /// </summary>
    public class EngineBase
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">ITypeFinder</param>
        protected void RegisterDependencies(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //依赖注入
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            //根据程序集依赖注入
            var _drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var _drInstances = new List<IDependencyRegistrar>();

            foreach (var drType in _drTypes)
            {
                _drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }

            //排序
            _drInstances = _drInstances.AsQueryable().OrderBy(t => t.Order).ToList();

            foreach (var dependencyRegistrar in _drInstances)
            {
                dependencyRegistrar.Register(builder, typeFinder);
            }
        }

        /// <summary>
        /// 映射注入依赖
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">ITypeFinder</param>
        protected void RegisterMapperConfiguration(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //查询映射类型AutoMapper
            var _mcTypes = typeFinder.FindClassesOfType<IMapperConfiguration>();
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
        }

        /// <summary>
        /// 映射注入依赖
        /// </summary>
        /// <param name="_webTypeFinder">ITypeFinder</param>
        protected void RegisterMapperConfiguration(ITypeFinder _webTypeFinder)
        {
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
        /// <param name="containerManager">ContainerManager</param>
        protected void RunStartupTasks(ContainerManager containerManager)
        {
            var _typeFinder = containerManager.Resolve<ITypeFinder>();
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
    }
}