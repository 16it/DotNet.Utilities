namespace YanZhiwei.DotNet.Core.ActiveMQ
{
    using Apache.NMS;
    using Apache.NMS.ActiveMQ.Commands;
    using System;
    using System.Collections.Concurrent;
    using YanZhiwei.DotNet.Core.MQ;

    /// <summary>
    /// 消息队列，生产者
    /// </summary>
    public sealed class ActiveMQProducer : ActiveMQBase, IMQProducer, IDisposable
    {
        #region Fields

        /// <summary>
        /// 队列缓存字典
        /// </summary>
        public ConcurrentDictionary<string, IMessageProducer> MQProducerDic = new ConcurrentDictionary<string, IMessageProducer>();

        #endregion Fields

        #region Methods

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            IMessageProducer _mqProducer = null;
            foreach (var item in this.MQProducerDic)
            {
                if (MQProducerDic.TryGetValue(item.Key, out _mqProducer))
                {
                    _mqProducer?.Close();
                }
            }
            MQProducerDic.Clear();
            if (MQSession != null)
                MQSession.Close();
            if (MQCnnection != null)
                MQCnnection.Close();
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
            CreateProducer();
        }

        /// <summary>
        /// 向队列发送数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        public void SendMessage<T>(T data)
        {
            SendMessageBase(this.QueueName, data);
        }

        /// <summary>
        /// 向指定队列发送数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="queueName">指定队列名</param>
        public void SendMessage<T>(T data, string queueName)
        {
            SendMessageBase(queueName, data);
        }

        /// <summary>
        ///创建队列
        /// </summary>
        /// <returns></returns>
        private IMessageProducer CreateProducer()
        {
            return CreateProducer(base.QueueName);
        }

        /// <summary>
        /// 创建队列
        /// </summary>
        private IMessageProducer CreateProducer(string producerName)
        {
            if (MQSession == null)
            {
                Open();
            }
            IMessageProducer _messageProducer = null;
            if (!MQProducerDic.ContainsKey(producerName))
            {
                switch (MQMode)
                {
                    case MQMode.Queue:
                        _messageProducer = MQSession.CreateProducer(new ActiveMQQueue(producerName));
                        break;

                    case MQMode.Topic:
                        _messageProducer = MQSession.CreateProducer(new ActiveMQTopic(producerName));
                        break;
                }
                MQProducerDic.TryAdd(producerName, _messageProducer);
            }
            else
            {
                _messageProducer = MQProducerDic[producerName];
            }
            return _messageProducer;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="body">数据</param>
        private void SendMessageBase<T>(string queueName, T body)
        {
            var _producer = CreateProducer(queueName);
            IMessage _message;
            if (body is byte[])
            {
                _message = _producer.CreateBytesMessage(body as byte[]);
            }
            else if (body is string)
            {
                _message = _producer.CreateTextMessage(body as string);
            }
            else
            {
                _message = _producer.CreateObjectMessage(body);
            }
            if (_message != null)
            {
                _producer.Send(_message, MsgDeliveryMode.Persistent, MsgPriority.Normal, TimeSpan.MinValue);
            }
        }

        #endregion Methods
    }
}