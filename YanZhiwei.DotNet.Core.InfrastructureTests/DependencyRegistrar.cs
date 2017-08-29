using Autofac;
using YanZhiwei.DotNet.Core.Infrastructure;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.InfrastructureTests
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 0;
            }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var _finder = (AppDomainTypeFinder)typeFinder;
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        }
    }
}