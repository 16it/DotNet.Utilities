using System.Net.Sockets;

namespace YanZhiwei.DotNet2.Utilities.Models
{
    /// <summary>
    /// Socket连接信息
    /// </summary>
    /// 时间：2016/6/7 13:25
    /// 备注：
    public class SocketConnectionInfo
    {
        public const int BufferSize = 1048576;
        public Socket Socket;
        public byte[] Buffer;
        public int BytesRead { get; set; }
    }
}