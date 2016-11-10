namespace YanZhiwei.DotNet2.Utilities.Collection
{
    using Args;
    using System.Collections.Generic;
    using System.Threading;
    
    /// <summary>
    /// 工作队列
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// 时间：2016/11/10 15:45
    /// 备注：
    public class WorkQueue<T>
    {
        #region Fields
        
        /// <summary>
        /// 队列处理是否需要单线程顺序执行
        /// ture表示单线程处理队列的T对象
        /// 默认为false，表明按照顺序出队
        /// </summary>
        public readonly bool WorkSequential;
        
        private static readonly object isWorkingLooker = new object(); //对IsWorking的同步对象
        private static readonly object looker = new object(); //队列同步对象
        
        private bool IsWorking; //表明处理线程是否正在工作
        private Queue<T> queue; //实际的队列
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count">队列容量</param>
        /// <param name="isOneThread">是否单线程处理</param>
        public WorkQueue(int count, bool isOneThread)
        {
            queue = count > 0 ? new Queue<T>(count) : new Queue<T>();
            WorkSequential = isOneThread;
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkQueue()
        : this(0, false)
        {
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOneThread">是否单线程处理</param>
        public WorkQueue(bool isOneThread)
        : this(0, isOneThread)
        {
        }
        
        #endregion Constructors
        
        #region Delegates
        
        /// <summary>
        /// 工作队列处理委托
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">EnqueueEventArgs</param>
        public delegate void UserWorkHandler(object sender, EnqueueEventArgs<T> e);
        
        #endregion Delegates
        
        #region Events
        
        /// <summary>
        /// 工作队列处理业务事件
        /// </summary>
        public event UserWorkHandler OnUserWorkHandlerEvent;
        
        #endregion Events
        
        #region Methods
        
        /// <summary>
        /// 向工作队列添加对象，
        /// 对象添加以后，如果已经绑定工作的事件
        /// 会触发事件处理程序，对item对象进行处理
        /// </summary>
        /// <param name="item">添加到队列的对象</param>
        public void EnqueueItem(T item)
        {
            lock(looker)
            {
                queue.Enqueue(item);
            }
            
            lock(isWorkingLooker)
            {
                if(!IsWorking)
                {
                    IsWorking = true;
                    ThreadPool.QueueUserWorkItem(doUserWork);
                }
            }
        }
        
        /// <summary>
        /// 谨慎使用此函数，
        /// 只保证此瞬间，队列值为空
        /// </summary>
        /// <returns>队列是否为空</returns>
        public bool IsEmpty()
        {
            lock(looker)
            {
                return queue.Count == 0;
            }
        }
        
        /// <summary>
        /// 处理队列中对象的
        /// </summary>
        /// <param name="o"></param>
        private void doUserWork(object o)
        {
            while(!IsEmpty())
            {
                try
                {
                    lock(isWorkingLooker)
                    {
                        T _item = queue.Dequeue();
                        
                        if(OnUserWorkHandlerEvent != null)
                        {
                            if(WorkSequential)
                            {
                                OnUserWorkHandlerEvent(this, new EnqueueEventArgs<T>(_item));
                            }
                            else
                            {
                                ThreadPool.QueueUserWorkItem(item =>
                                {
                                    OnUserWorkHandlerEvent(this, new EnqueueEventArgs<T>((T)item));
                                }, _item);
                            }
                        }
                    }
                }
                finally
                {
                    lock(isWorkingLooker)
                    {
                        IsWorking = false;
                    }
                }
            }
            
            //try
            //{
            //    T item;
            //    while(true)
            //    {
            //        lock(looker)
            //        {
            //            if(queue.Count > 0)
            //            {
            //                item = queue.Dequeue();
            //            }
            //            else
            //            {
            //                return;
            //            }
            //        }
            //        if(!item.Equals(default(T)))
            //        {
            //            if(WorkSequential)
            //            {
            //                if(OnUserWorkHandlerEvent != null)
            //                {
            //                    OnUserWorkHandlerEvent(this, new EnqueueEventArgs<T>(item));
            //                }
            //            }
            //            else
            //            {
            //                ThreadPool.QueueUserWorkItem(obj =>
            //                {
            //                    if(OnUserWorkHandlerEvent != null)
            //                    {
            //                        OnUserWorkHandlerEvent(this, new EnqueueEventArgs<T>((T)obj));
            //                    }
            //                }, item);
            //            }
            //        }
            //    }
            //}
            //finally
            //{
            //    lock(isWorkingLooker)
            //    {
            //        IsWorking = false;
            //    }
            //}
        }
        
        #endregion Methods
    }
}