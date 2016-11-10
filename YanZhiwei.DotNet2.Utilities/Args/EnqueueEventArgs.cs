using System;

namespace YanZhiwei.DotNet2.Utilities.Args
{
    /// <summary>
    /// 工作线成队列
    /// </summary>
    public class EnqueueEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 泛型数值
        /// </summary>
        public T Item
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="item">传递数值</param>
        public EnqueueEventArgs(T item)
        {
            Item = item;
        }
    }
}