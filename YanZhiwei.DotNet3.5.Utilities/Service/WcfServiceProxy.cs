namespace YanZhiwei.DotNet3._5.Utilities.Service
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.Xml;

    /// <summary>
    /// Wcf 服务代理抽象类
    /// </summary>
    public abstract class WcfServiceProxy<T> where T : class
    {
        #region Properties

        /// <summary>
        /// 获取或设置配置了此绑定的通道上可以接收的消息的最大大小。
        /// </summary>
        protected abstract int MaxReceivedMessageSize
        {
            get;    //= 2147483647;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        protected abstract TimeSpan Timeout
        {
            get;    // TimeSpan.FromMinutes(10);
        }

        /// <summary>
        /// 服务端 URI
        /// </summary>
        protected abstract string ServiceURL
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 自定义Behaviors
        /// </summary>
        /// <param name="endpoint">ServiceEndpoint</param>
        public abstract void AddEndpointBehaviors(ServiceEndpoint endpoint);

        /// <summary>
        /// 获得通信状态
        /// </summary>
        public ICommunicationObject CommunicationObject
        {
            get; private set;
        }

        /// <summary>
        /// 创建WCF信道
        /// 用于把WCF服务当作ASMX Web 服务。用于兼容旧的Web ASMX 服务
        /// </summary>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual T CreateBasicHttpChannel()
        {
            Binding _binding = CreateBasicHttpBinding();
            return CreateChannel(_binding);
        }

        /// <summary>
        /// 创建WCF信道
        /// 使用 TCP 协议，用于在局域网(Intranet)内跨机器通信。有几个特点：可靠性、事务支持和安全，优化了 WCF 到 WCF 的通信。限制是服务端和客户端都必须使用 WCF 来实现。
        /// </summary>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual T CreateNetTcpChannel()
        {
            Binding _binding = CreateNetTcpBinding();
            return CreateChannel(_binding);
        }

        /// <summary>
        /// 创建WCF信道
        /// 使用 TCP 协议，用于在局域网(Intranet)内跨机器通信。有几个特点：可靠性、事务支持和安全，优化了 WCF 到 WCF 的通信。限制是服务端和客户端都必须使用 WCF 来实现。
        /// </summary>
        public virtual T CreateNetTcpChannel<DataContractCallBack>()
              where DataContractCallBack : class, new()
        {
            Binding _binding = CreateNetTcpBinding();
            return CreateDuplexChannelFactory<DataContractCallBack>(_binding);
        }

        /// <summary>
        /// 创建WCF信道
        /// 和 WSHttpBinding 相比，它支持 duplex 类型的服务。
        /// </summary>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual T CreateWSDualHttpChannel()
        {
            Binding _binding = CreateWSDualHttpBinding();
            return CreateChannel(_binding);
        }

        /// <summary>
        /// 创建WCF服务
        /// 比 BasicHttpBinding 更加安全，通常用于 non-duplex 服务通讯
        /// </summary>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual T CreateWSHttpChannel()
        {
            Binding _binding = CreateWSHttpBinding();
            return CreateChannel(_binding);
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
            _basicHttpBinding.OpenTimeout = Timeout;
            _basicHttpBinding.ReceiveTimeout = Timeout;
            _basicHttpBinding.SendTimeout = Timeout;
            return _basicHttpBinding;
        }

        private Binding CreateNetTcpBinding()
        {
            NetTcpBinding _netTcpBinding = new NetTcpBinding();
            _netTcpBinding.Security.Mode = SecurityMode.None;
            _netTcpBinding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            _netTcpBinding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            _netTcpBinding.ReaderQuotas.MaxStringContentLength = MaxReceivedMessageSize;
            _netTcpBinding.ReaderQuotas.MaxArrayLength = MaxReceivedMessageSize;
            _netTcpBinding.ReaderQuotas.MaxBytesPerRead = MaxReceivedMessageSize;
            _netTcpBinding.OpenTimeout = Timeout;
            _netTcpBinding.ReceiveTimeout = Timeout;
            _netTcpBinding.SendTimeout = Timeout;
            _netTcpBinding.ListenBacklog = 1000;
            _netTcpBinding.MaxConnections = 1000;
            _netTcpBinding.TransferMode = TransferMode.Buffered;
            return _netTcpBinding;
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
            _basicHttpBinding.OpenTimeout = Timeout;
            _basicHttpBinding.ReceiveTimeout = Timeout;
            _basicHttpBinding.SendTimeout = Timeout;

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
            _wsHttpBinding.OpenTimeout = Timeout;
            _wsHttpBinding.ReceiveTimeout = Timeout;
            _wsHttpBinding.SendTimeout = Timeout;
            return _wsHttpBinding;
        }

        private T CreateChannel(Binding binding)
        {
            ChannelFactory<T> _channelFactory = new ChannelFactory<T>(binding, new EndpointAddress(ServiceURL));
            AddEndpointBehaviors(_channelFactory.Endpoint);

            foreach (OperationDescription op in _channelFactory.Endpoint.Contract.Operations)
            {
                var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dataContractBehavior != null)
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }

            _channelFactory.Open();
            return _channelFactory.CreateChannel();
        }

        private T CreateDuplexChannelFactory<DataContractCallBack>(Binding binding)
            where DataContractCallBack : class, new()
        {
            DataContractCallBack _contractCall = new DataContractCallBack();
            InstanceContext _context = new InstanceContext(_contractCall);
            CommunicationObject = _context;
            DuplexChannelFactory<T> _channelFacotry = new DuplexChannelFactory<T>(_context, binding);
            AddEndpointBehaviors(_channelFacotry.Endpoint);

            foreach (OperationDescription op in _channelFacotry.Endpoint.Contract.Operations)
            {
                var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dataContractBehavior != null)
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }

            _channelFacotry.Open();
            return _channelFacotry.CreateChannel(new EndpointAddress(ServiceURL));
        }

        #endregion Methods
    }
}