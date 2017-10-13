namespace YanZhiwei.DotNet.Core.ActiveMQ
{
    using System;

    using Apache.NMS;
    using Apache.NMS.ActiveMQ.Commands;

    using YanZhiwei.DotNet.Core.MQ;

    /// <summary>
    /// 消费者,打开连接,监听队列,接收数据
    /// </summary>
    public sealed class ActiveMQConsumer : ActiveMQBase, IMQConsumer, IDisposable
    {
        #region Properties

        /// <summary>
        /// 接收到数据回调,原生IMessage类型
        /// </summary>
        public Action<IMessage> OnMessageReceived
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            MQConsumer?.Close();
            MQSession?.Close();
            MQCnnection?.Close();
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            base.Init();

            switch (MQMode)
            {
                case MQMode.Queue:
                    MQConsumer = MQSession.CreateConsumer(new ActiveMQQueue(this.QueueName));
                    break;

                case MQMode.Topic:
                    MQConsumer = MQSession.CreateConsumer(new ActiveMQTopic(this.QueueName));
                    break;
            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void StartListen()
        {
            if (MQConsumer == null)
            {
                Open();
            }

            MQConsumer.Listener += msg =>
            {
                if (OnMessageReceived != null)
                    OnMessageReceived(msg);
            };
        }

        #endregion Methods
    }
}