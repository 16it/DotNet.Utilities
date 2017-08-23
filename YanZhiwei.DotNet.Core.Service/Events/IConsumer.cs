namespace YanZhiwei.DotNet.Core.Service.Events
{
    /// <summary>
    /// 基于实体类的消费者接口
    /// </summary>
    /// <typeparam name="T">实体类类型</typeparam>
    public interface IConsumer<T>
    {
        /// <summary>
        /// 处理事件订阅
        /// </summary>
        /// <param name="eventMessage">实体类</param>
        void HandleEvent(T eventMessage);
    }
}