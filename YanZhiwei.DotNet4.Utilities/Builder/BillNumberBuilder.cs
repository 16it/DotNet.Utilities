using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        /// 时间：2016/9/29 13:54
        /// 备注：
        public static string NextBillNumber(Func<ulong> nextBillNumberFactory, string prefix = "")
        {
            lock(locker)
            {
                string str = DateTime.Now.ToShortDateString();
                DateTime time1 = Convert.ToDateTime(str + " 0:00:00");
                DateTime time2 = Convert.ToDateTime(str + " 23:59:59");
                var todayTotal = nextBillNumberFactory();
                return prefix + DateTime.Now.ToString("yyMMddfff") + (todayTotal + 1).ToString().PadLeft(5, '0');
            }
        }
    }
}
