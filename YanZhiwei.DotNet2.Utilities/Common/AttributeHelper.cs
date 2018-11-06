namespace YanZhiwei.DotNet2.Utilities.Common
{
    using System;

    /// <summary>
    /// Attribute 帮助类
    /// </summary>
    /// 时间：2016-01-12 15:17
    /// 备注：
    public static class AttributeHelper
    {
        #region Methods

        /// <summary>
        /// 获取自定义Attribute
        /// </summary>
        /// <typeparam name="TModel">泛型</typeparam>
        /// <typeparam name="TAttribute">泛型</typeparam>
        /// <returns>未获取到则返回NULL</returns>
        /// 时间：2016-01-12 15:22
        /// 备注：
        public static TAttribute GetCustom<TModel, TAttribute>()
        where TModel : class
            where TAttribute : Attribute
        {
            Type type = typeof(TModel);

            object[] cAttribute = type.GetCustomAttributes(typeof(TAttribute), true);

            if (cAttribute != null && cAttribute.Length > 0)
            {
                TAttribute tAttribute = cAttribute[0] as TAttribute;
                return tAttribute;
            }

            return null;
        }

        #endregion Methods
    }
}