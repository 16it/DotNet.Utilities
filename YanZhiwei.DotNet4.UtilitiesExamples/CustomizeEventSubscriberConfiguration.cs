using YanZhiwei.DotNet4.Utilities.EventHandle;

namespace YanZhiwei.DotNet4.UtilitiesExamples
{
    public class CustomizeEventSubscriberConfiguration
    {
        public static void Initialize()
        {
            CustomizeEventPublisher<GoodsSoldOut>.Instance().Subscribe(new GoodsSoldOutSubscriber());
        }
    }
}