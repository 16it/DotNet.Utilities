namespace YanZhiwei.DotNet3._5.Utilities.Service
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.Xml;

    /// <summary>
    /// Wcf 服务代理抽象类
    /// </summary>
    public abstract class WcfServiceProxy
    {
        #region Fields

        /// <summary>
        /// 获取或设置配置了此绑定的通道上可以接收的消息的最大大小。
        /// </summary>
        protected abstract int MaxReceivedMessageSize
        {
            get;    //= 2147483647;
            set;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        protected abstract TimeSpan Timeout
        {
            get;    // TimeSpan.FromMinutes(10);
            set;
        }

        /// <summary>
        /// 标识终结点的 URI
        /// </summary>
        protected abstract string Url
        {
            get;
            set;
        }

        #endregion Fields

        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>
        /// 类型
        /// </returns>
        /// 时间：2016/9/6 16:54
        /// 备注：
        public virtual T CreateService<T>()
        where T : class
        {
            BasicHttpBinding _binding = new BasicHttpBinding();
            _binding.MaxReceivedMessageSize = MaxReceivedMessageSize;
            _binding.ReaderQuotas = new XmlDictionaryReaderQuotas();
            _binding.ReaderQuotas.MaxStringContentLength = MaxReceivedMessageSize;
            _binding.ReaderQuotas.MaxArrayLength = MaxReceivedMessageSize;
            _binding.ReaderQuotas.MaxBytesPerRead = MaxReceivedMessageSize;
            _binding.OpenTimeout = Timeout;
            _binding.ReceiveTimeout = Timeout;
            _binding.SendTimeout = Timeout;
            ChannelFactory<T> _chan = new ChannelFactory<T>(_binding, new EndpointAddress(Url));

            foreach (OperationDescription op in _chan.Endpoint.Contract.Operations)
            {
                var dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dataContractBehavior != null)
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
            }

            _chan.Open();
            var service = _chan.CreateChannel();
            return service;
        }
    }
}