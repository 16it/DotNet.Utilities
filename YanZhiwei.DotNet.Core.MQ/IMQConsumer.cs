using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YanZhiwei.DotNet.Core.MQ
{
    /// <summary>
    /// 消息队列消费者
    /// </summary>
    public interface IMQConsumer
    {
        /// <summary>
        /// 打开
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 开始监听
        /// </summary>
        void StartListen();
    }
}
