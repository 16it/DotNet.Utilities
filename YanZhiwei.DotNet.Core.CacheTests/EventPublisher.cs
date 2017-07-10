using Microsoft.Practices.Unity;
using YanZhiwei.DotNet.Core.Cache;

namespace YanZhiwei.DotNet.Core.CacheTests
{
    public class EventPublisher : IEventPublisher
    {
        public EventPublisher()
        {
        }

        public void Publish<T>(T eventMessage)
        {
            
            //IUnityContainer container = new UnityContainer();
            //var consumers = DependencyResolver.Current.GetServices<IConsumer<T>>();
            //foreach (var consumer in consumers)
            //{
            //    this.PublishToConsumer(consumer, eventMessage);
            //}
        }

        protected virtual void PublishToConsumer<T>(IConsumer<T> consumer, T eventMessage)
        {
            consumer.HandleEvent(eventMessage);
        }
    }
}