using System;
using YanZhiwei.DotNet2.Utilities.Core;
using YanZhiwei.DotNet2.Utilities.Enums;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet2.UtilitiesExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {

                HighPerformanceServer _socket = new HighPerformanceServer(ServerType.TCP, "127.0.0.1", 9998);
              
                _socket.OnClientConnected += socket_OnClientConnected;
                _socket.OnDataReceived += socket_OnDataReceived;
                _socket.Start();
                //ProcessHelperExample.ExecBatCommand();
                //FileHelperExample.CopyLocalBigFile();
                // ObjectIdExample.Demo();
                ConsoleApplicationHelper.DetectShutdown(() =>
                {
                    Console.WriteLine("DetectShutdown");
                });
                ConsoleApplicationHelper.DetectKeyPress(input =>
                {
                    Console.WriteLine("input:" + input);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private static void socket_OnDataReceived(object sender, EventArgs e)
        {
            Console.WriteLine("终端连接");
        }

        private static void socket_OnClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("终端连接");
        }
    }
}