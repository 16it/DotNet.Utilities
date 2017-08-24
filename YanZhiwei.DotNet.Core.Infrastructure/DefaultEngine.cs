using Autofac;
using System;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure
{
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

        #endregion Properties

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        /// <summary>
        /// ResolveAll
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }
    }
}