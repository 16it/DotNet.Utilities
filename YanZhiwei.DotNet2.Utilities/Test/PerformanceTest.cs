namespace YanZhiwei.DotNet2.Utilities.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    
    using Model;
    
    /// <summary>
    /// 程序性能测试类
    /// </summary>
    public class PerformanceTest
    {
        #region Fields
        
        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime beginTime;
        
        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime endTime;
        
        /// <summary>
        /// 测试参数
        /// </summary>
        private PerformanceParams performance;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executeCount">执行次数</param>
        /// <param name="isMultithread">是否多线程</param>
        /// 时间：2016/11/10 11:14
        /// 备注：
        public PerformanceTest(int executeCount, bool isMultithread)
        {
            performance = new PerformanceParams()
            {
                RunCount = executeCount,
                IsMultithread = isMultithread
            };
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceTest()
        : this(1, false)
        {
        }
        
        #endregion Constructors
        
        #region Methods
        
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="testFactory">测试委托</param>
        /// <param name="resultFactory">结果委托</param>
        public void Execute(Action<int> testFactory, Action<string> resultFactory)
        {
            /*
            PerformanceTest p = new PerformanceTest();
            p.SetCount(10);//循环次数(默认:1)
            p.SetIsMultithread(true);//是否启动多线程测试 (默认:false)
            p.Execute(
            i =>{
            //需要测试的代码
            Response.Write(i+"<br>");
            System.Threading.Thread.Sleep(1000);
            },
            message =>
            {
            //输出总共运行时间
            Response.Write(message);   //总共执行时间：1.02206秒
            }
            );
            */
            List<Thread> _testThreads = new List<Thread>();
            beginTime = DateTime.Now;
            
            for(int i = 0; i < performance.RunCount; i++)
            {
                if(performance.IsMultithread)
                {
                    Thread _thread = new Thread(new ThreadStart(() =>
                    {
                        testFactory(i);
                    }));
                    _thread.Start();
                    _testThreads.Add(_thread);
                }
                else
                {
                    testFactory(i);
                }
            }
            
            if(performance.IsMultithread)
            {
                foreach(Thread t in _testThreads)
                {
                    while(t.IsAlive)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            
            resultFactory(GetResult());
        }
        
        /// <summary>
        /// 获取测试结果
        /// </summary>
        /// <returns>测试结果</returns>
        private string GetResult()
        {
            endTime = DateTime.Now;
            string _totalTime = ((endTime - beginTime).TotalMilliseconds / 1000.0).ToString("n5");
            return string.Format("总共执行时间：{0}秒", _totalTime);
        }
        
        #endregion Methods
    }
}