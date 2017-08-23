using System.Collections.Generic;
using YanZhiwei.DotNet.Core.Infrastructure;

namespace YanZhiwei.DotNet.Core.Service.Events
{
    /// <summary>
    /// 基于实体类的订阅服务
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        /// <summary>
        /// 获取订阅者
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Event consumers</returns>
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return EngineContext.Current.ResolveAll<IConsumer<T>>();
        }
    }
}