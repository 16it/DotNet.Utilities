using System.Text;

namespace YanZhiwei.DotNet4._5.Utilities.Communication
{
    /// <summary>
    /// Upd 终端
    /// </summary>
    public class UdpAppClient : UdpAppBase
    {
        private UdpAppClient()
        {
        }

        /// <summary>
        /// 连接Upd Server
        /// </summary>
        /// <param name="hostname">主机名</param>
        /// <param name="port">端口</param>
        /// <returns>UdpAppClient</returns>
        public static UdpAppClient ConnectTo(string hostname, int port)
        {
            var connection = new UdpAppClient();
            connection.AppUpdClient.Connect(hostname, port);
            return connection;
        }

        /// <summary>
        /// 发送数据报文
        /// </summary>
        /// <param name="message">数据报文</param>
        public void Send(string message)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            AppUpdClient.Send(datagram, datagram.Length);
        }
    }
}