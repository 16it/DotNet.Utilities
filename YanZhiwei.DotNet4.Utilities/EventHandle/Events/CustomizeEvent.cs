using System;

namespace YanZhiwei.DotNet4.Utilities.EventHandle.Events
{
    /// <summary>
    /// 自定义事件
    /// </summary>
    public class CustomizeEvent : ICustomizeEvent
    {
        /// <summary>
        /// 事件发生时间
        /// </summary>
        protected readonly DateTime OccurredOnTime;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        protected CustomizeEvent()
        {
            this.OccurredOnTime = DateTime.Now;
            this.IsRead = false;
        }
        
        /// <summary>
        /// 发生时间
        /// </summary>
        /// <returns></returns>
        public DateTime OccurredOn()
        {
            return this.OccurredOnTime;
        }
        
        /// <summary>
        /// 设置已经读取
        /// </summary>
        public void Read()
        {
            this.IsRead = true;
        }
        
        /// <summary>
        /// 已经读取
        /// </summary>
        public bool IsRead
        {
            get;
            private set;
        }
    }
}