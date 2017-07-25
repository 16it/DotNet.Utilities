namespace YanZhiwei.DotNet.Core.Infrastructure
{
    /// <summary>
    /// 在启动时由任务运行的接口
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        ///执行的接口
        /// </summary>
        void Execute();

        /// <summary>
        /// 顺序
        /// </summary>
        int Order { get; }
    }
}