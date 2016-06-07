using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using YanZhiwei.DotNet2.Utilities.Enums;
using YanZhiwei.DotNet2.Utilities.Models;

namespace YanZhiwei.DotNet2.Utilities.Net
{
    public sealed class HighPerformanceServer
    {
        private int currentConnections = 0;
        private Socket listener;
        private EndPoint ipeSender;

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections { get { return currentConnections; } }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxQueuedConnections { get; set; }

        /// <summary>
        /// IPEndPoint
        /// </summary>
        public IPEndPoint Endpoint { get; set; }

        /// <summary>
        /// 连接类型
        /// </summary>
        public ServerType Type { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// 时间：2016/6/7 11:35
        /// 备注：
        private HighPerformanceServer()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="ipAddress">ip地址</param>
        /// 时间：2016/6/7 11:35
        /// 备注：
        public HighPerformanceServer(ServerType type, string ipAddress) : this(type, ipAddress, 9888)
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
        public HighPerformanceServer(ServerType type, string ipAddress, ushort port)
        {
            Init(type, ipAddress, port);
        }

        /// <summary>
        /// Initializes the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="port">The port.</param>
        /// 时间：2016/6/7 11:37
        /// 备注：
        /// <exception cref="ArgumentException">
        /// The argument 'Port' is not valid. Please select a value greater than 100.
        /// or
        /// The argument 'IpAddress' is not valid
        /// or
        /// The argument 'ServerType' is not valid
        /// </exception>
        private void Init(ServerType server, string ipAddress, ushort port)
        {
            IPAddress ip;
            // Check the IpAddress to make sure that it is valid
            if (!String.IsNullOrEmpty(ipAddress) && IPAddress.TryParse(ipAddress, out ip))
            {
                this.Endpoint = new IPEndPoint(ip, port);
                // Make sure that the port is greater than 100 as not to conflict with any other programs
                if (port < 100)
                {
                    throw new ArgumentException("The argument 'Port' is not valid. Please select a value greater than 100.");
                }
                else
                {
                    this.Port = port;
                }
            }
            else
            {
                throw new ArgumentException("The argument 'IpAddress' is not valid");
            }
            // We never want a ServerType of None, but we include it as it is recommended by FXCop.
            if (server != ServerType.None)
            {
                this.Type = server;
            }
            else
            {
                throw new ArgumentException("The argument 'ServerType' is not valid");
            }
        }

        public event EventHandler<EventArgs> OnServerStart;

        public event EventHandler<EventArgs> OnServerStarted;

        public event EventHandler<EventArgs> OnServerStopping;

        public event EventHandler<EventArgs> OnServerStoped;

        public event EventHandler<EventArgs> OnClientConnected;

        public event EventHandler<EventArgs> OnClientDisconnecting;

        public event EventHandler<EventArgs> OnClientDisconnected;

        public event EventHandler<EventArgs> OnDataReceived;

        public void Start()
        {
            // Tell anything that is listening that we have starting to work
            if (OnServerStart != null)
            {
                OnServerStart(this, null);
            }

            // Get either a TCP or UDP socket depending on what we specified when we created the class
            listener = GetCorrectSocket();

            if (listener != null)
            {
                // Bind the socket to the endpoint
                listener.Bind(this.Endpoint);

                // TODO :: Add throttleling (using SEMAPHORE's)

                if (this.Type == ServerType.TCP)
                {
                    // Start listening to the socket, accepting any backlog
                    listener.Listen(this.MaxQueuedConnections);

                    // Use the BeginAccept to accept new clients
                    listener.BeginAccept(new AsyncCallback(ClientConnected), listener);
                }
                else if (this.Type == ServerType.UDP)
                {
                    // So we can buffer and store information, create a new information class
                    SocketConnectionInfo connection = new SocketConnectionInfo();
                    connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
                    connection.Socket = listener;
                    // Setup the IPEndpoint
                    ipeSender = new IPEndPoint(IPAddress.Any, this.Port);
                    // Start recieving from the client
                    listener.BeginReceiveFrom(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), connection);
                }

                // Tell anything that is listening that we have started to work
                if (OnServerStarted != null)
                {
                    OnServerStarted(this, null);
                }
            }
            else
            {
                // There was an error creating the correct socket
                throw new InvalidOperationException("Could not create the correct sever socket type.");
            }
        }

        internal Socket GetCorrectSocket()
        {
            if (this.Type == ServerType.TCP)
            {
                return new Socket(this.Endpoint.AddressFamily, System.Net.Sockets.SocketType.Stream, ProtocolType.Tcp);
            }
            else if (this.Type == ServerType.UDP)
            {
                return new Socket(this.Endpoint.AddressFamily, System.Net.Sockets.SocketType.Dgram, ProtocolType.Udp);
            }
            else
            {
                return null;
            }
        }

        public void Stop()
        {
            if (OnServerStopping != null)
            {
                OnServerStopping(this, null);
            }

            if (OnServerStoped != null)
            {
                OnServerStoped(this, null);
            }
        }

        internal void ClientConnected(IAsyncResult asyncResult)
        {
            // Increment our ConcurrentConnections counter
            Interlocked.Increment(ref currentConnections);

            // So we can buffer and store information, create a new information class
            SocketConnectionInfo connection = new SocketConnectionInfo();
            connection.Buffer = new byte[SocketConnectionInfo.BufferSize];

            // We want to end the async event as soon as possible
            Socket asyncListener = (Socket)asyncResult.AsyncState;
            Socket asyncClient = asyncListener.EndAccept(asyncResult);

            // Set the SocketConnectionInformations socket to the current client
            connection.Socket = asyncClient;

            // Tell anyone that's listening that we have a new client connected
            if (OnClientConnected != null)
            {
                OnClientConnected(this, null);
            }

            // TODO :: Add throttleling (using SEMAPHORE's)

            // Begin recieving the data from the client
            if (this.Type == ServerType.TCP)
            {
                asyncClient.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), connection);
            }
            else if (this.Type == ServerType.UDP)
            {
                asyncClient.BeginReceiveFrom(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), connection);
            }
            // Now we have begun recieving data from this client,
            // we can now accept a new client
            listener.BeginAccept(new AsyncCallback(ClientConnected), listener);
        }

        internal void DataReceived(IAsyncResult asyncResult)
        {
            try
            {
                SocketConnectionInfo connection = (SocketConnectionInfo)asyncResult.AsyncState;
                int bytesRead;
                // End the correct async process
                if (this.Type == ServerType.UDP)
                {
                    bytesRead = connection.Socket.EndReceiveFrom(asyncResult, ref ipeSender);
                }
                else if (this.Type == ServerType.TCP)
                {
                    bytesRead = connection.Socket.EndReceive(asyncResult);
                }
                else
                {
                    bytesRead = 0;
                }
                // Increment the counter of BytesRead
                connection.BytesRead += bytesRead;
                // Check to see whether the socket is connected or not...
                if (IsSocketConnected(connection.Socket))
                {
                    // If we have read no more bytes, raise the data received event
                    if (bytesRead == 0 || (bytesRead > 0 && bytesRead < SocketConnectionInfo.BufferSize))
                    {
                        byte[] buffer = connection.Buffer;
                        int totalBytesRead = connection.BytesRead;
                        // Setup the connection info again ready for another packet
                        connection = new SocketConnectionInfo();
                        connection.Buffer = new byte[SocketConnectionInfo.BufferSize];
                        connection.Socket = ((SocketConnectionInfo)asyncResult.AsyncState).Socket;
                        // Fire off the receive event as quickly as possible, then we can process the data...
                        if (this.Type == ServerType.UDP)
                        {
                            connection.Socket.BeginReceiveFrom(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), connection);
                        }
                        else if (this.Type == ServerType.TCP)
                        {
                            connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), connection);
                        }
                        // Remove any extra data
                        if (totalBytesRead < buffer.Length)
                        {
                            Array.Resize<Byte>(ref buffer, totalBytesRead);
                        }
                        // Now raise the event, sender will contain the buffer for now
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(buffer, null);
                        }
                        buffer = null;
                    }
                    else
                    {
                        // Resize the array ready for the next chunk of data
                        Array.Resize<Byte>(ref connection.Buffer, connection.Buffer.Length + SocketConnectionInfo.BufferSize);
                        // Fire off the receive event again, with the bigger buffer
                        if (this.Type == ServerType.UDP)
                        {
                            connection.Socket.BeginReceiveFrom(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, ref ipeSender, new AsyncCallback(DataReceived), connection);
                        }
                        else if (this.Type == ServerType.TCP)
                        {
                            connection.Socket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None, new AsyncCallback(DataReceived), connection);
                        }
                    }
                }
                else if (connection.BytesRead > 0)
                {
                    // We still have data
                    Array.Resize<Byte>(ref connection.Buffer, connection.BytesRead);
                    // call the event
                    if (OnDataReceived != null)
                    {
                        OnDataReceived(connection.Buffer, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal bool IsSocketConnected(Socket socket)
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }

        internal void DisconnectClient(SocketConnectionInfo connection)
        {
            if (OnClientDisconnecting != null)
            {
                OnClientDisconnecting(this, null);
            }
            connection.Socket.BeginDisconnect(true, new AsyncCallback(ClientDisconnected), connection);
        }

        internal void ClientDisconnected(IAsyncResult asyncResult)
        {
            SocketConnectionInfo sci = (SocketConnectionInfo)asyncResult;
            sci.Socket.EndDisconnect(asyncResult);
            if (OnClientDisconnected != null)
            {
                OnClientDisconnected(this, null);
            }
        }
    }
}