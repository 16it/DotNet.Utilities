using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using YanZhiwei.DotNet4.Utilities.EventHandle.Events;
using YanZhiwei.DotNet4.Utilities.EventHandle.Subscribers;

namespace YanZhiwei.DotNet4.Utilities.EventHandle
{
    /// <summary>
    /// 自定义事件触发者
    /// </summary>
    public class CustomizeEventPublisher<T> where T : ICustomizeEvent
    {
        private static readonly ThreadLocal<List<CustomizeEventSubscriber<T>>> subscribers = new ThreadLocal<List<CustomizeEventSubscriber<T>>>();
        private static readonly ThreadLocal<bool> publishing = new ThreadLocal<bool> { Value = false };

        private static CustomizeEventPublisher<T> instance;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>CustomizeEventPublisher</returns>
        public static CustomizeEventPublisher<T> Instance()
        {
            if (instance != null)
                return instance;

            CustomizeEventPublisher<T> _eventPublisher = new CustomizeEventPublisher<T>();
            Interlocked.CompareExchange(ref instance, _eventPublisher, null);
            return _eventPublisher;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="aDomainEvent">事件</param>
        public void Publish(T aDomainEvent)
        {
            if (publishing.Value)
            {
                return;
            }

            try
            {
                publishing.Value = true;
                List<CustomizeEventSubscriber<T>> _registeredSubscribers = subscribers.Value;

                if (_registeredSubscribers != null)
                {
                    Type _eventType = aDomainEvent.GetType();

                    foreach (var domainEventSubscriber in _registeredSubscribers)
                    {
                        if (aDomainEvent.IsRead)
                            continue;

                        Type _subscribedTo = domainEventSubscriber.SubscribedToEventType();

                        if (_subscribedTo == _eventType || _subscribedTo is ICustomizeEvent)
                        {
                            domainEventSubscriber.HandleEvent(aDomainEvent);
                        }
                    }

                    aDomainEvent.Read();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                publishing.Value = false;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns>CustomizeEventPublisher</returns>
        public CustomizeEventPublisher<T> Reset()
        {
            if (!publishing.Value)
            {
                subscribers.Value = null;
            }

            return this;
        }

        /// <summary>
        /// 增加订阅
        /// </summary>
        /// <param name="aSubscriber">CustomizeEventSubscriber</param>
        public void Subscribe(CustomizeEventSubscriber<T> aSubscriber)
        {
            if (publishing.Value)
                return;

            List<CustomizeEventSubscriber<T>> _registeredSubscribers = subscribers.Value;

            if (_registeredSubscribers == null)
            {
                _registeredSubscribers = new List<CustomizeEventSubscriber<T>>();
                subscribers.Value = _registeredSubscribers;
            }

            if (_registeredSubscribers.Any(ent => ent.SubscribedToEventType().FullName == aSubscriber.SubscribedToEventType().FullName
                                           && ent.GetType().FullName == aSubscriber.GetType().FullName))  //相同的订阅只接收一次。
                return;

            _registeredSubscribers.Add(aSubscriber);
        }
    }
}