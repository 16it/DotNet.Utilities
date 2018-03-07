namespace YanZhiwei.DotNet4.Utilities.Collection
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class LazyConcurrentDictionary<TKey, TValue>
    {
        #region Fields

        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> concurrentDictionary;

        #endregion Fields

        #region Constructors

        public LazyConcurrentDictionary()
        {
            this.concurrentDictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the or add.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns></returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var lazyResult = this.concurrentDictionary.GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication));

            return lazyResult.Value;
        }

        #endregion Methods
    }
}