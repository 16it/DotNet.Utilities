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
    using YanZhiwei.DotNet2.Utilities.Operator;

    /// <summary>
    /// Socket TCP协议终端客户端
    /// </summary>
    public class TcpAppClient
    {
        #region Fields

        /// <summary>
        /// 当前连接服务端地址
        /// </summary>
        public readonly IPAddress TcpClientIpAddress;

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public EventHandler<TcpSeesionEventArgs> OnDataReceived;

        /// <summary>
        /// 是否关闭.(窗体关闭时关闭代码)
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] recBuffer = new byte[1 * 1024 * 1024];

        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] sendBuffer = new byte[1 * 1024 * 1024];

        /// <summary>
        /// 当前管理对象
        /// </summary>
        private TcpClientConnectSession TcpClientConnectedSeesion;

        /// <summary>
        /// 当前连接服务端端口号
        /// </summary>
        private ushort TcpClientPort;

        /// <summary>
        /// 发送与接收使用的流
        /// </summary>
        private NetworkStream tcpClientStream;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">TcpClient Ip地址.</param>
        /// <param name="port">TcpClient 端口.</param>
        public TcpAppClient(string ip, ushort port)
        {
            ValidateOperator.Begin().IsIp(ip, "TcpClient Ip地址");
            TcpClientIpAddress = IPAddress.Parse(ip);
            TcpClientPort = port;
            TcpClientIpEndPoint = new IPEndPoint(TcpClientIpAddress, TcpClientPort);
            ConnectTcpClient = new TcpClient();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient ConnectTcpClient
        {
            get;
            private set;
        }

        /// <summary>
        /// 服务端IP+端口
        /// </summary>
        public IPEndPoint TcpClientIpEndPoint
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 连接到Server
        /// </summary>
        public void Connect()
        {
            try
            {
                ConnectTcpClient.Connect(TcpClientIpEndPoint);
                tcpClientStream = new NetworkStream(ConnectTcpClient.Client, true);
                TcpClientConnectedSeesion = new TcpClientConnectSession(TcpClientIpEndPoint, ConnectTcpClient, tcpClientStream);
                TcpClientConnectedSeesion.SkStream.BeginRead(recBuffer, 0, recBuffer.Length, new AsyncCallback(EndReader), TcpClientConnectedSeesion);
                RaiseDataReceivedEvent(TcpOperateEvent.ConnectSuccess, null, null, TcpClientIpEndPoint, null);
            }
            catch (Exception ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.ConnectError, null, ex, TcpClientIpEndPoint, null);
            }
        }

        /// <summary>
        /// 端口与Server的连接
        /// </summary>
        public void Disconnect()
        {
            TcpClientConnectSession _connectedSession = new TcpClientConnectSession();

            if (ConnectTcpClient != null)
            {
                ConnectTcpClient.Client.Shutdown(SocketShutdown.Both);
                Thread.Sleep(10);
                ConnectTcpClient.Close();
                isClose = true;
                ConnectTcpClient = null;
                RaiseDataReceivedEvent(TcpOperateEvent.Disconnect, null, null, TcpClientIpEndPoint, null);
            }
            else
            {
                RaiseDataReceivedEvent(TcpOperateEvent.Uninitialized, null, null, TcpClientIpEndPoint, null);
            }
        }

        /// <summary>
        /// 重连上端.
        /// </summary>
        public void RestartConnect()
        {
            TcpClientIpEndPoint = new IPEndPoint(TcpClientIpAddress, TcpClientPort);
            ConnectTcpClient = new TcpClient();
            Connect();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendData">消息</param>
        public void SendData(string sendData)
        {
            try
            {
                if (ConnectTcpClient != null)
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
        /// <param name="sendData">消息</param>
        public void SendData(byte[] sendData)
        {
            try
            {
                if (ConnectTcpClient != null)
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
        /// EndReader
        /// </summary>
        /// <param name="ir">IAsyncResult</param>
        private void EndReader(IAsyncResult ir)
        {
            TcpClientConnectSession _connectedSession = ir.AsyncState as TcpClientConnectSession;

            try
            {
                if (_connectedSession != null)
                {
                    if (isClose && ConnectTcpClient == null)
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
                RaiseDataReceivedEvent(TcpOperateEvent.DataReceivedError, null, ex, TcpClientIpEndPoint, null);
            }
        }

        /// <summary>
        /// 触发相关事件
        /// </summary>
        /// <param name="code">TcpOperateEvent</param>
        /// <param name="buffer">数据流</param>
        /// <param name="exception">异常信息</param>
        /// <param name="ipaddress">ip地址</param>
        /// <param name="tag">附加信息</param>
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
        /// 触发发送消息失败异常事件
        /// </summary>
        /// <param name="ex">Exception</param>
        private void SendDataFailed_Exception(Exception ex)
        {
            RaiseDataReceivedEvent(TcpOperateEvent.SendDataError, null, ex, TcpClientIpEndPoint, null);
            RestartConnect();
        }

        /// <summary>
        /// Sends the data failed_ null server.
        /// </summary>
        private void SendDataFailed_NullServer()
        {
            RaiseDataReceivedEvent(TcpOperateEvent.ObjectNull, null, null, TcpClientIpEndPoint, null);
            RestartConnect();
        }

        /// <summary>
        /// Sends the data failed_ un connect.
        /// </summary>
        private void SendDataFailed_UnConnect()
        {
            if (!ConnectTcpClient.Connected)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.UnConnect, null, null, TcpClientIpEndPoint, null);
                RestartConnect();
            }
        }

        /// <summary>
        /// Sends the data succeed.
        /// </summary>
        /// <param name="sendData">The send data.</param>
        private void SendDataSucceed(string sendData)
        {
            if (ConnectTcpClient.Connected)
            {
                if (tcpClientStream == null)
                {
                    tcpClientStream = ConnectTcpClient.GetStream();
                }

                byte[] _buffer = Encoding.UTF8.GetBytes(sendData);
                tcpClientStream.Write(_buffer, 0, _buffer.Length);
            }
        }

        /// <summary>
        /// Sends the data succeed.
        /// </summary>
        /// <param name="sendData">The send data.</param>
        private void SendDataSucceed(byte[] sendData)
        {
            if (ConnectTcpClient.Connected)
            {
                if (tcpClientStream == null)
                {
                    tcpClientStream = ConnectTcpClient.GetStream();
                }

                byte[] _buffer = sendData;
                tcpClientStream.Write(_buffer, 0, _buffer.Length);
            }
        }

        #endregion Methods
    }
}