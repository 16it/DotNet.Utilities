namespace YanZhiwei.DotNet2.Utilities.Info
{
    using System.Threading;

    /// <summary>
    ///  ThreadPool 信息
    /// </summary>
    public class ThreadPoolInfo
    {
        #region Properties

        /// <summary>
        /// 线程池中的活动工作线程数
        /// </summary>
        public static int ActiveThreadNumber
        {
            get
            {
                int _maxNumber, _availableNumber, _ioNumber;
                ThreadPool.GetMaxThreads(out _maxNumber, out _ioNumber);
                ThreadPool.GetAvailableThreads(out _availableNumber, out _ioNumber);
                return _maxNumber - _availableNumber;
            }
        }

        /// <summary>
        /// 线程池中的可用工作线程数
        /// </summary>
        public static int AvailableThreadNumber
        {
            get
            {
                int _availableNumber, _ioNumber;
                ThreadPool.GetAvailableThreads(out _availableNumber, out _ioNumber);
                return _availableNumber;
            }
        }

        /// <summary>
        /// 获取线程池中的最大工作线程数
        /// </summary>
        public static int MaxThreadNumber
        {
            get
            {
                int _maxNumber, _ioNumber;
                ThreadPool.GetMaxThreads(out _maxNumber, out _ioNumber);
                return _maxNumber;
            }
        }

        #endregion Properties
    }
}