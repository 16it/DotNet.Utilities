using System;
using System.Collections.Generic;
using System.Linq;

namespace YanZhiwei.DotNet3._5.Utilities.Lambda
{
    /// <summary>
    /// 递归连接
    /// </summary>
    public static class RecursiveJoinLambda
    {
        /// <summary>
        /// 递归连接
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="parentKeySelector">委托</param>
        /// <param name="childKeySelector">委托</param>
        /// <param name="resultSelector">委托</param>
        /// <returns>集合</returns>
        public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TKey> parentKeySelector,
            Func<TSource, TKey> childKeySelector,
            Func<TSource, IEnumerable<TResult>, TResult> resultSelector)
        {
            return RecursiveJoin(source, parentKeySelector, childKeySelector, resultSelector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// 递归连接
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="parentKeySelector">委托</param>
        /// <param name="childKeySelector">委托</param>
        /// <param name="resultSelector">委托</param>
        /// <returns>集合</returns>
        public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TKey> parentKeySelector,
            Func<TSource, TKey> childKeySelector,
            Func<TSource, int, IEnumerable<TResult>, TResult> resultSelector)
        {
            return RecursiveJoin(source, parentKeySelector, childKeySelector,
                (TSource element, int depth, int index, IEnumerable<TResult> children) => resultSelector(element, index, children));
        }

        /// <summary>
        /// 递归连接
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="parentKeySelector">委托</param>
        /// <param name="childKeySelector">委托</param>
        /// <param name="resultSelector">委托</param>
        /// <param name="comparer">IComparer</param>
        /// <returns>集合</returns>
        public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TKey> parentKeySelector,
            Func<TSource, TKey> childKeySelector,
            Func<TSource, IEnumerable<TResult>, TResult> resultSelector,
            IComparer<TKey> comparer)
        {
            return RecursiveJoin(source, parentKeySelector, childKeySelector,
                (TSource element, int depth, int index, IEnumerable<TResult> children) => resultSelector(element, children), comparer);
        }

        /// <summary>
        /// 递归连接
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="parentKeySelector">委托</param>
        /// <param name="childKeySelector">委托</param>
        /// <param name="resultSelector">委托</param>
        /// <param name="comparer">IComparer</param>
        /// <returns>集合</returns>
        public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TKey> parentKeySelector,
            Func<TSource, TKey> childKeySelector,
            Func<TSource, int, IEnumerable<TResult>, TResult> resultSelector,
            IComparer<TKey> comparer)
        {
            return RecursiveJoin(source, parentKeySelector, childKeySelector,
                (TSource element, int depth, int index, IEnumerable<TResult> children) => resultSelector(element, index, children), comparer);
        }

        /// <summary>
        /// 递归连接
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="parentKeySelector">委托</param>
        /// <param name="childKeySelector">委托</param>
        /// <param name="resultSelector">委托</param>
        /// <returns>集合</returns>
        public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TKey> parentKeySelector,
            Func<TSource, TKey> childKeySelector,
            Func<TSource, int, int, IEnumerable<TResult>, TResult> resultSelector)
        {
            return RecursiveJoin(source, parentKeySelector, childKeySelector, resultSelector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// 递归连接
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="parentKeySelector">委托</param>
        /// <param name="childKeySelector">委托</param>
        /// <param name="resultSelector">委托</param>
        /// <param name="comparer">IComparer</param>
        /// <returns>集合</returns>
        public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TKey> parentKeySelector,
            Func<TSource, TKey> childKeySelector,
            Func<TSource, int, int, IEnumerable<TResult>, TResult> resultSelector,
            IComparer<TKey> comparer)
        {
            source = new LinkedList<TSource>(source);
            SortedDictionary<TKey, TSource> _parents = new SortedDictionary<TKey, TSource>(comparer);
            SortedDictionary<TKey, LinkedList<TSource>> _children = new SortedDictionary<TKey, LinkedList<TSource>>(comparer);

            foreach (TSource element in source)
            {
                _parents[parentKeySelector(element)] = element;

                LinkedList<TSource> _list;

                TKey _childKey = childKeySelector(element);

                if (!_children.TryGetValue(_childKey, out _list))
                {
                    _children[_childKey] = _list = new LinkedList<TSource>();
                }

                _list.AddLast(element);
            }

            Func<TSource, int, IEnumerable<TResult>> _childSelector = null;

            _childSelector = (TSource parent, int depth) =>
            {
                LinkedList<TSource> _innerChildren = null;

                if (_children.TryGetValue(parentKeySelector(parent), out _innerChildren))
                {
                    return _innerChildren.Select((child, index) => resultSelector(child, index, depth, _childSelector(child, depth + 1)));
                }

                return Enumerable.Empty<TResult>();
            };

            return source.Where(element => !_parents.ContainsKey(childKeySelector(element)))
                .Select((element, index) => resultSelector(element, index, 0, _childSelector(element, 1)));
        }
    }
}