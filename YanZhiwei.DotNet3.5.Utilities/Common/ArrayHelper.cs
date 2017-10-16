namespace YanZhiwei.DotNet3._5.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Array 辅助类
    /// </summary>
    public static class ArrayHelper
    {
        #region Methods

        /// <summary>
        /// 将array转为具体List对象集合
        /// </summary>
        /// <param name="data">Array</param>
        /// <returns>List对象集合</returns>
        public static List<T> ToList<T>(this Array data)
            where T : class, new()
        {
            return data.Cast<T>().ToList<T>();
        }

        #endregion Methods
    }
}