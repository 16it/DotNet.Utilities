namespace YanZhiwei.DotNet2.Utilities.Args
{
    using System;
    using System.Net;

    using YanZhiwei.DotNet2.Utilities.Enum;

    /// <summary>
    /// Socket 服务停止完成
    /// </summary>
    public class SocketServerStopedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// 协议类型
        /// </summary>
        public SocketProtocol Protocol
        {
            get;
            set;
        }

        /// <summary>
        /// Socket 服务信息
        /// </summary>
        public IPEndPoint SocketServer
        {
            get; set;
        }

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime StopedTime
        {
            get; set;
        }

        #endregion Properties
    }
}