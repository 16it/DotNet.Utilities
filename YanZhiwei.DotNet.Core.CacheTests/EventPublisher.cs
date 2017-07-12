using Microsoft.Practices.Unity;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.CacheTests.Model;

namespace YanZhiwei.DotNet.Core.CacheTests
{
    public class EventPublisher : IEventPublisher
    {
        public EventPublisher()
        {
        }

        public void Publish<T>(T eventMessage)
        {
            var consumers = Global.ServiceLocator.ResolveAll<IConsumer<T>>();
            foreach (var consumer in consumers)
            {
                this.PublishToConsumer(consumer, eventMessage);
            }
        }

        protected virtual void PublishToConsumer<T>(IConsumer<T> consumer, T eventMessage)
        {
            consumer.HandleEvent(eventMessage);
        }
    }
}