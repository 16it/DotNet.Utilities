using System.Collections;
using System.Linq;
using System.Web;
using YanZhiwei.DotNet.Core.Cache;

namespace YanZhiwei.DotNet.Core.PerRequestCache
{
    /// <summary>
    ///一个HttpRequest中的各个单元需要处理相同或类似的数据。
    ///如果数据的生存期只是一个请求，就可以考虑使用HttpContext. Items作为短期的高速缓存。
    /// </summary>
    public class PerRequestProvider : ICacheProvider
    {
        /// <summary>
        /// 以键取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>
        /// 值
        /// </returns>
        public virtual object Get(string key)
        {
            var items = GetItems();
            if (items == null)
                return null;

            return items[key];
        }

        /// <summary>
        /// 从缓存中获取强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>
        /// 获取的强类型数据
        /// </returns>
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        /// 该key是否设置过缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;

            return (items[key] != null);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="minutes">分钟</param>
        /// <param name="isAbsoluteExpiration">是否绝对时间</param>
        public virtual void Set(string key, object value, int minutes, bool isAbsoluteExpiration)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (value != null)
            {
                if (items.Contains(key))
                    items[key] = value;
                else
                    items.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary GetItems()
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.Items;

            return null;
        }
    }
}