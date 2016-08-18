namespace YanZhiwei.DotNet.Core.Cache
{
    using System;

    /// <summary>
    /// 缓存提供者接口
    /// </summary>
    public interface ICacheProvider
    {
        #region Methods

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="keyRegex">正则表达式</param>
        void Clear(string keyRegex);

        /// <summary>
        /// 以键取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        object Get(string key);

        /// <summary>
        /// 从缓存中获取强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>获取的强类型数据</returns>
        T Get<T>(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="minutes">分钟</param>
        /// <param name="isAbsoluteExpiration">是否绝对时间</param>
        /// <param name="onRemoveFacotry">委托</param>
        void Set(string key, object value, int minutes, bool isAbsoluteExpiration, Action<string, object, string> onRemoveFacotry);

        #endregion Methods
    }
}