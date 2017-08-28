using System;
using System.Net;
using System.Net.Sockets;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.Utilities.Args
{
    /// <summary>
    /// Socket 会话参数
    /// </summary>
    public class SocketSeesionEventArgs : EventArgs
    {
        /// <summary>
        /// 终端信息
        /// </summary>
        public IPEndPoint DeviceInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 终端Key
        /// </summary>
        public string DeviceKey
        {
            get
            {
                return DeviceInfo.ToString();
            }
        }

        /// <summary>
        /// 数据报文
        /// </summary>
        public byte[] Buffer
        {
            get;
            set;
        }

        /// <summary>
        ///Socket对象
        /// </summary>
        public Socket Socket
        {
            get;
            set;
        }

        /// <summary>
        /// 协议类型
        /// </summary>
        public SocketProtocol Protocol
        {
            get;
            set;
        }
    }
}