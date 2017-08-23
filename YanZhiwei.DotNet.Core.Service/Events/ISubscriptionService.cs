using System.Collections.Generic;

namespace YanZhiwei.DotNet.Core.Service.Events
{
    /// <summary>
    /// 基于实体类的订阅服务
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// 获取订阅者
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Event consumers</returns>
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
