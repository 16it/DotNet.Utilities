namespace YanZhiwei.DotNet.Core.MQ
{
    /// <summary>
    /// 消息队列生产者
    /// </summary>
    public interface IMQProducer
    {
        /// <summary>
        /// 打开
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 向队列发送数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        void SendMessage<T>(T data);

        /// <summary>
        /// 向指定队列发送数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="queueName">指定队列名</param>
        void SendMessage<T>(T data, string queueName);
    }
}