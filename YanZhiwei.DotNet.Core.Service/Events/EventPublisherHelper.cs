namespace YanZhiwei.DotNet.Core.Service.Events
{
    /// <summary>
    /// 事件发布者辅助类
    /// </summary>
    public static class EventPublisherHelper
    {
        /// <summary>
        /// 实体类插入触发
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventPublisher"></param>
        /// <param name="entity"></param>
        public static void EntityInserted<T>(this IEventPublisher eventPublisher, T entity) where T : class
        {
            eventPublisher.Publish(new EntityInserted<T>(entity));
        }

        /// <summary>
        /// 实体类更新触发
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventPublisher"></param>
        /// <param name="entity"></param>
        public static void EntityUpdated<T>(this IEventPublisher eventPublisher, T entity) where T : class
        {
            eventPublisher.Publish(new EntityUpdated<T>(entity));
        }

        /// <summary>
        /// 实体类删除触发
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventPublisher"></param>
        /// <param name="entity"></param>
        public static void EntityDeleted<T>(this IEventPublisher eventPublisher, T entity) where T : class
        {
            eventPublisher.Publish(new EntityDeleted<T>(entity));
        }
    }
}