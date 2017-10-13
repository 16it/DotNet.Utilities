namespace YanZhiwei.DotNet.Core.ActiveMQ
{
    using Apache.NMS;
    using Apache.NMS.ActiveMQ;
    using YanZhiwei.DotNet.Core.MQ;

    /// <summary>
    /// ActiveMQ 抽象基类
    /// </summary>
    public abstract class ActiveMQBase
    {
        #region Fields

        public IConnection MQCnnection { get; protected set; }
        public IMessageConsumer MQConsumer { get; protected set; }
        public ISession MQSession { get; protected set; }

        #endregion Fields

        #region Properties

        /// <summary>
        /// 连接地址
        /// </summary>
        public string BrokerUri
        {
            get;
            set;
        }

        /// <summary>
        /// 指定使用队列的模式
        /// </summary>
        public MQMode MQMode
        {
            get;
            set;
        }

        /// <summary>
        /// 用于登录的密码,必须和用户名同时指定
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName
        {
            get;
            set;
        }

        /// <summary>
        /// 用于登录的用户名,必须和密码同时指定
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        #endregion Properties

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            ConnectionFactory _connectFactory = new ConnectionFactory(this.BrokerUri);
            if (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(Password))
                MQCnnection = _connectFactory.CreateConnection();
            else
                MQCnnection = _connectFactory.CreateConnection(UserName, Password);
            MQCnnection.Start();
            MQSession = MQCnnection.CreateSession();
        }
    }
}