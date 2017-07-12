using Microsoft.Practices.Unity;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.CacheTests.Model;
using System.Linq;
namespace YanZhiwei.DotNet.Core.CacheTests
{
    public class EventPublisher : IEventPublisher
    {
        public EventPublisher()
        {
        }

        public void Publish<T>(T eventMessage)
        {
            //var consumers = Global.ServiceLocator.ResolveAll<IConsumer<T>>();
            //foreach (var consumer in consumers)
            //{
            //    this.PublishToConsumer(consumer, eventMessage);
            //}
            var consumer = Global.ServiceLocator.Resolve<IConsumer<T>>();
            this.PublishToConsumer(consumer, eventMessage);
        }

        protected virtual void PublishToConsumer<T>(IConsumer<T> consumer, T eventMessage)
        {
            consumer.HandleEvent(eventMessage);
        }
    }
}