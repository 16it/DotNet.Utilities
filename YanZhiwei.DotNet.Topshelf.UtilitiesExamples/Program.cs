using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using YanZhiwei.DotNet.Topshelf.Utilities;

namespace YanZhiwei.DotNet.Topshelf.UtilitiesExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            TopshelfHelper<TownCrier> _server = new TopshelfHelper<TownCrier>("测试", "TopShelf服务测试", "TopShelf服务测试");
            _server.SetRunAsLocalSystem();
            _server.SerivceStarted += _server_SerivceStarted;
            _server.SerivceStoped += _server_SerivceStoped;
            _server.StartService();
            Console.ReadLine();

        }

        private static void _server_SerivceStoped(TownCrier obj)
        {
            
        }

        private static void _server_SerivceStarted(TownCrier obj)
        {
           
        }
    }

    public class TownCrier
    {
        readonly Timer _timer;
        public TownCrier()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} and all is well", DateTime.Now);
        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }

}
