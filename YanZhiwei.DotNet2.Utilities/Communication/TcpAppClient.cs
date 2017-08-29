namespace YanZhiwei.DotNet2.Utilities.Communication
{
    using Model;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using YanZhiwei.DotNet2.Utilities.Args;
    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.Enum;

    /// <summary>
    /// Socket Clinet
    /// </summary>
    public class TcpAppClient
    {
        #region Fields

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public EventHandler<TcpSeesionEventArgs> OnDataReceived;

        /// <summary>
        /// 客户端
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// 当前连接服务端地址
        /// </summary>
        private IPAddress ipAddress;

        /// <summary>
        /// 服务端IP+端口
        /// </summary>
        private IPEndPoint ipEndPoint;

        /// <summary>
        /// 是否关闭.(窗体关闭时关闭代码)
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 当前连接服务端端口号
        /// </summary>
        private int portNumber;

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] recBuffer = new byte[1 * 1024 * 1024];

        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] sendBuffer = new byte[1 * 1024 * 1024];

        /// <summary>
        /// 发送与接收使用的流
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// 当前管理对象
        /// </summary>
        private TcpClientConnectSession TcpClientConnectedSeesion;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">The ipaddress.</param>
        /// <param name="port">The port.</param>
        public TcpAppClient(string ip, int port)
        {
            ipAddress = IPAddress.Parse(ip);
            portNumber = port;
            ipEndPoint = new IPEndPoint(ipAddress, portNumber);
            client = new TcpClient();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">The ipaddress.</param>
        /// <param name="port">The port.</param>
        public TcpAppClient(IPAddress ip, int port)
        {
            ipAddress = ip;
            portNumber = port;
            ipEndPoint = new IPEndPoint(ipAddress, portNumber);
            client = new TcpClient();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 连接到Server
        /// </summary>
        public void Connect()
        {
            try
            {
                client.Connect(ipEndPoint);
                stream = new NetworkStream(client.Client, true);
                TcpClientConnectedSeesion = new TcpClientConnectSession(ipEndPoint, client, stream);
                TcpClientConnectedSeesion.SkStream.BeginRead(recBuffer, 0, recBuffer.Length, new AsyncCallback(EndReader), TcpClientConnectedSeesion);
                RaiseDataReceivedEvent(TcpOperateEvent.ConnectSuccess, null, null, ipEndPoint, null);
            }
            catch (Exception ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.ConnectError, null, ex, ipEndPoint, null);
            }
        }

        /// <summary>
        /// 端口与Server的连接
        /// </summary>
        public void Disconnect()
        {
            TcpClientConnectSession _connectedSession = new TcpClientConnectSession();

            if (client != null)
            {
                client.Client.Shutdown(SocketShutdown.Both);
                Thread.Sleep(10);
                client.Close();
                isClose = true;
                client = null;
                RaiseDataReceivedEvent(TcpOperateEvent.Disconnect, null, null, ipEndPoint, null);
            }
            else
            {
                RaiseDataReceivedEvent(TcpOperateEvent.Uninitialized, null, null, ipEndPoint, null);
            }
        }

        /// <summary>
        /// 重连上端.
        /// </summary>
        public void RestartConnect()
        {
            ipEndPoint = new IPEndPoint(ipAddress, portNumber);
            client = new TcpClient();
            Connect();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendData">The send data.</param>
        public void SendData(string sendData)
        {
            try
            {
                if (client != null)
                {
                    SendDataSucceed(sendData);
                    SendDataFailed_UnConnect();
                }
                else
                {
                    SendDataFailed_NullServer();
                }
            }
            catch (Exception ex)
            {
                SendDataFailed_Exception(ex);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendData">The send data.</param>
        public void SendData(byte[] sendData)
        {
            try
            {
                if (client != null)
                {
                    SendDataSucceed(sendData);
                    SendDataFailed_UnConnect();
                }
                else
                {
                    SendDataFailed_NullServer();
                }
            }
            catch (Exception ex)
            {
                SendDataFailed_Exception(ex);
            }
        }

        /// <summary>
        /// Ends the reader.
        /// </summary>
        /// <param name="ir">The ir.</param>
        private void EndReader(IAsyncResult ir)
        {
            TcpClientConnectSession _connectedSession = ir.AsyncState as TcpClientConnectSession;

            try
            {
                if (_connectedSession != null)
                {
                    if (isClose && client == null)
                    {
                        TcpClientConnectedSeesion.SkStream.Close();
                        TcpClientConnectedSeesion.SkStream.Dispose();
                        return;
                    }

                    _connectedSession.Offset = _connectedSession.SkStream.EndRead(ir);
                    byte[] _buffer = new byte[_connectedSession.Offset];
                    Array.Copy(recBuffer, _buffer, _connectedSession.Offset);

                    if (_buffer != null)
                    {
                        string _readString = Encoding.UTF8.GetString(_buffer);

                        if (string.Compare(_readString, "ServerOff", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RaiseDataReceivedEvent(TcpOperateEvent.ServerClose, null, null, _connectedSession.Ip, null);
                        }
                        else
                        {
                            RaiseDataReceivedEvent(TcpOperateEvent.DataReceived, _buffer, null, _connectedSession.Ip, null);
                        }
                    }

                    TcpClientConnectedSeesion.SkStream.BeginRead(recBuffer, 0, recBuffer.Length, new AsyncCallback(EndReader), TcpClientConnectedSeesion);
                }
            }
            catch (Exception ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.DataReceivedError, null, ex, ipEndPoint, null);
            }
        }

        /// <summary>
        /// Pusbs the client message.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="ipaddress">The ipaddress.</param>
        /// <param name="tag">The tag.</param>
        private void RaiseDataReceivedEvent(TcpOperateEvent code, byte[] buffer, Exception exception, IPEndPoint ipaddress, object tag)
        {
            TcpSeesionEventArgs _args = new TcpSeesionEventArgs();
            _args.Code = code;
            _args.DataBuffer = buffer;
            _args.Ex = exception;
            _args.Ip = ipaddress;
            _args.Tag = tag;
            OnDataReceived.RaiseEvent(this, _args);
        }

        /// <summary>
        /// Sends the data failed_ exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void SendDataFailed_Exception(Exception ex)
        {
            RaiseDataReceivedEvent(TcpOperateEvent.SendDataError, null, ex, ipEndPoint, null);
            RestartConnect();
        }

        /// <summary>
        /// Sends the data failed_ null server.
        /// </summary>
        private void SendDataFailed_NullServer()
        {
            RaiseDataReceivedEvent(TcpOperateEvent.ObjectNull, null, null, ipEndPoint, null);
            RestartConnect();
        }

        /// <summary>
        /// Sends the data failed_ un connect.
        /// </summary>
        private void SendDataFailed_UnConnect()
        {
            if (!client.Connected)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.UnConnect, null, null, ipEndPoint, null);
                RestartConnect();
            }
        }

        /// <summary>
        /// Sends the data succeed.
        /// </summary>
        /// <param name="sendData">The send data.</param>
        private void SendDataSucceed(string sendData)
        {
            if (client.Connected)
            {
                if (stream == null)
                {
                    stream = client.GetStream();
                }

                byte[] _buffer = Encoding.UTF8.GetBytes(sendData);
                stream.Write(_buffer, 0, _buffer.Length);
            }
        }

        /// <summary>
        /// Sends the data succeed.
        /// </summary>
        /// <param name="sendData">The send data.</param>
        private void SendDataSucceed(byte[] sendData)
        {
            if (client.Connected)
            {
                if (stream == null)
                {
                    stream = client.GetStream();
                }

                byte[] _buffer = sendData;
                stream.Write(_buffer, 0, _buffer.Length);
            }
        }

        #endregion Methods
    }
}