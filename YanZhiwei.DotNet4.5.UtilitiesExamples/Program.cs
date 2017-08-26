using System;
using System.Threading.Tasks;
using YanZhiwei.DotNet4._5.Utilities.Communication;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet4._5.UtilitiesExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //创建Udp Server主站
            var server = new UdpAppServer("127.0.0.1", 32123);

            //开始监听从终端传输过来得消息
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await server.Receive();
                    Console.WriteLine(string.Format("{0} 主站收到：{1}", DateTime.Now.FormatDate(1), received.Message));
                    server.Reply("copy " + received.Message, received.Sender);
                    if (received.Message == "quit")
                        break;
                }
            });

            //创建Udp终端，并链接到主站
            var client = UdpAppClient.ConnectTo("127.0.0.1", 32123);

            //等待来自服务器的应答消息发送到控制台
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        var received = await client.Receive();
                        Console.WriteLine(received.Message);
                        if (received.Message.Contains("quit"))
                            break;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                    }
                }
            });

            string read;
            do
            {
                read = Console.ReadLine();
                client.Send(read);
            } while (read != "quit");

        }
    }
}