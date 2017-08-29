namespace YanZhiwei.DotNet2.Utilities.Communication
{
    using Collection;
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
    /// Socket TCP协议主站服务端
    /// </summary>
    public class TcpAppServer
    {
        #region Fields

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public EventHandler<TcpSeesionEventArgs> OnDataReceived;

        /// <summary>
        /// 当前IP,端口对象
        /// </summary>
        public readonly IPEndPoint TcpServerEndPoint;

        /// <summary>
        /// 是否停止
        /// </summary>
        private bool isStop = false;

        /// <summary>
        /// 服务端
        /// </summary>
        public TcpListener TcpServer { get; private set; }

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] recBuffer = new byte[1 * 1024 * 1024];

        /// <summary>
        /// 信号量
        /// </summary>
        private Semaphore semap = new Semaphore(5, 5000);

        /// <summary>
        /// 发送缓冲区
        /// </summary>
        private byte[] sendBuffer = new byte[1 * 1024 * 1024];

        /// <summary>
        /// The synchronize root
        /// </summary>
        private object syncRoot = new object();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">TcpServer Ip地址</param>
        /// <param name="port">TcpServer 端口</param>
        public TcpAppServer(string ip, ushort port)
        {
            ValidateOperator.Begin().IsIp(ip, "TcpServer Ip地址");
            TcpClientConnectList = new ThreadSafeList<TcpClientConnectSession>();
            IPAddress _ipaddress = IPAddress.Parse(ip);
            TcpServerEndPoint = new IPEndPoint(_ipaddress, port);
            TcpServer = new TcpListener(TcpServerEndPoint);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 客户端队列集合
        /// </summary>
        public ThreadSafeList<TcpClientConnectSession> TcpClientConnectList
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 断开，移除所有终端链接
        /// </summary>
        /// 时间：2016-04-12 19:19
        /// 备注：
        public void ClearAllClients()
        {
            if (TcpClientConnectList != null)
            {
                for (int i = 0; i < TcpClientConnectList.Count; i++)
                {
                    TcpClientConnectSession _connectedSession = TcpClientConnectList[i];
                    TcpClientConnectList.Remove(_connectedSession);
                    _connectedSession.Close();
                }
            }
        }

        /// <summary>
        /// 断开，移除某个终端连接
        /// </summary>
        /// <param name="ip">IPEndPoint</param>
        /// 时间：2016-04-12 19:21
        /// 备注：
        public void ClearClient(IPEndPoint ip)
        {
            if (TcpClientConnectList != null)
            {
                TcpClientConnectSession _connectedSession = TcpClientConnectList.Find(o =>
                {
                    return o.Ip == ip;
                });

                if (_connectedSession != null)
                {
                    TcpClientConnectList.Remove(_connectedSession);
                }
            }
        }

        /// <summary>
        /// 向所有在线的客户端发送信息.
        /// </summary>
        /// <param name="sendData">发送的文本</param>
        public void SendToAll(string sendData)
        {
            for (int i = 0; i < TcpClientConnectList.Count; i++)
            {
                SendToClient(TcpClientConnectList[i].Ip, sendData);
            }
        }

        /// <summary>
        /// 向所有在线的客户端发送信息.
        /// </summary>
        /// <param name="sendDataBuffer">发送的文本</param>
        public void SendToAll(byte[] sendDataBuffer)
        {
            for (int i = 0; i < TcpClientConnectList.Count; i++)
            {
                SendToClient(TcpClientConnectList[i].Ip, sendDataBuffer);
            }
        }

        /// <summary>
        /// 向某一位客户端发送信息
        /// </summary>
        /// <param name="ip">客户端IP+端口地址</param>
        /// <param name="sendDataBuffer">发送的数据包</param>
        public void SendToClient(IPEndPoint ip, byte[] sendDataBuffer)
        {
            try
            {
                TcpClientConnectSession _connectedSession = TcpClientConnectList.Find(o =>
                {
                    return o.Ip == ip;
                });

                if (_connectedSession != null)
                {
                    if (_connectedSession.Client.Connected)
                    {
                        NetworkStream _stream = _connectedSession.SkStream;

                        if (_stream.CanWrite)
                        {
                            byte[] _buffer = sendDataBuffer;
                            _stream.Write(_buffer, 0, _buffer.Length);
                        }
                        else
                        {
                            _stream = _connectedSession.Client.GetStream();

                            if (_stream.CanWrite)
                            {
                                byte[] _buffer = sendDataBuffer;
                                _stream.Write(_buffer, 0, _buffer.Length);
                            }
                            else
                            {
                                TcpClientConnectList.Remove(_connectedSession);
                                RaiseDataReceivedEvent(TcpOperateEvent.RemoveClientConnect, null, null, _connectedSession.Ip, null);
                            }
                        }
                    }
                    else
                    {
                        RaiseDataReceivedEvent(TcpOperateEvent.NoClinets, null, null, _connectedSession.Ip, null);
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.SendDataError, null, ex, ip, null);
            }
        }

        /// <summary>
        /// 向某一位客户端发送信息
        /// </summary>
        /// <param name="ip">客户端IP+端口地址</param>
        /// <param name="sendData">发送的数据包</param>
        public void SendToClient(IPEndPoint ip, string sendData)
        {
            try
            {
                TcpClientConnectSession _connectedSession = TcpClientConnectList.Find(o =>
                {
                    return o.Ip == ip;
                });

                if (_connectedSession != null)
                {
                    if (_connectedSession.Client.Connected)
                    {
                        NetworkStream _netStream = _connectedSession.SkStream;

                        if (_netStream.CanWrite)
                        {
                            byte[] _buffer = Encoding.UTF8.GetBytes(sendData);
                            _netStream.Write(_buffer, 0, _buffer.Length);
                        }
                        else
                        {
                            _netStream = _connectedSession.Client.GetStream();

                            if (_netStream.CanWrite)
                            {
                                byte[] _buffer = Encoding.UTF8.GetBytes(sendData);
                                _netStream.Write(_buffer, 0, _buffer.Length);
                            }
                            else
                            {
                                TcpClientConnectList.Remove(_connectedSession);
                            }
                        }
                    }
                    else
                    {
                        RaiseDataReceivedEvent(TcpOperateEvent.NoClinets, null, null, ip, null);
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.SendDataError, null, ex, ip, null);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start()
        {
            try
            {
                TcpServer.Start();
                Thread _task = new Thread(new ThreadStart(delegate
                {
                    while (true)
                    {
                        if (isStop != false)
                        {
                            break;
                        }

                        GetAcceptTcpClient();
                        Thread.Sleep(1);
                    }
                }));
                _task.Start();
                RaiseDataReceivedEvent(TcpOperateEvent.StartSucceed, null, null, TcpServerEndPoint, null);
            }
            catch (SocketException ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.StartError, null, ex, TcpServerEndPoint, null);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if (TcpServer != null)
            {
                SendToAll("ServerOff");
                TcpServer.Stop();
                TcpServer = null;
                isStop = true;
                RaiseDataReceivedEvent(TcpOperateEvent.Stop, null, null, TcpServerEndPoint, null);
            }
        }

        /// <summary>
        /// 添加或更新终端连接
        /// </summary>
        /// <param name="connectedSession">TcpClientConnectSession</param>
        private void AddTcpClientConnecedSession(TcpClientConnectSession connectedSession)
        {
            TcpClientConnectSession _connectedSession = TcpClientConnectList.Find(o => o.Ip == connectedSession.Ip);

            if (_connectedSession == null)
            {
                TcpClientConnectList.Add(connectedSession);
            }
            else
            {
                TcpClientConnectList.Remove(_connectedSession);
                TcpClientConnectList.Add(connectedSession);
            }

            RaiseDataReceivedEvent(TcpOperateEvent.NewClientConnect, null, null, connectedSession.Ip, null);
        }

        /// <summary>
        /// 异步接收发送的信息.
        /// </summary>
        /// <param name="ir">IAsyncResult</param>
        private void EndReader(IAsyncResult ir)
        {
            TcpClientConnectSession _connectedSession = ir.AsyncState as TcpClientConnectSession;

            if (_connectedSession != null && TcpServer != null)
            {
                try
                {
                    if (_connectedSession.NewClientFlag || _connectedSession.Offset != 0)
                    {
                        _connectedSession.NewClientFlag = false;
                        _connectedSession.Offset = _connectedSession.SkStream.EndRead(ir);

                        if (_connectedSession.Offset != 0)
                        {
                            byte[] _buffer = new byte[_connectedSession.Offset];
                            Array.Copy(recBuffer, _buffer, _connectedSession.Offset);
                            RaiseDataReceivedEvent(TcpOperateEvent.DataReceived, _buffer, null, _connectedSession.Ip, null);
                        }
                        else
                        {
                            TcpClientConnectList.Remove(_connectedSession);//移除连接终端
                            RaiseDataReceivedEvent(TcpOperateEvent.ClientOffline, null, null, _connectedSession.Ip, null);
                        }

                        _connectedSession.SkStream.BeginRead(recBuffer, 0, recBuffer.Length, new AsyncCallback(EndReader), _connectedSession);
                    }
                }
                catch (Exception ex)
                {
                    lock (syncRoot)
                    {
                        TcpClientConnectList.Remove(_connectedSession);
                        RaiseDataReceivedEvent(TcpOperateEvent.DataReceivedError, null, ex, _connectedSession.Ip, null);
                    }
                }
            }
        }

        /// <summary>
        /// 等待处理新的连接
        /// </summary>
        private void GetAcceptTcpClient()
        {
            try
            {
                semap.WaitOne();
                TcpClient _tclient = TcpServer.AcceptTcpClient();
                Socket _socket = _tclient.Client;
                NetworkStream _stream = new NetworkStream(_socket, true); //承载这个Socket
                TcpClientConnectSession _connectedSession = new TcpClientConnectSession(_tclient.Client.RemoteEndPoint as IPEndPoint, _tclient, _stream);
                _connectedSession.NewClientFlag = true;
                AddTcpClientConnecedSession(_connectedSession);
                _connectedSession.SkStream.BeginRead(recBuffer, 0, recBuffer.Length, new AsyncCallback(EndReader), _connectedSession);

                if (_stream.CanWrite)
                {
                }

                semap.Release();
            }
            catch (Exception ex)
            {
                semap.Release();
                RaiseDataReceivedEvent(TcpOperateEvent.NewClientConnectError, null, ex, null, null);
            }
        }

        /// <summary>
        /// 向终端触发相关事件订阅
        /// </summary>
        /// <param name="code">TcpOperateEvent</param>
        /// <param name="buffer">数据流</param>
        /// <param name="exception">异常辛星</param>
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
            OnDataReceived.RaiseEvent<TcpSeesionEventArgs>(this, _args);
        }

        #endregion Methods
    }
}