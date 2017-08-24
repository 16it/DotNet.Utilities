using Autofac;
using System;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure
{
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

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }
    }
}