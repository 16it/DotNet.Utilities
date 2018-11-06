namespace YanZhiwei.DotNet2.Utilities.Collection
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Queue线程安全实现的帮助类
    /// 说明
    /// 默认读锁超时1000毫秒
    /// 默认写锁超时1000毫秒
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class ThreadSafeQueue<T>
    {
        #region Fields

        /* 参考资料
         * 参考：
         * 1. http://www.codeproject.com/Articles/38908/Thread-Safe-Generic-Queue-Class
         * 2. http://stackoverflow.com/questions/13416889/thread-safe-queue-enqueue-dequeue
         * 3. http://blogs.msdn.com/b/jaredpar/archive/2009/02/16/a-more-usable-thread-safe-collection.aspx
         */

        /// <summary>
        /// 默认读锁超时1000毫秒
        /// </summary>
        private readonly int _readerTimeout = 1000;

        /// <summary>
        /// ReaderWriterLock 对象
        /// </summary>
        private readonly ReaderWriterLock _rwlock = new ReaderWriterLock();

        /// <summary>
        /// 泛型Queue对象
        /// </summary>
        private readonly Queue<T> _storeQueue;

        /// <summary>
        /// 默认写锁超时1000毫秒
        /// </summary>
        private readonly int _writerTimeout = 1000;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ThreadSafeQueue()
        {
            _storeQueue = new Queue<T>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capacity">初始容量</param>
        public ThreadSafeQueue(int capacity)
        {
            _storeQueue = new Queue<T>(capacity);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection">IEnumerable</param>
        public ThreadSafeQueue(IEnumerable<T> collection)
        {
            _storeQueue = new Queue<T>(collection);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Count 【线程安全】
        /// </summary>
        /// <returns>queue 数量</returns>
        public int Count()
        {
            _rwlock.AcquireReaderLock(_readerTimeout);

            try
            {
                return _storeQueue.Count;
            }
            finally
            {
                _rwlock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Dequeue【线程安全】
        /// </summary>
        /// <returns>泛型</returns>
        public T Dequeue()
        {
            _rwlock.AcquireReaderLock(_readerTimeout);

            try
            {
                return _storeQueue.Dequeue();
            }
            finally
            {
                _rwlock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// DequeueAll【线程安全】
        /// </summary>
        /// <returns>IList</returns>
        public IList<T> DequeueAll()
        {
            _rwlock.AcquireReaderLock(_readerTimeout);

            try
            {
                IList<T> _list = new List<T>();

                while (_storeQueue.Count > 0)
                {
                    _list.Add(_storeQueue.Dequeue());
                }

                return _list;
            }
            finally
            {
                _rwlock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Enqueue【线程安全】
        /// </summary>
        /// <param name="item">泛型</param>
        public void Enqueue(T item)
        {
            _rwlock.UpgradeToWriterLock(_writerTimeout);

            try
            {
                _storeQueue.Enqueue(item);
            }
            finally
            {
                _rwlock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// EnqueueAll【线程安全】
        /// </summary>
        /// <param name="itemsToQueue">IEnumerable</param>
        public void EnqueueAll(IEnumerable<T> itemsToQueue)
        {
            _rwlock.UpgradeToWriterLock(_writerTimeout);

            try
            {
                foreach (T item in itemsToQueue)
                {
                    _storeQueue.Enqueue(item);
                }
            }
            finally
            {
                _rwlock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// EnqueueAll【线程安全】
        /// </summary>
        /// <param name="itemsToQueue">IList</param>
        public void EnqueueAll(IList<T> itemsToQueue)
        {
            _rwlock.UpgradeToWriterLock(_writerTimeout);

            try
            {
                foreach (T item in itemsToQueue)
                {
                    _storeQueue.Enqueue(item);
                }
            }
            finally
            {
                _rwlock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// GetEnumerator【线程安全】
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            Queue<T> _tmpQueue;
            _rwlock.AcquireReaderLock(_readerTimeout);

            try
            {
                _tmpQueue = new Queue<T>(_storeQueue);
            }
            finally
            {
                _rwlock.ReleaseReaderLock();
            }

            foreach (T item in _tmpQueue)
            {
                yield return item;
            }
        }

        #endregion Methods
    }
}