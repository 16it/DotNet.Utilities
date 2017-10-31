namespace YanZhiwei.DotNet3._5.Utilities.Service
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.Xml;

    using YanZhiwei.DotNet3._5.Utilities.Interfaces;

    /// <summary>
    /// WCF服务寄宿抽象基类
    /// </summary>
    /// <typeparam name="ServerType">The type of the erver type.</typeparam>
    /// <typeparam name="ContractType">The type of the ontract type.</typeparam>
    public abstract class WCFServiceHostFactory<ServerType, ContractType>
        where ServerType : ContractType
        where ContractType : IContractService
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceURL">WCF服务地址</param>
        /// <param name="maxReceivedMessageSize">获取或设置配置了此绑定的通道上可以接收的消息的最大大小</param>
        /// <param name="openTimeout">超时时间</param>
        public WCFServiceHostFactory(string serviceURL, int maxReceivedMessageSize, TimeSpan openTimeout, TimeSpan receiveTimeout, TimeSpan sendTimeout)
        {
            ServiceURL = serviceURL;
            MaxReceivedMessageSize = maxReceivedMessageSize;
            OpenTimeout = openTimeout;
            SendTimeout = sendTimeout;
            ReceiveTimeout = receiveTimeout;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceURL">WCF服务地址</param>
        public WCFServiceHostFactory(string serviceURL)
            : this(serviceURL, 2147483647, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30))
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 获取或设置配置了此绑定的通道上可以接收的消息的最大大小。
        /// </summary>
        public int MaxReceivedMessageSize
        {
            get;    //= 2147483647;
            protected set;
        }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceURL
        {
            get;
            protected set;
        }

        /// <summary>
        /// 设置在传输引发异常之前可用于打开连接的时间间隔
        /// </summary>
        public TimeSpan OpenTimeout
        {
            get;    // TimeSpan.FromMinutes(10);
            protected set;
        }

        /// <summary>
        /// 设置在传输引发异常之前可用于完成写入操作的时间间隔
        /// </summary>
        public TimeSpan ReceiveTimeout
        {
            get;    // TimeSpan.FromMinutes(10);
            protected set;
        }

        /// <summary>
        /// 设置连接在撤消之前保持非活动状态的最大时间间隔，在此时间间隔内未接收任何应用程序消息
        /// </summary>
        public TimeSpan SendTimeout
        {
            get;    // TimeSpan.FromMinutes(10);
            protected set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 自定义Behaviors
        /// 运行于服务级别，适用于所有端点，负责内容如：实例化、事务、授权、审计 等；
        /// </summary>
        /// <param name="description">ServiceDescription</param>
        public abstract void AddServiceBehaviors(ServiceDescription description);

        /// <summary>
        /// 创建WCF服务
        /// 用于把WCF服务当作ASMX Web 服务。用于兼容旧的Web ASMX 服务
        /// </summary>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual ServiceHost CreateBasicHttpServiceHost()
        {
            return CreateServiceHost(CreateBasicHttpBinding());
        }

        /// <summary>
        /// 创建WCF服务
        /// 使用 TCP 协议，用于在局域网(Intranet)内跨机器通信。有几个特点：可靠性、事务支持和安全，优化了 WCF 到 WCF 的通信。限制是服务端和客户端都必须使用 WCF 来实现。
        /// </summary>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual ServiceHost CreateNetTcpServiceHost()
        {
            return CreateServiceHost(CreateNetTcpBinding());
        }

        /// <summary>
        /// 创建WCF服务
        /// 和 WSHttpBinding 相比，它支持 duplex 类型的服务。
        /// </summary>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual ServiceHost CreateWSDualHttpServiceHost()
        {
            return CreateServiceHost(CreateWSDualHttpBinding());
        }

        /// <summary>
        /// 创建WCF服务
        /// 比 BasicHttpBinding 更加安全，通常用于 non-duplex 服务通讯
        /// </summary>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual ServiceHost CreateWSHttpServiceHost()
        {
            return CreateServiceHost(CreateWSHttpBinding());
        }

        private BasicHttpBinding CreateBasicHttpBinding()
        {
            BasicHttpBinding _basicHttpBinding = new BasicHttpBinding();
            _basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
            _basicHttpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            _basicHttpBinding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            _basicHttpBinding.ReaderQuotas.MaxStringContentLength = MaxReceivedMessageSize;
            _basicHttpBinding.ReaderQuotas.MaxArrayLength = MaxReceivedMessageSize;
            _basicHttpBinding.ReaderQuotas.MaxBytesPerRead = MaxReceivedMessageSize;
            _basicHttpBinding.OpenTimeout = OpenTimeout;
            _basicHttpBinding.ReceiveTimeout = ReceiveTimeout;
            _basicHttpBinding.SendTimeout = SendTimeout;
            return _basicHttpBinding;
        }

        private Binding CreateNetTcpBinding()
        {
            NetTcpBinding _netTcpBinding = new NetTcpBinding();
            _netTcpBinding.Security.Mode = SecurityMode.None;
            _netTcpBinding.MaxConnections = 1000;
            _netTcpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            _netTcpBinding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            _netTcpBinding.ReaderQuotas.MaxStringContentLength = MaxReceivedMessageSize;
            _netTcpBinding.ReaderQuotas.MaxArrayLength = MaxReceivedMessageSize;
            _netTcpBinding.ReaderQuotas.MaxBytesPerRead = MaxReceivedMessageSize;
            _netTcpBinding.OpenTimeout = OpenTimeout;
            _netTcpBinding.ReceiveTimeout = ReceiveTimeout;
            _netTcpBinding.SendTimeout = SendTimeout;
            _netTcpBinding.TransferMode = TransferMode.Buffered;
            return _netTcpBinding;
        }

        private ServiceHost CreateServiceHost(Binding binding)
        {
            ServiceHost _host = new ServiceHost(typeof(ServerType));
            ServiceEndpoint _endpoint = _host.AddServiceEndpoint(typeof(ContractType), binding, new Uri(ServiceURL));
            AddServiceBehaviors(_host.Description);
            return _host;
        }

        private WSDualHttpBinding CreateWSDualHttpBinding()
        {
            WSDualHttpBinding _basicHttpBinding = new WSDualHttpBinding();
            _basicHttpBinding.Security.Mode = WSDualHttpSecurityMode.None;
            _basicHttpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            _basicHttpBinding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            _basicHttpBinding.ReaderQuotas.MaxStringContentLength = MaxReceivedMessageSize;
            _basicHttpBinding.ReaderQuotas.MaxArrayLength = MaxReceivedMessageSize;
            _basicHttpBinding.ReaderQuotas.MaxBytesPerRead = MaxReceivedMessageSize;
            _basicHttpBinding.OpenTimeout = OpenTimeout;
            _basicHttpBinding.ReceiveTimeout = ReceiveTimeout;
            _basicHttpBinding.SendTimeout = SendTimeout;
            return _basicHttpBinding;
        }

        private WSHttpBinding CreateWSHttpBinding()
        {
            WSHttpBinding _wsHttpBinding = new WSHttpBinding();
            _wsHttpBinding.Security.Mode = SecurityMode.None;
            _wsHttpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            _wsHttpBinding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            _wsHttpBinding.ReaderQuotas.MaxStringContentLength = MaxReceivedMessageSize;
            _wsHttpBinding.ReaderQuotas.MaxArrayLength = MaxReceivedMessageSize;
            _wsHttpBinding.ReaderQuotas.MaxBytesPerRead = MaxReceivedMessageSize;
            _wsHttpBinding.OpenTimeout = OpenTimeout;
            _wsHttpBinding.ReceiveTimeout = ReceiveTimeout;
            _wsHttpBinding.SendTimeout = SendTimeout;
            return _wsHttpBinding;
        }

        #endregion Methods
    }
}