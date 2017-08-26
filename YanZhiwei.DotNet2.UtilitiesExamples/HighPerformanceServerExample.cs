using System;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Communication;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.UtilitiesExamples
{
    public class HighPerformanceServerExample
    {
        public static void Main(string[] args)
        {
            try
            {
                HighPerformanceServer _server = new HighPerformanceServer(SocketProtocol.UDP, "127.0.0.1", 9887);
                _server.OnDataReceived += Server_OnDataReceived;
                _server.OnServerStart += _server_OnServerStart;
                _server.OnClientConnected += _server_OnClientConnected;
                _server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void Server_OnDataReceived(object sender, EventArgs e)
        {
            if (sender != null)
            {
                byte[] _buffer = (byte[])sender;
                Console.WriteLine(ByteHelper.ToHexStringWithBlank(_buffer));
            }
        }

        private static void _server_OnClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("终端链接");
        }

        private static void _server_OnServerStart(object sender, EventArgs e)
        {
            Console.WriteLine("启动成功。");
        }
    }
}