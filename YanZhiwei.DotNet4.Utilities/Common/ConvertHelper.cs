﻿namespace YanZhiwei.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    ///  转换帮助类
    /// </summary>
    /// 时间：2016-01-14 12:21
    /// 备注：
    public static class ConvertHelper
    {
        #region Methods

        /// <summary>
        /// 将字符串转换为Guid
        /// </summary>
        /// <param name="data">需要转换的字符串</param>
        /// <param name="defalut">默认数值</param>
        /// <returns>转换返回</returns>
        public static Guid ToGuidOrDefault(this string data, Guid defalut)
        {
            Guid _result = Guid.Empty;
            if (Guid.TryParse(data, out _result))
                return _result;
            else
                return defalut;
        }

        #endregion Methods
    }
}