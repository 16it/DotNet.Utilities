using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Reflection;
using System.Web.Http;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;
using YanZhiwei.DotNet.Core.Infrastructure.WebApi.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure.WebApi
{
    /// <summary>
    /// WebApiEngine 抽象类
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Infrastructure.EngineBase" />
    /// <seealso cref="YanZhiwei.DotNet.Core.Infrastructure.IEngine" />
    public class WebApiEngineBase : EngineBase, IEngine
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
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var config = GlobalConfiguration.Configuration;
            //依赖注入
            RegisterDependencies(config);
            //依赖注入映射配置
            RegisterMapperConfiguration(config);
            //运行初始化任务
            RunStartupTasks(config);
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
        protected void RegisterDependencies(HttpConfiguration config)
        {
            var _builder = new ContainerBuilder();
            _builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).AsSelf().PropertiesAutowired();
            _builder.RegisterWebApiFilterProvider(config);
            _builder.RegisterWebApiModelBinderProvider();
            //依赖注入
            var _typeFinder = new WebAppTypeFinder();
            base.RegisterDependencies(_builder, _typeFinder);
            var _container = _builder.Build();
            this._containerManager = new WebApiContainerManager(_container);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
        }

        
        /// <summary>
        /// 映射注入依赖
        /// </summary>
        protected virtual void RegisterMapperConfiguration(HttpConfiguration config)
        {
            //Mvc TypeFinder
            WebAppTypeFinder _webTypeFinder = new WebAppTypeFinder();
            base.RegisterMapperConfiguration(_webTypeFinder);
        }

        /// <summary>
        /// 运行启动任务
        /// </summary>
        protected virtual void RunStartupTasks(HttpConfiguration config)
        {
            base.RunStartupTasks(_containerManager);
        }

        #endregion Methods
    }
}