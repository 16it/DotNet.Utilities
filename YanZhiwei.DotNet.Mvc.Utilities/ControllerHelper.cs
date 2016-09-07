namespace YanZhiwei.DotNet.Mvc.Utilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Controller 辅助类
    /// </summary>
    /// 时间：2016/9/7 17:25
    /// 备注：
    public static class ControllerHelper
    {
        #region Methods

        /// <summary>
        /// 获取Controller上定义的特性集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterContext">ActionExecutingContext</param>
        /// <returns>Controller上定义的特性集合</returns>
        /// 时间：2016/9/7 17:32
        /// 备注：
        public static IEnumerable<T> GetCustomAttributes<T>(this ActionExecutingContext filterContext)
        where T : FilterAttribute, IActionFilter
        {
            return filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(T), false).Cast<T>();
        }

        #endregion Methods
    }
}