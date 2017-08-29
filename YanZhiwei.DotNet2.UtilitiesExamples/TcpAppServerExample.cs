using System;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Communication;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.UtilitiesExamples
{
    public class TcpAppServerExample
    {
        private static readonly object syncObj = new object();

        private static void Main(string[] args)
        {
            try
            {
                TcpAppServer _server = new TcpAppServer("192.168.51.223", 9882);
                _server.OnDataReceived += (sender, connectedSession) =>
                {
                    switch (connectedSession.Code)
                    {
                        case TcpOperateEventCode.ServerrStartSucceed:
                            Console.WriteLine("StartSucceed." + connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.DataReceived:
                            Console.WriteLine("Server DataReceived:" + ByteHelper.ToHexStringWithBlank(connectedSession.DataBuffer) + connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.NewClientConnected:
                            Console.WriteLine("NewClientConnect" + connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.ClientOffline:
                            Console.WriteLine("ClientOffline" + connectedSession.Ip.ToString());
                            break;
                    }
                };
                _server.Start();

                TcpAppClient _client = new TcpAppClient("192.168.51.223", 9882);
                _client.OnDataReceived += (sender, _connectedSession) =>
                {
                    switch (_connectedSession.Code)
                    {
                        case TcpOperateEventCode.ConnectSuccess:
                            Console.WriteLine("ConnectSuccess." + _connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.DataReceived:
                            byte[] _cc = _connectedSession.DataBuffer;
                            Console.WriteLine("Client DataReceived:" + _cc.Length + _connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.CliendDisconnected:
                            Console.WriteLine("Disconnect" + _connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.ServerClose:
                            Console.WriteLine("ServerClose" + _connectedSession.Ip.ToString());
                            break;
                    }
                };
                _client.Connect();

                TcpAppClient _client2 = new TcpAppClient("192.168.51.223", 9882);
                _client2.OnDataReceived += (sender, _connectedSession) =>
                {
                    switch (_connectedSession.Code)
                    {
                        case TcpOperateEventCode.ConnectSuccess:
                            Console.WriteLine("ConnectSuccess." + _connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.DataReceived:
                            byte[] _cc = _connectedSession.DataBuffer;
                            Console.WriteLine("Client DataReceived:" + _cc.Length + _connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.CliendDisconnected:
                            Console.WriteLine("Disconnect" + _connectedSession.Ip.ToString());
                            break;

                        case TcpOperateEventCode.ServerClose:
                            Console.WriteLine("ServerClose" + _connectedSession.Ip.ToString());
                            break;
                    }
                };
                _client2.Connect();

                _server.SendToAll("Hello World.");
                _client2.SendData("Hello World2.");
                _client.SendData("Hello World.");
                //_server.Stop();
                _client.Disconnect();
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
    }
}