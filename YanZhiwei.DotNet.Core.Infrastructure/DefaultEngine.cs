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
            AppDomainTypeFinder _webTypeFinder = new AppDomainTypeFinder();
            base.RegisterDependencies(_builder, _webTypeFinder);
            base.RegisterMapperConfiguration(_builder, _webTypeFinder);
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