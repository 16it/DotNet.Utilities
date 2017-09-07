using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using YanZhiwei.DotNet3._5.Utilities.Interfaces;

namespace YanZhiwei.DotNet3._5.Utilities.Service
{
    /// <summary>
    /// WCF服务寄宿抽象基类
    /// </summary>
    /// <typeparam name="ServerType">The type of the erver type.</typeparam>
    /// <typeparam name="ContractType">The type of the ontract type.</typeparam>
    public abstract class WcfServiceHostFactory<ServerType, ContractType>
          where ServerType : class
          where ContractType : IContractService
    {
        #region Properties

        /// <summary>
        /// 获取或设置配置了此绑定的通道上可以接收的消息的最大大小。
        /// </summary>
        public abstract int MaxReceivedMessageSize
        {
            get;    //= 2147483647;
        }

        /// <summary>
        /// 终结点的基址
        /// </summary>
        public abstract string ServiceAddr
        {
            get;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        public abstract TimeSpan Timeout
        {
            get;    // TimeSpan.FromMinutes(10);
        }

        /// <summary>
        /// 承载服务的基址
        /// </summary>
        public abstract Uri BaseURI
        {
            get;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 自定义Behaviors
        /// </summary>
        /// <param name="behaviors">KeyedByTypeCollection</param>
        public abstract void AddBehaviors(KeyedByTypeCollection<IEndpointBehavior> behaviors);

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
            return _netTcpBinding;
        }

        private ServiceHost CreateServiceHost(Binding binding)
        {
            ServiceHost _host = new ServiceHost(typeof(ServerType), BaseURI);
            ServiceEndpoint _endpoint = _host.AddServiceEndpoint(typeof(ContractType), binding, ServiceAddr);
            AddBehaviors(_endpoint.Behaviors);
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

        #endregion Methods
    }
}