using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YanZhiwei.DotNet4._5.Utilities.Model;

namespace YanZhiwei.DotNet4._5.Utilities.Communication
{
    /// <summary>
    /// Udp抽象类
    /// </summary>
    public abstract class UdpAppBase
    {
        /// <summary>
        /// Udp Client
        /// </summary>
        protected UdpClient AppUpdClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected UdpAppBase()
        {
            AppUpdClient = new UdpClient();
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <returns>UdpReceived</returns>
        public async Task<UdpAppReceived> Receive()
        {
            var _result = await AppUpdClient.ReceiveAsync();
            return new UdpAppReceived()
            {
                Message = Encoding.ASCII.GetString(_result.Buffer, 0, _result.Buffer.Length),
                Sender = _result.RemoteEndPoint
            };
        }
    }
}