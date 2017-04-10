using System;
using YanZhiwei.DotNet4.Utilities.EventHandle;

namespace YanZhiwei.DotNet4.UtilitiesExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CustomizeEventSubscriberConfiguration.Initialize();
                CustomizeEventPublisher<GoodsSoldOut>.Instance().Publish(new GoodsSoldOut(Guid.Empty, Guid.Empty));
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
    }
}