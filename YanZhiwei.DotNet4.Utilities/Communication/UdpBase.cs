using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YanZhiwei.DotNet4.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.Communication
{
    public abstract class UdpBase
    {
        protected UdpClient Client;

        protected UdpBase()
        {
            Client = new UdpClient();
        }

        //https://stackoverflow.com/questions/19786668/c-sharp-udp-socket-client-and-server
        public async Task<UdpReceived> Receive()
        {
            var result = await Client.ReceiveAsync();
            return new UdpReceived()
            {
                Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Sender = result.RemoteEndPoint
            };
        }
    }
}