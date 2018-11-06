namespace YanZhiwei.DotNet2.Utilities.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// 线程安全集合
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class ThreadSafeList<T> : IList<T>
    {
        #region Fields

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly => false;

        /*
         * 参考：
         * 1. http://www.codeproject.com/KB/cs/safe_enumerable.aspx
         */

        /// <summary>
        /// 集合
        /// </summary>
        private readonly List<T> _storeList;

        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object _syncRoot = new object();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// ThreadSafeList构造函数
        /// </summary>
        public ThreadSafeList()
        {
            _storeList = new List<T>();
        }

        /// <summary>
        /// ThreadSafeList构造函数
        /// </summary>
        /// <param name="data">IEnumerable</param>
        public ThreadSafeList(IEnumerable<T> data)
        {
            _storeList = new List<T>(data);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _storeList.Count;
                }
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>T</returns>
        public T this[int index]
        {
            get
            {
                lock (_syncRoot)
                {
                    return _storeList[index];
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _storeList[index] = value;
                }
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="item">数据项</param>
        public void Add(T item)
        {
            lock (_syncRoot)
            {
                _storeList.Add(item);
            }
        }

        /// <summary>
        /// 先判断是否存在集合里面，若存在则移出，然后重新添加
        /// <para>eg:personList.Add(_person, p => p.Age == 19);</para>
        /// </summary>
        /// <param name="t">泛型</param>
        /// <param name="match">委托</param>
        public void Add(T t, Predicate<T> match)
        {
            if (match != null)
            {
                T _finded = Find(match);

                if (_finded != null)
                {
                    Remove(_finded);
                }

                Add(t);
            }
        }

        /// <summary>
        /// 去重复集合添加
        /// </summary>
        /// <param name="items">添加集合</param>
        /// <param name="comparaer">IComparer</param>
        public void AddUniqueTF(IEnumerable<T> items, IComparer<T> comparaer)
        {
            lock (_syncRoot)
            {
                _storeList.Sort(comparaer);

                foreach (T item in items)
                {
                    int _result = _storeList.BinarySearch(item, comparaer);

                    if (_result < 0)
                    {
                        _storeList.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 作为只读
        /// </summary>
        /// <returns>ReadOnlyCollection</returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            lock (_syncRoot)
            {
                return new ReadOnlyCollection<T>(this);
            }
        }

        /// <summary>
        /// 移出所有元素
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _storeList.Clear();
            }
        }

        /// <summary>
        /// 是否包含某项元素
        /// </summary>
        /// <param name="item">数据项</param>
        /// <returns>是否包含</returns>
        public bool Contains(T item)
        {
            lock (_syncRoot)
            {
                return _storeList.Contains(item);
            }
        }

        /// <summary>
        /// 复制到某个类型数组
        /// </summary>
        /// <param name="array">复制到苏族</param>
        /// <param name="arrayIndex">开始位置</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                _storeList.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="match">委托</param>
        /// <returns>是否存在</returns>
        public bool Exists(Predicate<T> match)
        {
            if (match != null)
            {
                lock (_syncRoot)
                {
                    foreach (T item in _storeList)
                    {
                        if (match(item))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="match">委托</param>
        /// <returns>查找到项</returns>
        public T Find(Predicate<T> match)
        {
            if (match != null)
            {
                lock (_syncRoot)
                {
                    return _storeList.Find(match);
                }
            }

            return default(T);
        }

        /// <summary>
        /// 查找群不
        /// </summary>
        /// <param name="match">委托</param>
        /// <returns>查找到的集合</returns>
        public List<T> FindAll(Predicate<T> match)
        {
            if (match != null)
            {
                lock (_syncRoot)
                {
                    return _storeList.FindAll(match);
                }
            }

            return null;
        }

        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action">委托</param>
        public void ForEach(Action<T> action)
        {
            if (action != null)
            {
                lock (_syncRoot)
                {
                    foreach (T item in _storeList)
                    {
                        action(item);
                    }
                }
            }
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>
        /// 可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            lock (_syncRoot)
            {
                return new ThreadSafeEnumerator<T>(_storeList.GetEnumerator(), _syncRoot);
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>
        /// 可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_syncRoot)
            {
                return new ThreadSafeEnumerator<T>(_storeList.GetEnumerator(), _syncRoot);
            }
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>索引位置</returns>
        public int IndexOf(T item)
        {
            lock (_syncRoot)
            {
                return _storeList.IndexOf(item);
            }
        }

        /// <summary>
        /// 插入一项
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="item">插入项</param>
        public void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                _storeList.Insert(index, item);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="item">需要移除项</param>
        /// <returns>是否移除成功</returns>
        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                return _storeList.Remove(item);
            }
        }

        /// <summary>
        /// RemoveAll
        /// </summary>
        /// <param name="match">Predicate委托</param>
        public void RemoveAll(Predicate<T> match)
        {
            if (match != null)
            {
                lock (_syncRoot)
                {
                    _storeList.RemoveAll(match);
                }
            }
        }

        /// <summary>
        /// RemoveAt
        /// </summary>
        /// <param name="index">index</param>
        public void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                _storeList.RemoveAt(index);
            }
        }

        /// <summary>
        /// Trims the excess.
        /// </summary>
        public void TrimExcess()
        {
            lock (_syncRoot)
            {
                _storeList.TrimExcess();
            }
        }

        #endregion Methods
    }
}