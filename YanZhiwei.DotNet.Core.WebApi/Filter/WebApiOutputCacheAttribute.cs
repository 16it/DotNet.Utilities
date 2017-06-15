using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace YanZhiwei.DotNet.Core.WebApi.Filter
{
    /// <summary>
    /// Action Filter to cache responses
    /// </summary>
    public abstract class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 缓存时间【秒】
        /// </summary>
        public int CacheSeconds;

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        private bool cacheEnabled = true;

        /// <summary>
        /// 缓存取决于访问令牌
        /// </summary>
        private readonly bool dependsOnIdentity;

        // cache repository
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        /// <summary>
        /// 无效缓存，用于重置缓存
        /// </summary>
        private readonly bool invalidateCache;

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

            //   ReadConfig();
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">The filter context.</param>
        /// <exception cref="System.ArgumentNullException">actionContext</exception>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (cacheEnabled)
            {
                if (actionContext != null)
                {
                    if (IsCacheable(actionContext))
                    {
                        string _cachekey = CreateCacheKey(actionContext);

                        if (WebApiCache.Contains(_cachekey))
                        {
                            string _cacheStrinig = (string)WebApiCache.Get(_cachekey);
                            if (_cacheStrinig != null)
                            {
                                actionContext.Response = actionContext.Request.CreateResponse();
                                actionContext.Response.Content = new StringContent(_cacheStrinig);
                                MediaTypeHeaderValue _contenttype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct");
                                if (_contenttype == null)
                                    _contenttype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                                actionContext.Response.Content.Headers.ContentType = _contenttype;
                                return;
                            }
                        }
                    }
                }
            }
        }

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

        public abstract string GetIdentityToken(HttpActionContext actionContext);

        /// <summary>
        /// Called when [action executed].
        /// </summary>
        /// <param name="actionExecutedContext">The filter context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (cacheEnabled)
                {
                    if (WebApiCache != null)
                    {
                        string _cachekey = CreateCacheKey(actionExecutedContext.ActionContext);
                        if (actionExecutedContext.Response != null && actionExecutedContext.Response.Content != null)
                        {
                            string body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

                            if (WebApiCache.Contains(_cachekey))
                            {
                                WebApiCache.Set(_cachekey, body, DateTime.Now.AddSeconds(CacheSeconds));
                                WebApiCache.Set(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_timespan));
                            }
                            else
                            {
                                WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(CacheSeconds));
                                WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_timespan));
                            }
                        }
                    }
                }

                if (invalidateCache)
                {
                    CleanCache();
                }
            }
            catch (Exception ex)
            {
                //TraceManager.TraceError(ex);
            }
        }

        /// <summary>
        /// Removes all items from the cache
        /// </summary>
        private static void CleanCache()
        {
            if (WebApiCache != null)
            {
                List<string> keyList = WebApiCache.Select(w => w.Key).ToList();
                foreach (string key in keyList)
                {
                    WebApiCache.Remove(key);
                }
            }
        }

        //private void ReadConfig()
        //{
        //    if (!Boolean.TryParse(WebConfigurationManager.AppSettings["CacheEnabled"], out cacheEnabled))
        //        cacheEnabled = false;

        //    if (!Int32.TryParse(WebConfigurationManager.AppSettings["CacheTimespan"], out _timespan))
        //        _timespan = 1800; // seconds
        //}

        private bool IsCacheable(HttpActionContext ac)
        {
            if (CacheSeconds > 0)
            {
                if (ac.Request.Method == HttpMethod.Get || ac.Request.Method == HttpMethod.Post)
                    return true;
            }

            return false;
        }
    }
}