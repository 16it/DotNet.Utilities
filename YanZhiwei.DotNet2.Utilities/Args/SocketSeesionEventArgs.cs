using System;
using System.Net;

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
        public IPEndPoint TerminalInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 数据报文
        /// </summary>
        public byte[] DataBuffer
        {
            get;
            set;
        }
    }
}