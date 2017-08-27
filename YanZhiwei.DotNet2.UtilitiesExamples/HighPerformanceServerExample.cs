using System;
using System.Text;
using YanZhiwei.DotNet2.Utilities.Args;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Communication;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.UtilitiesExamples
{
    public class HighPerformanceServerExample
    {
        private static HighPerformanceServer Server = null;

        public static void Main(string[] args)
        {
            try
            {
                Server = new HighPerformanceServer(SocketProtocol.TCP, "127.0.0.1", 9887);
                Server.OnDataReceived += Server_OnDataReceived;
                Server.OnServerStart += _server_OnServerStart;
                Server.OnClientConnected += _server_OnClientConnected;
                Server.Start();
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

        private static void Server_OnDataReceived(object sender, SocketSeesionEventArgs e)
        {
            if (sender != null)
            {
                byte[] msg = Encoding.ASCII.GetBytes("This is a test");

                //Server.Reply(msg, e.TerminalInfo);
                Console.WriteLine(DateTime.Now.FormatDate(1) + " " + e.TerminalInfo.ToString() + " " + ByteHelper.ToHexStringWithBlank(e.DataBuffer));
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