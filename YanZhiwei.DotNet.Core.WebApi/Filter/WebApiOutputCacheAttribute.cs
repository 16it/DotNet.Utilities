namespace YanZhiwei.DotNet.Core.WebApi.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Caching;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using YanZhiwei.DotNet.Core.Config;

    /// <summary>
    /// 处理Webapi缓存
    /// </summary>
    public abstract class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        #region Fields

        /// <summary>
        /// 缓存时间【秒】
        /// </summary>
        public int CacheSeconds;

        /// <summary>
        /// Web Api缓存对象
        /// </summary>
        private static readonly ObjectCache apiOutputCache = MemoryCache.Default;

        /// <summary>
        /// 缓存取决于访问令牌
        /// </summary>
        private readonly bool dependsOnIdentity;

        /// <summary>
        /// 无效缓存，用于重置缓存
        /// </summary>
        private readonly bool invalidateCache;

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        private bool cacheEnabled = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public WebApiOutputCacheAttribute()
            : this(false)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dependsOnIdentity">缓存取决于访问令牌</param>
        public WebApiOutputCacheAttribute(bool dependsOnIdentity)
            : this(dependsOnIdentity, false)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dependsOnIdentity">缓存取决于访问令牌</param>
        /// <param name="invalidateCache">无效缓存，用于重置缓存</param>
        public WebApiOutputCacheAttribute(bool dependsOnIdentity, bool invalidateCache)
        {
            this.dependsOnIdentity = dependsOnIdentity;
            this.invalidateCache = invalidateCache;
            //读取缓存配置总开关
            this.cacheEnabled = CachedConfigContext.Instance.WebApiOutputCacheConfig.EnableOutputCache;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取身份访问令牌
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <returns>身份访问令牌</returns>
        public abstract string GetIdentityToken(HttpActionContext actionContext);

        /// <summary>
        /// Called when [action executed].
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (CheckedCacheEnable())
            {
                if (actionExecutedContext.Response != null && actionExecutedContext.Response.Content != null)
                {
                    string _cachekey = CreateCacheKey(actionExecutedContext.ActionContext);
                    string _responebody = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

                    if (apiOutputCache.Contains(_cachekey))
                    {
                        apiOutputCache.Set(_cachekey, _responebody, DateTime.Now.AddSeconds(CacheSeconds));
                        apiOutputCache.Set(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(CacheSeconds));
                    }
                    else
                    {
                        apiOutputCache.Add(_cachekey, _responebody, DateTime.Now.AddSeconds(CacheSeconds));
                        apiOutputCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(CacheSeconds));
                    }

                    CleanCache(invalidateCache);
                }
            }
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (CheckedCacheEnable() && CheckedCurRequestCacheEnable(actionContext))
            {
                string _cachekey = CreateCacheKey(actionContext);

                if (apiOutputCache.Contains(_cachekey))
                {
                    string _cacheApiOutStrinig = (string)apiOutputCache.Get(_cachekey);

                    if (_cacheApiOutStrinig != null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse();
                        actionContext.Response.Content = new StringContent(_cacheApiOutStrinig);
                        MediaTypeHeaderValue _contenttype = (MediaTypeHeaderValue)apiOutputCache.Get(_cachekey + ":response-ct");

                        if (_contenttype == null)
                            _contenttype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);

                        actionContext.Response.Content.Headers.ContentType = _contenttype;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 移除所有Web Api缓存
        /// </summary>
        private static void CleanCache(bool invalidateCache)
        {
            if (invalidateCache && apiOutputCache != null)
            {
                List<string> _keyList = apiOutputCache.Select(w => w.Key).ToList();

                foreach (string key in _keyList)
                {
                    apiOutputCache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 创建用于缓存Key
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <returns>Key</returns>
        private string CreateCacheKey(HttpActionContext actionContext)
        {
            string _cachekey = string.Join(":", new string[]
            {
                actionContext.Request.RequestUri.OriginalString,
                actionContext.Request.Headers.Accept.FirstOrDefault().ToString(),
            });

            if (dependsOnIdentity)
                _cachekey = _cachekey.Insert(0, GetIdentityToken(actionContext));

            return _cachekey;
        }

        /// <summary>
        /// 判断是否可以缓存
        /// </summary>
        private bool CheckedCacheEnable()
        {
            return (cacheEnabled && apiOutputCache != null);
        }

        /// <summary>
        /// 当前请求是否可缓存
        /// </summary>
        private bool CheckedCurRequestCacheEnable(HttpActionContext context)
        {
            if (CacheSeconds > 0)
            {
                if (context.Request.Method == HttpMethod.Get || context.Request.Method == HttpMethod.Post)
                    return true;
            }

            return false;
        }

        #endregion Methods
    }
}