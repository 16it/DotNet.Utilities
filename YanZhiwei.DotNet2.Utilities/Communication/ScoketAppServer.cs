namespace YanZhiwei.DotNet2.Utilities.Communication
{
    using Enum;
    using Model;
    using Operator;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using YanZhiwei.DotNet2.Utilities.Args;
    using YanZhiwei.DotNet2.Utilities.Common;

    /// <summary>
    /// Socket 服务
    /// </summary>
    /// 时间：2016/6/7 22:38
    /// 备注：
    public sealed class ScoketAppServer
    {
        #region Fields

        private int currentConnections = 0;
        private EndPoint ipeSender;
        private Socket listener;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="protocol">类型</param>
        /// <param name="ipAddress">ip地址</param>
        /// 时间：2016/6/7 11:35
        /// 备注：
        public ScoketAppServer(SocketProtocol protocol, string ipAddress)
        : this(protocol, ipAddress, 9888, 1024)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="protocol">类型</param>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="port">端口</param>
        /// <param name="maxQueuedConnections">Socket最多可容纳的等待接受的传入连接数</param>
        /// 时间：2016/6/7 11:35
        /// 备注：
        public ScoketAppServer(SocketProtocol protocol, string ipAddress, ushort port, int maxQueuedConnections)
        {
            ValidateOperator.Begin().NotNullOrEmpty(ipAddress, "Ip地址").IsIp(ipAddress, "Ip地址");
            IPAddress _ipAddress;

            if (IPAddress.TryParse(ipAddress, out _ipAddress))
            {
                this.Endpoint = new IPEndPoint(_ipAddress, port);
            }
            else
            {
                throw new ArgumentException("未能识别的Ip地址。");
            }

            this.Protocol = protocol;
            this.Port = port;
            this.MaxQueuedConnections = maxQueuedConnections;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// 终端已经连接事件
        /// </summary>
        /// 时间：2016/6/7 22:47
        /// 备注：
        public event EventHandler<SocketSeesionEventArgs> OnClientConnected;

        /// <summary>
        /// 终端已经断开连接事件
        /// </summary>
        /// 时间：2016/6/7 22:48
        /// 备注：
        public event EventHandler<SocketSeesionEventArgs> OnClientDisconnected;

        /// <summary>
        /// 终端正在断开连接事件
        /// </summary>
        /// 时间：2016/6/7 22:48
        /// 备注：
        public event EventHandler<SocketSeesionEventArgs> OnClientDisconnecting;

        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// 时间：2016/6/7 22:48
        /// 备注：
        public event EventHandler<SocketSeesionEventArgs> OnDataReceived;

        /// <summary>
        /// 服务启动完成事件
        /// </summary>
        /// 时间：2016/6/7 22:46
        /// 备注：
        public event EventHandler<SocketServerStartedEventArgs> OnServerStarted;

        /// <summary>
        /// 服务已经停止事件
        /// </summary>
        /// 时间：2016/6/7 22:47
        /// 备注：
        public event EventHandler<SocketServerStopedEventArgs> OnServerStoped;

        #endregion Events

        #region Properties

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections
        {
            get
            {
                return currentConnections;
            }
        }

        /// <summary>
        /// IPEndPoint
        /// </summary>
        public IPEndPoint Endpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Socket最多可容纳的等待接受的传入连接数
        /// 这个数不包含那些已经建立连接的数量。
        /// </summary>
        public int MaxQueuedConnections
        {
            get;
            set;
        }

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port
        {
            get;
            set;
        }

        /// <summary>
        /// 连接类型
        /// </summary>
        public SocketProtocol Protocol
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 适用于tcp回复
        /// </summary>
        /// <param name="datagram">数据报文</param>
        /// <param name="clientSocket">tcp终端</param>
        public void Reply(byte[] datagram, Socket clientSocket)
        {
            clientSocket.Send(datagram);
        }

        /// <summary>
        /// 适用于UDP回复
        /// </summary>
        /// <param name="datagram">数据报文</param>
        /// <param name="retmoteEndpoint">upd终端信息</param>
        public void Reply(byte[] datagram, IPEndPoint retmoteEndpoint)
        {
            listener.SendTo(datagram, retmoteEndpoint);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// 时间：2016/6/7 22:54
        /// 备注：
        /// <exception cref="System.InvalidOperationException">未能成功创建Socket服务类型。</exception>
        public void Start()
        {
            listener = GetCorrectSocket();
            #region 解决UDP错误10054:远程主机强迫关闭了一个现有的连接 
            if (Protocol == SocketProtocol.UDP)
            {
                uint IOC_IN = 0x80000000;
                uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                listener.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            #endregion
            if (listener != null)
            {
                listener.Bind(this.Endpoint);

                if (this.Protocol == SocketProtocol.TCP)
                {
                    listener.Listen(this.MaxQueuedConnections);
                    listener.BeginAccept(new AsyncCallback(ClientConnected), listener);
                }
                else if (this.Protocol == SocketProtocol.UDP)
                {
                    SocketConnectionInfo _connection = new SocketConnectionInfo();
                    _connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
                    _connection.Socket = listener;
                    ipeSender = new IPEndPoint(IPAddress.Any, this.Port);
                    listener.BeginReceiveFrom(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), _connection);
                }

                SocketServerStartedEventArgs _arg = new SocketServerStartedEventArgs
                {
                    Protocol = this.Protocol,
                    SocketServer = this.Endpoint,
                    StartedTime = DateTime.Now
                };
                OnServerStarted.RaiseEvent(this, _arg);
            }
            else
            {
                throw new InvalidOperationException("未能成功创建Socket服务类型。");
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// 时间：2016/6/7 22:54
        /// 备注：
        public void Stop()
        {
            listener.Close();
            SocketServerStopedEventArgs _arg = new SocketServerStopedEventArgs
            {
                Protocol = this.Protocol,
                SocketServer = this.Endpoint,
                StopedTime = DateTime.Now
            };
            OnServerStoped.RaiseEvent(this, _arg);
        }

        /// <summary>
        /// 处理终端异步连接
        /// </summary>
        /// <param name="asyncResult">IAsyncResult</param>
        /// 时间：2016/6/7 22:54
        /// 备注：
        internal void ClientConnected(IAsyncResult asyncResult)
        {
            Interlocked.Increment(ref currentConnections);
            SocketConnectionInfo _connection = new SocketConnectionInfo
            {
                Buffer = new byte[SocketConnectionInfo.BufferSize]
            };
            Socket _asyncListener = (Socket)asyncResult.AsyncState;
            Socket _asyncClient = _asyncListener.EndAccept(asyncResult);
            _connection.Socket = _asyncClient;
            OnClientConnected.RaiseEvent(null, CreateSocketSeesion(_connection, null));

            if (this.Protocol == SocketProtocol.TCP)
            {
                _asyncClient.BeginReceive(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), _connection);
            }
            else if (this.Protocol == SocketProtocol.UDP)
            {
                _asyncClient.BeginReceiveFrom(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), _connection);
            }

            listener.BeginAccept(new AsyncCallback(ClientConnected), _connection);
        }

        internal void ClientDisconnected(IAsyncResult asyncResult)
        {
            SocketConnectionInfo _connection = (SocketConnectionInfo)asyncResult.AsyncState;
            OnClientDisconnected.RaiseEvent(null, CreateSocketSeesion(_connection, null));
            _connection.Socket.EndDisconnect(asyncResult);
        }

        /// <summary>
        /// 处理数据异步接收
        /// </summary>
        /// <param name="asyncResult">IAsyncResult</param>
        /// 时间：2016/6/7 22:56
        /// 备注：
        internal void DataReceived(IAsyncResult asyncResult)
        {
            try
            {
                SocketConnectionInfo _connection = (SocketConnectionInfo)asyncResult.AsyncState;
                int _bytesRead = CaluDataReceivedReadLength(_connection, asyncResult);
                _connection.BytesRead += _bytesRead;

                if (IsSocketConnected(_connection.Socket))
                {
                    HanlderIsSocketConnectedDataReceived(_connection, _bytesRead, asyncResult);
                }
                else if (_connection.BytesRead > 0)
                {
                    HanlderSpecialDataReceived(_connection, _bytesRead, asyncResult);
                }
                else
                {
                    HanlderTcpClientDisconnected(_connection, _bytesRead, asyncResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 处理tcp client断开事件
        /// </summary>
        /// <param name="connection">SocketConnectionInfo</param>
        internal void DisconnectClient(SocketConnectionInfo connection)
        {
            OnClientDisconnecting.RaiseEvent(null, CreateSocketSeesion(connection, null));

            if (connection.Socket != null)
                connection.Socket.BeginDisconnect(true, new AsyncCallback(ClientDisconnected), connection);
        }

        /// <summary>
        /// 获取socket
        /// </summary>
        /// <returns>Socket</returns>
        internal Socket GetCorrectSocket()
        {
            if (this.Protocol == SocketProtocol.TCP)
            {
                return new Socket(this.Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            else if (this.Protocol == SocketProtocol.UDP)
            {
                return new Socket(this.Endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断是否终端是否断开链接
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <returns>是否断开链接</returns>
        internal bool IsSocketConnected(Socket socket)
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }

        /// <summary>
        /// 计算DataReceived读取长度
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        private int CaluDataReceivedReadLength(SocketConnectionInfo connection, IAsyncResult asyncResult)
        {
            int _bytesRead;

            switch (this.Protocol)
            {
                case SocketProtocol.UDP:
                    _bytesRead = connection.Socket.EndReceiveFrom(asyncResult, ref ipeSender);
                    break;

                case SocketProtocol.TCP:
                    _bytesRead = connection.Socket.EndReceive(asyncResult);
                    break;

                default:
                    _bytesRead = 0;
                    break;
            }

            return _bytesRead;
        }

        /// <summary>
        /// 创建SocketSeesionEventArgs 参数
        /// </summary>
        private SocketSeesionEventArgs CreateSocketSeesion(SocketConnectionInfo connect, byte[] buffer)
        {
            SocketSeesionEventArgs _arg = new SocketSeesionEventArgs();
            _arg.Socket = connect.Socket;
            _arg.Buffer = buffer;
            _arg.Protocol = this.Protocol;

            switch (this.Protocol)
            {
                case SocketProtocol.TCP:
                    _arg.DeviceInfo = (IPEndPoint)connect.Socket.RemoteEndPoint;
                    break;

                case SocketProtocol.UDP:
                    _arg.DeviceInfo = (IPEndPoint)ipeSender;
                    break;
            }

            return _arg;
        }

        /// <summary>
        /// 处理有效的socket数据接收
        /// </summary>
        private void HanlderIsSocketConnectedDataReceived(SocketConnectionInfo connection, int bytesRead, IAsyncResult asyncResult)
        {
            if (bytesRead == 0 || (bytesRead > 0 && bytesRead < SocketConnectionInfo.BufferSize))
            {
                byte[] _buffer = connection.Buffer;
                int _totalBytesRead = connection.BytesRead;
                connection = new SocketConnectionInfo();
                connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
                connection.Socket = ((SocketConnectionInfo)asyncResult.AsyncState).Socket;

                if (this.Protocol == SocketProtocol.UDP)
                {
                    connection.Socket.BeginReceiveFrom(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), connection);
                }
                else if (this.Protocol == SocketProtocol.TCP)
                {
                    connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), connection);
                }

                if (_totalBytesRead < _buffer.Length)
                {
                    Array.Resize<byte>(ref _buffer, _totalBytesRead);
                }

                OnDataReceived.RaiseEvent(null, CreateSocketSeesion(connection, _buffer));

                _buffer = null;
            }
            else
            {
                Array.Resize<Byte>(ref connection.Buffer, connection.Buffer.Length + SocketConnectionInfo.BufferSize);

                if (this.Protocol == SocketProtocol.UDP)
                {
                    connection.Socket.BeginReceiveFrom(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), connection);
                }
                else if (this.Protocol == SocketProtocol.TCP)
                {
                    connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), connection);
                }
            }
        }

        /// <summary>
        /// 处理特殊的数据接受，buffer大于零的情况
        /// </summary>
        private void HanlderSpecialDataReceived(SocketConnectionInfo connection, int bytesRead, IAsyncResult asyncResult)
        {
            Array.Resize<byte>(ref connection.Buffer, connection.BytesRead);

            OnDataReceived.RaiseEvent(null, CreateSocketSeesion(connection, connection.Buffer));
        }

        /// <summary>
        /// 处理tcp 终端连接断开
        /// </summary>
        private void HanlderTcpClientDisconnected(SocketConnectionInfo connection, int bytesRead, IAsyncResult asyncResult)
        {
            if (OnClientDisconnected != null)
            {
                DisconnectClient(connection);
            }
        }

        #endregion Methods
    }
}