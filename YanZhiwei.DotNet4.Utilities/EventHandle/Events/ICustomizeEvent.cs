using System;

namespace YanZhiwei.DotNet4.Utilities.EventHandle.Events
{
    /// <summary>
    /// 自定义事件接口
    /// </summary>
    public interface ICustomizeEvent
    {
        /// <summary>
        /// 发生时间
        /// </summary>
        /// <returns>时间</returns>
        DateTime OccurredOn();
        
        /// <summary>
        /// 设置为已读
        /// </summary>
        /// <returns></returns>
        void Read();
        
        /// <summary>
        /// 是否已读
        /// </summary>
        bool IsRead
        {
            get;
        }
    }
}