namespace YanZhiwei.DotNet4.Utilities.Collection
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// 队列管理
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public sealed class QueueManager<T>
        where T : class, new()
    {
        #region Fields

        /// <summary>
        /// 定时读取队列委托
        /// </summary>
        public Action<List<T>> TimeReadBatchList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="taskMinute">定时读取队列数据分钟</param>
        public QueueManager(int taskMinute)
        {
            QueueCollection = new ConcurrentQueue<T>();
            StartDequeueTask(taskMinute);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 队列，线程安全
        /// </summary>
        public ConcurrentQueue<T> QueueCollection
        {
            get; private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 数据入队
        /// </summary>
        /// <param name="item">数据对象</param>
        public void Add(T item)
        {
            QueueCollection.Enqueue(item);
        }

        private void StartDequeueTask(int taskMinute)
        {
            ThreadPool.QueueUserWorkItem((arg) =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(taskMinute));
                    if (QueueCollection.Count > 0)
                    {
                        T _item = null;
                        List<T> _batchList = new List<T>();
                        while (QueueCollection.TryDequeue(out _item))
                        {
                            _batchList.Add(_item);
                        }
                        if (_batchList.Count > 0 && TimeReadBatchList != null)
                        {
                            TimeReadBatchList(_batchList);
                        }
                    }
                }
            });
        }

        #endregion Methods
    }
}