using System;
using System.Threading;
using YanZhiwei.DotNet2.Utilities.Args;
using YanZhiwei.DotNet2.Utilities.Collection;

namespace YanZhiwei.DotNet2.UtilitiesExamples
{
    public class WorkQueueExample
    {
        private static void Main(string[] args)
        {
            try
            {
                WorkQueue<int> workQueue = new WorkQueue<int>(1000, false);
                workQueue.OnUserWorkHandlerEvent += WorkQueue_OnUserWorkHandlerEvent;
                ThreadPool.QueueUserWorkItem(o =>
                {
                    for(int i = 0; i < 1000; i++)
                    {
                        workQueue.EnqueueItem(i);
                    }
                });
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
        
        private static void WorkQueue_OnUserWorkHandlerEvent(object sender, EnqueueEventArgs<int> e)
        {
            Console.WriteLine("ThreadId:" + Thread.CurrentThread.ManagedThreadId + "，Item:" + e.Item);
        }
    }
}