namespace YanZhiwei.DotNet.Core.Infrastructure.Mvc
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using System;
    using System.Web.Mvc;
    using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;
    using YanZhiwei.DotNet.Core.Infrastructure.Mvc.DependencyManagement;

    /// <summary>
    /// 基于Mvc的Engine基类
    /// </summary>
    public class MvcEngineBase : EngineBase, IEngine
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
            base.RegisterDependencies(_builder, _typeFinder);
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
            base.RegisterMapperConfiguration(_webTypeFinder);
        }

        /// <summary>
        /// 运行启动任务
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            base.RunStartupTasks(_containerManager);
        }

        #endregion Methods
    }
}