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
        private static ScoketAppServer Server = null;

        //public static void Main(string[] args)
        //{
        //    try
        //    {
        //        Server = new HighPerformanceServer(SocketProtocol.TCP, "127.0.0.1", 9887, 10);
        //        Server.OnDataReceived += Server_OnDataReceived;
        //        Server.OnServerStarted += _server_OnServerStart;
        //        Server.OnClientConnected += _server_OnClientConnected;
        //        Server.OnClientDisconnected += Server_OnClientDisconnected;
        //        Server.OnClientDisconnecting += Server_OnClientDisconnecting;
        //        Server.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        Console.ReadLine();
        //    }
        //}

        private static void Server_OnClientDisconnecting(object sender, SocketSeesionEventArgs e)
        {
            Console.WriteLine(e.DeviceKey + "OnClientDisconnecting");
        }

        private static void Server_OnClientDisconnected(object sender, SocketSeesionEventArgs e)
        {
            Console.WriteLine(e.DeviceKey + "OnClientDisconnected");
        }

        private static void Server_OnDataReceived(object sender, SocketSeesionEventArgs e)
        {
            byte[] msg = Encoding.ASCII.GetBytes("This is a test");
            if (e.Protocol == SocketProtocol.UDP)
                Server.Reply(msg, e.DeviceInfo);
            else
                e.Socket.Send(msg);
            //Server.Reply(msg, e.TerminalInfo);
            Console.WriteLine(DateTime.Now.FormatDate(1) + " [" + e.DeviceKey + "] " + ByteHelper.ToHexStringWithBlank(e.Buffer));
        }

        private static void _server_OnClientConnected(object sender, SocketSeesionEventArgs e)
        {
            Console.WriteLine(" [" + e.DeviceKey + "] " + "OnClientConnected");
        }

        private static void _server_OnServerStart(object sender, SocketServerStartedEventArgs e)
        {
            Console.WriteLine(e.SocketServer + " " + "启动成功。");
        }
    }
}