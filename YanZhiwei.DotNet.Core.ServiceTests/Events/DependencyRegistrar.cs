using Autofac;
using System;
using System.Linq;
using YanZhiwei.DotNet.Core.Infrastructure;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;
using YanZhiwei.DotNet.Core.Service.Events;

namespace YanZhiwei.DotNet.Core.ServiceTests.Events
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();
            builder.RegisterInstance(new UserService()).As<IUserService>();
            //builder.RegisterType<UserService>().As<IUserService>();

        }
    }
}