using System;
using YanZhiwei.DotNet4.Utilities.EventHandle.Events;

namespace YanZhiwei.DotNet4.Utilities.EventHandle.Subscribers
{
    /// <summary>
    /// 自定义事件订阅
    /// </summary>
    /// <typeparam name="T">ICustomizeEvent</typeparam>
    public abstract class CustomizeEventSubscriber<T> where T : ICustomizeEvent
    {
        /// <summary>
        /// 订阅的事件类型
        /// </summary>
        /// <returns></returns>
        public Type SubscribedToEventType()
        {
            return typeof(T);
        }
        
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="domainEvent">事件</param>
        public abstract void HandleEvent(T domainEvent);
    }
}