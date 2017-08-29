namespace YanZhiwei.DotNet2.Utilities.Enum
{
    #region Enumerations

    /// <summary>
    /// 具体错误类型
    /// </summary>
    public enum TcpOperateEventCode
    {
        /// <summary>
        /// 对象为null
        /// </summary>
        ObjectNull,

        /// <summary>
        /// 连接时发生错误
        /// </summary>
        ConnectError,

        /// <summary>
        /// 连接成功.
        /// </summary>
        ConnectSuccess,

        /// <summary>
        /// 没有客户端连接
        /// </summary>
        NoClinets,

        /// <summary>
        /// 服务启动成功
        /// </summary>
        ServerrStartSucceed,

        /// <summary>
        /// 服务启动失败
        /// </summary>
        ServerStartError,

        /// <summary>
        /// 服务停止
        /// </summary>
        ServerStop,

        /// <summary>
        /// 发送消息失败
        /// </summary>
        SendDataError,

        /// <summary>
        /// 新的客户端连接
        /// </summary>
        NewClientConnected,

        /// <summary>
        /// 新的客户端连接错误
        /// </summary>
        NewClientConnectError,

        /// <summary>
        ///移除客户端连接
        /// </summary>
        RemoveClientConnect,

        /// <summary>
        ///未连接上Server
        /// </summary>
        UnConnectServer,

        /// <summary>
        ///数据接收
        /// </summary>
        DataReceived,

        /// <summary>
        /// 数据接收错误
        /// </summary>
        DataReceivedError,

        /// <summary>
        ///Client尚未初始化
        /// </summary>
        ClientUninitialized,

        /// <summary>
        ///Client断开连接
        /// </summary>
        CliendDisconnected,

        /// <summary>
        /// 客户端下线
        /// </summary>
        ClientOffline,

        /// <summary>
        /// Server关闭
        /// </summary>
        ServerClose
    }

    #endregion Enumerations
}