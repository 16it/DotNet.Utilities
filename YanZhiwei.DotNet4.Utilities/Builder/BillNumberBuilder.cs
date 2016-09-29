using System;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet4.Utilities.Builder
{
    /// <summary>
    /// 订单号创建
    /// </summary>
    /// 时间：2016/9/29 13:53
    /// 备注：
    public class BillNumberBuilder
    {
        private static object locker = new object();
        
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="nextBillNumberFactory">委托，参数：一天开始时间，一天结束时间</param>
        /// <param name="prefix">订单前缀</param>
        /// <returns>订单号</returns>
        /// 时间：2016/9/29 14:08
        /// 备注：
        public static string NextBillNumber(Func<DateTime, DateTime, ulong> nextBillNumberFactory, string prefix = "")
        {
            lock(locker)
            {
                ulong _todayTotal = nextBillNumberFactory(DateTime.Now.StartOfDay(), DateTime.Now.EndOfDay());
                return string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyMMddfff"), (_todayTotal + 1).ToString().PadLeft(5, '0'));
            }
        }
    }
}