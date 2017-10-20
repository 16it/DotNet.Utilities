namespace YanZhiwei.DotNet3._5.Utilities.Service
{
    using System;
    using System.ServiceModel;
    using System.Threading;

    using YanZhiwei.DotNet3._5.Utilities.Interfaces;

    /// <summary>
    /// WCF 服务操作上下文基类
    /// </summary>
    public abstract class WCFServiceContextBase<IWCFService>
        where IWCFService : IContractService
    {
        #region Fields

        /// <summary>
        /// 通讯心跳检测间隔
        /// </summary>
        public readonly uint MonitorClientChanelSec = 0;

        /// <summary>
        /// 服务地址
        /// </summary>
        public readonly string ServiceUrl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceUrl">WCF url地址</param>
        /// <param name="monitorClientChanelSec">通讯心跳检测间隔</param>
        public WCFServiceContextBase(string serviceUrl, uint monitorClientChanelSec)
        {
            ServiceUrl = serviceUrl;
            MonitorClientChanelSec = monitorClientChanelSec;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 是否启用心跳机制维护Service与Client通道
        /// </summary>
        public bool IsMonitorClientChanel
        {
            get;
            protected set;
        }

        /// <summary>
        /// WCF服务
        /// </summary>
        public IWCFService Service
        {
            get; protected set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 通讯心跳检测
        /// </summary>
        /// <param name="failedFactory">通讯失败操作委托</param>
        public virtual void MonitorClientChanel(Action<IWCFService> failedFactory)
        {
            IsMonitorClientChanel = true;
            ThreadPool.QueueUserWorkItem
            (
                (args) =>
                {
                    while (IsMonitorClientChanel)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(MonitorClientChanelSec));

                        try
                        {
                            var _communicatObject = ((ICommunicationObject)Service);

                            if (_communicatObject.State == CommunicationState.Faulted)
                            {
                                _communicatObject.Abort();
                                Service = CreateService();
                                if (failedFactory != null)
                                    failedFactory(Service);
                            }
                        }
                        catch
                        {
                            Service = CreateService();
                        }
                    }
                }
            );
        }

        /// <summary>
        /// 停止心跳监听
        /// </summary>
        public virtual void StopMonitorClientChanel()
        {
            IsMonitorClientChanel = false;
            var _communicatObject = ((ICommunicationObject)Service);

            if (_communicatObject.State == CommunicationState.Opened)
                _communicatObject.Abort();
        }

        /// <summary>
        /// 创建WCF服务
        /// </summary>
        /// <returns>IWCFService</returns>
        protected abstract IWCFService CreateService();

        #endregion Methods
    }
}