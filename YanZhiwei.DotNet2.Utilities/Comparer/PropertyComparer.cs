namespace YanZhiwei.DotNet2.Utilities.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// 泛型属性比较
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    internal class PropertyComparer<T> : IComparer<T>
    {
        #region Fields

        /// <summary>
        /// 排序操作方向
        /// </summary>
        private ListSortDirection direction;

        /// <summary>
        /// 属性
        /// </summary>
        private PropertyDescriptor property;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pperty">属性</param>
        /// <param name="pdirection">排序操作方向</param>
        public PropertyComparer(PropertyDescriptor pperty, ListSortDirection pdirection)
        {
            property = pperty;
            direction = pdirection;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 比较两个对象并返回一个值，该值指示一个对象小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="itemA">对象</param>
        /// <param name="itemB">被比较对象</param>
        /// <returns>int</returns>
        public int Compare(T itemA, T itemB)
        {
            object _itemAValue = GetPropertyValue(itemA, property.Name);
            object _itemBValue = GetPropertyValue(itemB, property.Name);
            return direction == ListSortDirection.Ascending ? CompareAscending(_itemAValue, _itemBValue) :
                   CompareDescending(_itemAValue, _itemBValue);
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="itemA">对象</param>
        /// <param name="itemB">被比较对象</param>
        /// <returns>是否相等</returns>
        public bool Equals(T itemA, T itemB)
        {
            return itemA.Equals(itemB);
        }

        /// <summary>
        /// 获取hashCode
        /// </summary>
        /// <param name="item">操作对象</param>
        /// <returns>hashCode</returns>
        public int GetHashCode(T item)
        {
            return item.GetHashCode();
        }

        /// <summary>
        /// 设置排序操作方向
        /// </summary>
        /// <param name="sortDirection">ListSortDirection</param>
        public void SetDirection(ListSortDirection sortDirection)
        {
            direction = sortDirection;
        }

        /// <summary>
        /// 升序比较
        /// </summary>
        /// <param name="itemA">对象</param>
        /// <param name="itemB">被比较对象</param>
        /// <returns>位置</returns>
        private int CompareAscending(object itemA, object itemB)
        {
            int _result;

            if(itemA is IComparable)
            {
                _result = ((IComparable)itemA).CompareTo(itemB);
            }

            else if(itemA.Equals(itemB))
            {
                _result = 0;
            }

            else
            {
                _result = ((IComparable)itemA).CompareTo(itemB);
            }

            return _result;
        }

        /// <summary>
        /// 降序比较
        /// </summary>
        /// <param name="itemA">对象</param>
        /// <param name="itemB">被比较对象</param>
        /// <returns>位置</returns>
        private int CompareDescending(object itemA, object itemB)
        {
            return -CompareAscending(itemA, itemB);
        }

        /// <summary>
        /// 依据属性名称获取对象数值
        /// </summary>
        /// <param name="item">对象</param>
        /// <param name="propertyName">对象属性名称</param>
        /// <returns>属性数值</returns>
        private object GetPropertyValue(T item, string propertyName)
        {
            PropertyInfo _propertyInfo = item.GetType().GetProperty(propertyName);
            return _propertyInfo != null ? _propertyInfo.GetValue(item, null) : null;
        }

        #endregion Methods
    }
}