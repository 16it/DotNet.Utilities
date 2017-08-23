
namespace YanZhiwei.DotNet.Core.Service.Events
{
    /// <summary>
    /// 基于实体类的事件发布者接口
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="eventMessage">实体类对象</param>
        void Publish<T>(T eventMessage);
    }
}
