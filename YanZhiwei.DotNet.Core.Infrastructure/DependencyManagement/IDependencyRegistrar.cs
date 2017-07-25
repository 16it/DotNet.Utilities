using Autofac;

namespace YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖注入接口
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// 注入服务和接口
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">ITypeFinder</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        /// <summary>
        /// 顺序
        /// </summary>
        int Order { get; }
    }
}