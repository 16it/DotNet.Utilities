using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YanZhiwei.DotNet4._5.Utilities.Common
{
    /// <summary>
    /// Enumerable 辅助类
    /// </summary>
    public static class EnumerableHelper
    {
        /// <summary>
        /// 异步分页处理数据
        /// </summary>
        /// <param name="item">需要分页的数据源</param>
        /// <param name="dataPageSize">每页大小</param>
        /// <param name="dataPageFactory">分页处理委托，参数：每页数据源，总页数，页索引</param>
        /// <returns>Task</returns>
        public static async Task DataPageProcessAsync<T>(
            IEnumerable<T> item, int dataPageSize,
            Action<IEnumerable<T>, int, int> dataPageFactory) where T : class
        {
            await Task.Run(() =>
            {
                DotNet3._5.Utilities.Common.EnumerableHelper.DataPageProcess<T>(item, dataPageSize, dataPageFactory);
            });
        }
    }
}