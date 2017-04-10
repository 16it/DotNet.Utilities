using System;
using YanZhiwei.DotNet4.Utilities.EventHandle.Subscribers;

namespace YanZhiwei.DotNet4.UtilitiesExamples
{
    public class GoodsSoldOutSubscriber : CustomizeEventSubscriber<GoodsSoldOut>
    {
        public override void HandleEvent(GoodsSoldOut domainEvent)
        {
            Console.WriteLine("GoodsSoldOut:" + domainEvent.GoodsId);
        }
    }
}