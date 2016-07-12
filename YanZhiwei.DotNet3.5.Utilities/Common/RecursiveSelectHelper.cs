namespace YanZhiwei.DotNet3._5.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Linq 递归查找
    /// </summary>
    /// 时间：2016/7/12 15:27
    /// 备注：
    public static class RecursiveSelectHelper
    {
        #region Methods

        /*
         1.源码：http://www.superstarcoders.com/blogs/posts/recursive-select-in-c-sharp-and-linq.aspx
         */
        /// <summary>
        /// Linq 递归查找
        /// </summary>
        /// <typeparam name="TSource">泛型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="childSelector">选择器</param>
        /// <returns>集合</returns>
        /// 时间：2016/7/12 15:27
        /// 备注：
        public static IEnumerable<TSource> RecursiveSelect<TSource>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> childSelector)
        {
            return RecursiveSelect(source, childSelector, element => element);
        }

        /// <summary>
        /// Linq 递归查找
        /// </summary>
        /// <typeparam name="TSource">泛型</typeparam>
        /// <typeparam name="TResult">泛型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="childSelector">子集合选择器</param>
        /// <param name="selector">选择器</param>
        /// <returns>集合</returns>
        /// 时间：2016/7/12 15:27
        /// 备注：
        public static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, TResult> selector)
        {
            return RecursiveSelect(source, childSelector, (element, index, depth) => selector(element));
        }

        /// <summary>
        /// Linq 递归查找
        /// </summary>
        /// <typeparam name="TSource">泛型</typeparam>
        /// <typeparam name="TResult">泛型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="childSelector">子集合选择器</param>
        /// <param name="selector">选择器</param>
        /// <returns>集合</returns>
        /// 时间：2016/7/12 15:27
        /// 备注：
        public static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, int, TResult> selector)
        {
            return RecursiveSelect(source, childSelector, (element, index, depth) => selector(element, index));
        }

        /// <summary>
        /// Recursives the select.
        /// </summary>
        /// <typeparam name="TSource">泛型</typeparam>
        /// <typeparam name="TResult">泛型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="childSelector">子集合选择器</param>
        /// <param name="selector">选择器</param>
        /// <returns>集合</returns>
        /// 时间：2016/7/12 15:27
        /// 备注：
        public static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, int, int, TResult> selector)
        {
            return RecursiveSelect(source, childSelector, selector, 0);
        }

        private static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, int, int, TResult> selector, int depth)
        {
            return source.SelectMany((element, index) => Enumerable.Repeat(selector(element, index, depth), 1)
               .Concat(RecursiveSelect(childSelector(element) ?? Enumerable.Empty<TSource>(),
                  childSelector, selector, depth + 1)));
        }

        #endregion Methods
    }
}