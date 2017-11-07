namespace YanZhiwei.DotNet4.Utilities.Collection
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// 周期性执行的队列
    /// </summary>
    public class ScheduleExecQueueManager<T>
    {
        #region Fields

        /// <summary>
        /// 定时读取队列委托
        /// </summary>
        public Action<List<T>> OnDequeueTimer;

        /// <summary>
        /// 同步锁对象
        /// </summary>
        private static readonly object syncRoot = new object();

        private Timer ScheduleExecuTimer = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScheduleExecQueueManager()
        {
            QueueCollection = new ConcurrentQueue<T>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 队列，线程安全
        /// </summary>
        public ConcurrentQueue<T> QueueCollection
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 数据入队
        /// </summary>
        /// <param name="item">数据对象</param>
        /// <param name="scheduleExecTime">下次执行时间</param>
        public void Add(T item, DateTime scheduleExecTime)
        {
            lock(syncRoot)
            {
                if(QueueCollection.Count == 0)
                {
                    Debug.WriteLine(string.Format("{0}，开启队列任务。", DateTime.Now));
                    int _exec = (int)(scheduleExecTime - DateTime.Now).TotalMilliseconds;

                    if(ScheduleExecuTimer == null)
                        ScheduleExecuTimer = new Timer(ScheduleExecuDequeue, null, _exec, Timeout.Infinite);

                    else
                        ScheduleExecuTimer.Change(_exec, Timeout.Infinite);
                }

                QueueCollection.Enqueue(item);
            }
        }

        private void ScheduleExecuDequeue(object args)
        {
            if(QueueCollection.Count > 0)
            {
                Debug.WriteLine(string.Format("{0}，开始出队。", DateTime.Now));
                T _item = default(T);
                List<T> _batchList = new List<T>();

                while(QueueCollection.TryDequeue(out _item))
                {
                    _batchList.Add(_item);
                }

                if(_batchList.Count > 0 && OnDequeueTimer != null)
                {
                    OnDequeueTimer(_batchList);
                }
            }
        }

        #endregion Methods
    }
}