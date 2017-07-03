namespace YanZhiwei.DotNet.Core.WebApi.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        {
            this.dependsOnIdentity = dependsOnIdentity;
            //读取缓存配置总开关
            //this.CacheEnabled = CachedConfigContext.Instance.WebApiOutputCacheConfig.EnableOutputCache;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public bool CacheEnabled
        {
            get
            {
                return CachedConfigContext.Instance.WebApiOutputCacheConfig.EnableOutputCache;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 检查Response字符串是否合法，用于判断Response字符串是否可以缓存
        /// 用于正确的响应才缓存结果
        /// </summary>
        /// <param name="responeString">The respone string.</param>
        /// <returns>否可以缓存</returns>
        public abstract bool CheckedResponseAvailable(string responeString);

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
                try
                {
                    if (actionExecutedContext.Response != null && actionExecutedContext.Response.Content != null)
                    {
                        string _cachekey = CreateCacheKey(actionExecutedContext.ActionContext);
                        AddOrUpdateWebApiOutCache(_cachekey, actionExecutedContext);
                    }
                }
                catch (Exception ex)
                {
                    //LogWriteHelper.Instance.Error("OnActionExecuted", "WebApiOutputCacheAttribute", ex);
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
                try
                {
                    string _cachekey = CreateCacheKey(actionContext);

                    if (!string.IsNullOrEmpty(_cachekey) && apiOutputCache.Contains(_cachekey))
                    {
                        string _cacheApiOutStrinig = (string)apiOutputCache.Get(_cachekey);

                        if (_cacheApiOutStrinig != null)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse();
                            actionContext.Response.Content = new StringContent(_cacheApiOutStrinig);
                            MediaTypeHeaderValue _contenttype = (MediaTypeHeaderValue)apiOutputCache.Get(_cachekey + ":response-ct");

                            if (_contenttype == null)
                                _contenttype = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");

                            actionContext.Response.Content.Headers.ContentType = _contenttype;
                            // LogWriteHelper.Instance.Info(string.Format("Key:{0} 获取缓存成功。", _cachekey));
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //LogWriteHelper.Instance.Error("OnActionExecuting", "WebApiOutputCacheAttribute", ex);
                }
            }
        }

        private void AddOrUpdateWebApiOutCache(string cachekey, HttpActionExecutedContext actionExecutedContext)
        {
            if (!string.IsNullOrEmpty(cachekey))
            {
                string _responebody = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

                if (CheckedResponseAvailable(_responebody))
                {
                    MediaTypeHeaderValue _contentType = actionExecutedContext.Response.Content.Headers.Contains("Content-Type") == true ? actionExecutedContext.Response.Content.Headers.ContentType : MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
                    DateTime _cacheExpire = DateTime.Now.AddSeconds(CacheSeconds);

                    if (apiOutputCache.Contains(cachekey))
                    {
                        apiOutputCache.Set(cachekey, _responebody, _cacheExpire);
                        apiOutputCache.Set(cachekey + ":response-ct", _contentType, _cacheExpire);
                    }
                    else
                    {
                        apiOutputCache.Add(cachekey, _responebody, _cacheExpire);
                        apiOutputCache.Add(cachekey + ":response-ct", _contentType, _cacheExpire);
                    }

                    //LogWriteHelper.Instance.Info(string.Format("Key:{0} 存储缓存成功。", cachekey), "WebApiOutputCacheAttribute");
                }
            }
        }

        /// <summary>
        /// 判断是否可以缓存
        /// </summary>
        private bool CheckedCacheEnable()
        {
            return (CacheEnabled && apiOutputCache != null);
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

        /// <summary>
        /// 创建用于缓存Key
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <returns>Key</returns>
        private string CreateCacheKey(HttpActionContext actionContext)
        {
            try
            {
                string _cachekey = string.Join(":", new string[]
                {
                    actionContext.Request.RequestUri.OriginalString,
                    actionContext.Request.Headers.Contains("User-Agent") == true ? actionContext.Request.Headers.UserAgent.ToString() : string.Empty
                });

                if (dependsOnIdentity)
                    _cachekey = _cachekey.Insert(0, GetIdentityToken(actionContext));

                return _cachekey;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion Methods
    }
}