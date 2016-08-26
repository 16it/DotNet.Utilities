namespace YanZhiwei.DotNet2.Utilities.Communication
{
    using Enum;
    using Model;
    using Operator;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Socket 服务
    /// </summary>
    /// 时间：2016/6/7 22:38
    /// 备注：
    public sealed class HighPerformanceServer
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
        /// <param name="type">类型</param>
        /// <param name="ipAddress">ip地址</param>
        /// 时间：2016/6/7 11:35
        /// 备注：
        public HighPerformanceServer(TCPIPType type, string ipAddress)
        : this(type, ipAddress, 9888)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="port">端口</param>
        /// 时间：2016/6/7 11:35
        /// 备注：
        public HighPerformanceServer(TCPIPType type, string ipAddress, ushort port)
        {
            Init(type, ipAddress, port);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// 时间：2016/6/7 11:35
        /// 备注：
        private HighPerformanceServer()
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// 终端已经连接事件
        /// </summary>
        /// 时间：2016/6/7 22:47
        /// 备注：
        public event EventHandler<EventArgs> OnClientConnected;

        /// <summary>
        /// 终端已经断开连接事件
        /// </summary>
        /// 时间：2016/6/7 22:48
        /// 备注：
        public event EventHandler<EventArgs> OnClientDisconnected;

        /// <summary>
        /// 终端正在断开连接事件
        /// </summary>
        /// 时间：2016/6/7 22:48
        /// 备注：
        public event EventHandler<EventArgs> OnClientDisconnecting;

        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// 时间：2016/6/7 22:48
        /// 备注：
        public event EventHandler<EventArgs> OnDataReceived;

        /// <summary>
        /// 服务启动事件
        /// </summary>
        /// 时间：2016/6/7 22:46
        /// 备注：
        public event EventHandler<EventArgs> OnServerStart;

        /// <summary>
        /// 服务启动完成事件
        /// </summary>
        /// 时间：2016/6/7 22:46
        /// 备注：
        public event EventHandler<EventArgs> OnServerStarted;

        /// <summary>
        /// 服务已经停止事件
        /// </summary>
        /// 时间：2016/6/7 22:47
        /// 备注：
        public event EventHandler<EventArgs> OnServerStoped;

        /// <summary>
        /// 服务正在停止事件
        /// </summary>
        /// 时间：2016/6/7 22:46
        /// 备注：
        public event EventHandler<EventArgs> OnServerStopping;

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
        /// 最大连接数
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
        public TCPIPType Type
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 启动服务
        /// </summary>
        /// 时间：2016/6/7 22:54
        /// 备注：
        /// <exception cref="System.InvalidOperationException">未能成功创建Socket服务类型。</exception>
        public void Start()
        {
            if(OnServerStart != null)
            {
                OnServerStart(this, null);
            }

            listener = GetCorrectSocket();

            if(listener != null)
            {
                listener.Bind(this.Endpoint);

                if(this.Type == TCPIPType.TCP)
                {
                    listener.Listen(this.MaxQueuedConnections);
                    listener.BeginAccept(new AsyncCallback(ClientConnected), listener);
                }
                else if(this.Type == TCPIPType.UDP)
                {
                    SocketConnectionInfo _connection = new SocketConnectionInfo();
                    _connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
                    _connection.Socket = listener;
                    ipeSender = new IPEndPoint(IPAddress.Any, this.Port);
                    listener.BeginReceiveFrom(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), _connection);
                }

                if(OnServerStarted != null)
                {
                    OnServerStarted(this, null);
                }
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
            if(OnServerStopping != null)
            {
                OnServerStopping(this, null);
            }

            if(OnServerStoped != null)
            {
                OnServerStoped(this, null);
            }
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
            SocketConnectionInfo _connection = new SocketConnectionInfo();
            _connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
            Socket _asyncListener = (Socket)asyncResult.AsyncState;
            Socket _asyncClient = _asyncListener.EndAccept(asyncResult);
            _connection.Socket = _asyncClient;

            if(OnClientConnected != null)
            {
                OnClientConnected(this, null);
            }

            if(this.Type == TCPIPType.TCP)
            {
                _asyncClient.BeginReceive(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), _connection);
            }
            else if(this.Type == TCPIPType.UDP)
            {
                _asyncClient.BeginReceiveFrom(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), _connection);
            }

            listener.BeginAccept(new AsyncCallback(ClientConnected), listener);
        }

        internal void ClientDisconnected(IAsyncResult asyncResult)
        {
            SocketConnectionInfo _sci = (SocketConnectionInfo)asyncResult;
            _sci.Socket.EndDisconnect(asyncResult);

            if(OnClientDisconnected != null)
            {
                OnClientDisconnected(this, null);
            }
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
                int _bytesRead;

                if(this.Type == TCPIPType.UDP)
                {
                    _bytesRead = _connection.Socket.EndReceiveFrom(asyncResult, ref ipeSender);
                }
                else if(this.Type == TCPIPType.TCP)
                {
                    _bytesRead = _connection.Socket.EndReceive(asyncResult);
                }
                else
                {
                    _bytesRead = 0;
                }

                _connection.BytesRead += _bytesRead;

                if(IsSocketConnected(_connection.Socket))
                {
                    if(_bytesRead == 0 || (_bytesRead > 0 && _bytesRead < SocketConnectionInfo.BufferSize))
                    {
                        byte[] _buffer = _connection.Buffer;
                        int _totalBytesRead = _connection.BytesRead;
                        _connection = new SocketConnectionInfo();
                        _connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
                        _connection.Socket = ((SocketConnectionInfo)asyncResult.AsyncState).Socket;

                        if(this.Type == TCPIPType.UDP)
                        {
                            _connection.Socket.BeginReceiveFrom(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), _connection);
                        }
                        else if(this.Type == TCPIPType.TCP)
                        {
                            _connection.Socket.BeginReceive(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), _connection);
                        }

                        if(_totalBytesRead < _buffer.Length)
                        {
                            Array.Resize<byte>(ref _buffer, _totalBytesRead);
                        }

                        if(OnDataReceived != null)
                        {
                            OnDataReceived(_buffer, null);
                        }

                        _buffer = null;
                    }
                    else
                    {
                        Array.Resize<Byte>(ref _connection.Buffer, _connection.Buffer.Length + SocketConnectionInfo.BufferSize);

                        if(this.Type == TCPIPType.UDP)
                        {
                            _connection.Socket.BeginReceiveFrom(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), _connection);
                        }
                        else if(this.Type == TCPIPType.TCP)
                        {
                            _connection.Socket.BeginReceive(_connection.Buffer, 0, _connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), _connection);
                        }
                    }
                }
                else if(_connection.BytesRead > 0)
                {
                    Array.Resize<byte>(ref _connection.Buffer, _connection.BytesRead);

                    if(OnDataReceived != null)
                    {
                        OnDataReceived(_connection.Buffer, null);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal void DisconnectClient(SocketConnectionInfo connection)
        {
            if(OnClientDisconnecting != null)
            {
                OnClientDisconnecting(this, null);
            }

            connection.Socket.BeginDisconnect(true, new AsyncCallback(ClientDisconnected), connection);
        }

        internal Socket GetCorrectSocket()
        {
            if(this.Type == TCPIPType.TCP)
            {
                return new Socket(this.Endpoint.AddressFamily, System.Net.Sockets.SocketType.Stream, ProtocolType.Tcp);
            }
            else if(this.Type == TCPIPType.UDP)
            {
                return new Socket(this.Endpoint.AddressFamily, System.Net.Sockets.SocketType.Dgram, ProtocolType.Udp);
            }
            else
            {
                return null;
            }
        }

        internal bool IsSocketConnected(Socket socket)
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="server">ServerType</param>
        /// <param name="ipAddress">Ip地址</param>
        /// <param name="port">端口</param>
        /// 时间：2016/6/7 22:45
        /// 备注：
        /// <exception cref="System.ArgumentException">未能识别的Ip地址。</exception>
        private void Init(TCPIPType server, string ipAddress, ushort port)
        {
            ValidateOperator.Begin().NotNullOrEmpty(ipAddress, "Ip地址").IsIp(ipAddress, "Ip地址");
            IPAddress _ipAddress;

            if(IPAddress.TryParse(ipAddress, out _ipAddress))
            {
                this.Endpoint = new IPEndPoint(_ipAddress, port);
            }
            else
            {
                throw new ArgumentException("未能识别的Ip地址。");
            }

            this.Type = server;
            this.Port = port;
        }

        #endregion Methods
    }
}