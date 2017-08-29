namespace YanZhiwei.DotNet2.Utilities.Communication
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    
    using Collection;
    
    using Model;
    
    using YanZhiwei.DotNet2.Utilities.Args;
    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.Enum;
    
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
        /// 当前IP地址
        /// </summary>
        private IPAddress ipaddress;
        
        /// <summary>
        /// 当前IP,端口对象
        /// </summary>
        private IPEndPoint ipEndPoint;
        
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool isStop = false;
        
        /// <summary>
        /// 服务端
        /// </summary>
        private TcpListener listener;
        
        /// <summary>
        /// 当前监听端口
        /// </summary>
        private int portNumber;
        
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
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        public TcpAppServer(IPAddress ip, int port)
        {
            TcpClientConnectList = new ThreadSafeList<TcpClientConnectSession>();
            ipaddress = ip;
            portNumber = port;
            listener = new TcpListener(ipaddress, portNumber);
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        public TcpAppServer(string ip, int port)
        {
            TcpClientConnectList = new ThreadSafeList<TcpClientConnectSession>();
            ipaddress = IPAddress.Parse(ip);
            portNumber = port;
            ipEndPoint = new IPEndPoint(ipaddress, portNumber);
            listener = new TcpListener(ipaddress, portNumber);
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// 客户端队列集合
        /// </summary>
        public ThreadSafeList<TcpClientConnectSession> TcpClientConnectList
        {
            get;
            set;
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
            if(TcpClientConnectList != null)
            {
                for(int i = 0; i < TcpClientConnectList.Count; i++)
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
            if(TcpClientConnectList != null)
            {
                TcpClientConnectSession _connectedSession = TcpClientConnectList.Find(o =>
                {
                    return o.Ip == ip;
                });
                
                if(_connectedSession != null)
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
            for(int i = 0; i < TcpClientConnectList.Count; i++)
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
            for(int i = 0; i < TcpClientConnectList.Count; i++)
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
                
                if(_connectedSession != null)
                {
                    if(_connectedSession.Client.Connected)
                    {
                        NetworkStream _stream = _connectedSession.SkStream;
                        
                        if(_stream.CanWrite)
                        {
                            byte[] _buffer = sendDataBuffer;
                            _stream.Write(_buffer, 0, _buffer.Length);
                        }
                        
                        else
                        {
                            _stream = _connectedSession.Client.GetStream();
                            
                            if(_stream.CanWrite)
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
            
            catch(Exception ex)
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
                
                if(_connectedSession != null)
                {
                    if(_connectedSession.Client.Connected)
                    {
                        NetworkStream _netStream = _connectedSession.SkStream;
                        
                        if(_netStream.CanWrite)
                        {
                            byte[] _buffer = Encoding.UTF8.GetBytes(sendData);
                            _netStream.Write(_buffer, 0, _buffer.Length);
                        }
                        
                        else
                        {
                            _netStream = _connectedSession.Client.GetStream();
                            
                            if(_netStream.CanWrite)
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
            
            catch(Exception ex)
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
                listener.Start();
                Thread _task = new Thread(new ThreadStart(delegate
                {
                    while(true)
                    {
                        if(isStop != false)
                        {
                            break;
                        }
                        
                        GetAcceptTcpClient();
                        Thread.Sleep(1);
                    }
                }));
                _task.Start();
                RaiseDataReceivedEvent(TcpOperateEvent.StartSucceed, null, null, ipEndPoint, null);
            }
            
            catch(SocketException ex)
            {
                RaiseDataReceivedEvent(TcpOperateEvent.StartError, null, ex, ipEndPoint, null);
            }
        }
        
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            if(listener != null)
            {
                SendToAll("ServerOff");
                listener.Stop();
                listener = null;
                isStop = true;
                RaiseDataReceivedEvent(TcpOperateEvent.Stop, null, null, ipEndPoint, null);
            }
        }
        
        /// <summary>
        /// Adds the client list.
        /// </summary>
        /// <param name="sk">SocketObj</param>
        private void AddClientList(TcpClientConnectSession sk)
        {
            TcpClientConnectSession _connectedSession = TcpClientConnectList.Find(o =>
            {
                return o.Ip == sk.Ip;
            });
            
            if(_connectedSession == null)
            {
                TcpClientConnectList.Add(sk);
            }
            
            else
            {
                TcpClientConnectList.Remove(_connectedSession);
                TcpClientConnectList.Add(sk);
            }
            
            RaiseDataReceivedEvent(TcpOperateEvent.NewClientConnect, null, null, sk.Ip, null);
        }
        
        /// <summary>
        /// 异步接收发送的信息.
        /// </summary>
        /// <param name="ir">IAsyncResult</param>
        private void EndReader(IAsyncResult ir)
        {
            TcpClientConnectSession _connectedSession = ir.AsyncState as TcpClientConnectSession;
            
            if(_connectedSession != null && listener != null)
            {
                try
                {
                    if(_connectedSession.NewClientFlag || _connectedSession.Offset != 0)
                    {
                        _connectedSession.NewClientFlag = false;
                        _connectedSession.Offset = _connectedSession.SkStream.EndRead(ir);
                        
                        if(_connectedSession.Offset != 0)
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
                
                catch(Exception ex)
                {
                    lock(syncRoot)
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
                TcpClient _tclient = listener.AcceptTcpClient();
                Socket _socket = _tclient.Client;
                NetworkStream _stream = new NetworkStream(_socket, true); //承载这个Socket
                TcpClientConnectSession _connectedSession = new TcpClientConnectSession(_tclient.Client.RemoteEndPoint as IPEndPoint, _tclient, _stream);
                _connectedSession.NewClientFlag = true;
                AddClientList(_connectedSession);
                _connectedSession.SkStream.BeginRead(recBuffer, 0, recBuffer.Length, new AsyncCallback(EndReader), _connectedSession);
                
                if(_stream.CanWrite)
                {
                }
                
                semap.Release();
            }
            
            catch(Exception ex)
            {
                semap.Release();
                RaiseDataReceivedEvent(TcpOperateEvent.NewClientConnectError, null, ex, null, null);
            }
        }
        
        /// <summary>
        /// 推送Server消息到终端
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
            OnDataReceived.RaiseEvent<TcpSeesionEventArgs>(this, _args);
        }
        
        #endregion Methods
    }
}