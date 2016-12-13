namespace YanZhiwei.DotNet2.Utilities.Core
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    
    /// <summary>
    /// 交互重试机制
    /// </summary>
    /// 时间：2016/12/13 17:05
    /// 备注：
    public class InteractRetry : IDisposable
    {
        #region Fields
        
        private static readonly object looker = new object();
        
        private BackgroundWorker backgroupThread = null;
        private int retryCount = 1;
        private int waitTimeout = 0;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public InteractRetry()
        {
        }
        
        #endregion Constructors
        
        #region Delegates
        
        /// <summary>
        ///取消委托
        /// </summary>
        public delegate void CancelledEventHandler();
        
        /// <summary>
        /// 完成委托
        /// </summary>
        /// <param name="success">是否成功</param>
        public delegate void CompletedEventHandler(bool success);
        
        /// <summary>
        /// 每一次重试开始操作委托
        /// </summary>
        /// <param name="retryIndex">重试次数，从0开始</param>
        public delegate void PerRetryBeginEventHandler(int retryIndex);
        
        /// <summary>
        /// 每一次重试结束操作委托
        /// </summary>
        /// <param name="retryIndex">重试次数，从0开始</param>
        public delegate void PerRetryEndEventHandler(int retryIndex);
        
        /// <summary>
        /// 每一次重试失败委托
        /// </summary>
        /// <param name="ex">Exception为失败的异常信息</param>
        public delegate void PerRetryFailedEventHandler(System.Exception ex);
        
        /// <summary>
        /// 进度更改委托
        /// </summary>
        /// <param name="percent">百分比</param>
        public delegate void ProgressChangedEventHandler(int percent);
        
        #endregion Delegates
        
        #region Events
        
        /// <summary>
        /// 取消事件
        /// </summary>
        public event CancelledEventHandler Cancelled;
        
        /// <summary>
        /// 完成事件
        /// </summary>
        public event CompletedEventHandler Completed;
        
        /// <summary>
        /// 每一次重试开始操作事件
        /// </summary>
        public event PerRetryBeginEventHandler PerRetryBegin;
        
        /// <summary>
        /// 每一次重试结束操作事件
        /// </summary>
        public event PerRetryEndEventHandler PerRetryEnd;
        
        /// <summary>
        /// 每一次重试失败事件
        /// </summary>
        public event PerRetryFailedEventHandler PerRetryFailedCompleted;
        
        /// <summary>
        /// 进度更改事件
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;
        
        #endregion Events
        
        #region Properties
        
        /// <summary>
        /// 是否正在运行异步操作
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return this.backgroupThread != null && this.backgroupThread.IsBusy;
            }
        }
        
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount
        {
            get
            {
                return this.retryCount;
            }
            
            private set
            {
                if(value < 1)
                {
                    value = 1;
                }
                
                this.retryCount = value;
            }
        }
        
        /// <summary>
        /// 超时时间【毫秒】
        /// </summary>
        public int WaitTimeout
        {
            get
            {
                return this.waitTimeout;
            }
            
            private set
            {
                if(value < 0)
                {
                    value = 0;
                }
                
                this.waitTimeout = value;
            }
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 取消操作
        /// </summary>
        public void Cancel()
        {
            if(this.backgroupThread != null)
            {
                this.backgroupThread.CancelAsync();
            }
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// 时间：2016/12/13 17:25
        /// 备注：
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// 异步开始
        /// </summary>
        /// <param name="executeFacotry">目标委托</param>
        /// <param name="waitTimeout">超时时间【毫秒】</param>
        /// <param name="retryCount">重试次数</param>
        public void StartAsync(Action executeFacotry, int waitTimeout, int retryCount)
        {
            StartAsyncRetry(executeFacotry, waitTimeout, retryCount);
        }
        
        /// <summary>
        /// 异步开始
        /// </summary>
        /// <param name="executeFacotry">目标委托</param>
        /// <param name="waitTimeout">超时时间【毫秒】</param>
        /// <param name="retryCount">重试次数</param>
        /// 时间：2016/12/13 17:33
        /// 备注：
        public void StartAsyncFunc(Func<bool> executeFacotry, int waitTimeout, int retryCount)
        {
            StartAsyncRetry(executeFacotry, waitTimeout, retryCount);
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// 时间：2016/12/13 17:25
        /// 备注：
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                this.Cancel();
            }
            
            this.backgroupThread = null;
        }
        
        private void InvokeCompletedEvent(bool success = false)
        {
            if(this.Completed != null)
            {
                this.Completed(success);
            }
            
            this.backgroupThread.ReportProgress(100);
        }
        
        private void OnBackgroupThreadDoWork(object sender, DoWorkEventArgs e)
        {
            Start(e.Argument);
        }
        
        private void OnBackgroupThreadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(this.ProgressChanged != null)
            {
                this.ProgressChanged(e.ProgressPercentage);
            }
        }
        
        private void Start(object target)
        {
            if(target == null)
            {
                return;
            }
            
            int _retryCount = this.RetryCount;
            
            lock(looker)
            {
                this.backgroupThread.ReportProgress(5);
                
                while(!this.backgroupThread.CancellationPending)
                {
                    if(this.PerRetryBegin != null)
                    {
                        this.PerRetryBegin(this.RetryCount - _retryCount);
                    }
                    
                    try
                    {
                        if(target.GetType() == typeof(Action))
                        {
                            (target as Action).Invoke();
                            InvokeCompletedEvent(true);
                            return;
                        }
                        else
                        {
                            if((target as Func<bool>).Invoke())
                            {
                                InvokeCompletedEvent(true);
                                return;
                            }
                            else
                            {
                                throw new InvalidOperationException("Execute Failed.");
                            }
                        }
                    }
                    catch(System.Exception ex)
                    {
                        if(this.PerRetryFailedCompleted != null)
                        {
                            this.PerRetryFailedCompleted(ex);
                        }
                        
                        this.backgroupThread.ReportProgress((this.RetryCount - _retryCount + 1) * 100 / this.RetryCount);
                    }
                    finally
                    {
                        if(this.PerRetryEnd != null)
                        {
                            this.PerRetryEnd(this.RetryCount - _retryCount);
                        }
                    }
                    
                    if(this.RetryCount > 0)
                    {
                        _retryCount--;
                        
                        if(_retryCount == 0)
                        {
                            InvokeCompletedEvent();
                            return;
                        }
                    }
                    
                    Thread.Sleep(this.WaitTimeout);
                }
                
                if(this.backgroupThread.CancellationPending)
                {
                    if(this.Cancelled != null)
                    {
                        this.Cancelled();
                    }
                }
            }
        }
        
        private void StartAsyncRetry(object executeFacotry, int waitTimeout, int retryCount)
        {
            if(backgroupThread == null)
            {
                this.backgroupThread = new BackgroundWorker();
                this.backgroupThread.WorkerSupportsCancellation = true;
                this.backgroupThread.WorkerReportsProgress = true;
                this.backgroupThread.DoWork += OnBackgroupThreadDoWork;
                this.backgroupThread.ProgressChanged += OnBackgroupThreadProgressChanged;
            }
            
            if(this.backgroupThread.IsBusy)
            {
                return;
            }
            
            this.WaitTimeout = waitTimeout;
            this.RetryCount = retryCount;
            this.backgroupThread.RunWorkerAsync(executeFacotry);
        }
        
        #endregion Methods
    }
}