namespace YanZhiwei.DotNet.Core.Web
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using YanZhiwei.DotNet4.Utilities.WebForm;

    /// <summary>
    /// 网页请求辅助类
    /// </summary>
    public class WebHelper : IWebHelper
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly string[] _staticFileExtensions;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContext">HttpContextBase</param>
        public WebHelper(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
            this._staticFileExtensions = new[] { ".axd", ".ashx", ".bmp", ".css", ".gif", ".htm", ".html", ".ico", ".jpeg", ".jpg", ".js", ".png", ".rar", ".zip" };
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 客户端是否被重定向到新位置
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = _httpContext.Response;
                return response.IsRequestBeingRedirected;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 获取当前请求IP地址
        /// </summary>
        /// <returns>
        /// 当前请求IP地址
        /// </returns>
        public virtual string GetCurrentIpAddress()
        {
            if (!_httpContext.IsRequestAvailable())
                return string.Empty;

            var result = "";
            try
            {
                if (_httpContext.Request.Headers != null)
                {
                    //X-Forwarded-For(XFF)是用来识别通过HTTP代理或负载均衡方式连接到Web服务器的客户端最原始的IP地址的HTTP请求头字段。
                    //这一HTTP头一般格式如下:
                    // X-Forwarded-For:client1,proxy1,proxy2
                    // 其中的值通过一个 逗号 + 空格 把多个IP地址区分开, 最左边(client1)是最原始客户端的IP地址,
                    //代理服务器每成功收到一个请求，就把请求来源IP地址添加到右边。
                    //在上面这个例子中，这个请求成功通过了三台代理服务器：proxy1, proxy2 及 proxy3。
                    var forwardedHttpHeader = "X-FORWARDED-FOR";
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ForwardedHTTPheader"]))
                    {
                        forwardedHttpHeader = ConfigurationManager.AppSettings["ForwardedHTTPheader"];
                    }

                    //它用于通过HTTP代理或负载平衡器来识别连接到Web服务器的客户端的始发IP地址
                    string _xff = _httpContext.Request.Headers.AllKeys
                        .Where(x => forwardedHttpHeader.Equals(x, StringComparison.InvariantCultureIgnoreCase))
                        .Select(k => _httpContext.Request.Headers[k])
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(_xff))
                    {
                        result = _xff.Split(new[] { ',' }).FirstOrDefault();
                    }
                }

                if (string.IsNullOrEmpty(result) && _httpContext.Request.UserHostAddress != null)
                {
                    result = _httpContext.Request.UserHostAddress;
                }
            }
            catch
            {
                return result;
            }

            if (result == "::1")
                result = "127.0.0.1";

            if (!string.IsNullOrEmpty(result))
            {
                int _index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (_index > 0)
                    result = result.Substring(0, _index);
            }
            return result;
        }

        /// <summary>
        /// 获取当前请求的URL
        /// </summary>
        /// <returns>当前请求的URL</returns>
        public virtual string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;
            if (_httpContext.IsRequestAvailable() && _httpContext.Request.UrlReferrer != null)
                referrerUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        /// <summary>
        /// 当前连接是否安全
        /// </summary>
        /// <returns>连接是否安全</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            bool useSsl = false;
            if (_httpContext.IsRequestAvailable())
            {
                //1. 是否使用 HTTP_CLUSTER_HTTPS？
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Use_HTTP_CLUSTER_HTTPS"]) &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["Use_HTTP_CLUSTER_HTTPS"]))
                {
                    //配置节点：Use_HTTP_CLUSTER_HTTPS： 它将用于确定当前请求是否为HTTPS。
                    useSsl = ServerVariables("HTTP_CLUSTER_HTTPS") == "on";
                }
                //2. 是否使用 HTTP_X_FORWARDED_PROTO
                else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Use_HTTP_X_FORWARDED_PROTO"]) &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["Use_HTTP_X_FORWARDED_PROTO"]))
                {
                    //配置节点：Use_HTTP_X_FORWARDED_PROTO
                    //X-Forwarded-Proto：记录一个请求一个请求最初从浏览器发出时候，是使用什么协议。
                    //因为有可能当一个请求最初和反向代理通信时，是使用https，但反向代理和服务器通信时改变成http协议，
                    //这个时候，X-Forwarded-Proto的值应该是https
                    useSsl = string.Equals(ServerVariables("HTTP_X_FORWARDED_PROTO"), "https", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    useSsl = _httpContext.Request.IsSecureConnection;
                }
            }

            return useSsl;
        }

        /// <summary>
        /// 如果请求的资源是引擎不需要处理的典型资源之一，则返回true.
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>如果请求指向静态资源文件，则为true。</returns>
        /// <remarks>
        /// 这些是被认为是静态资源的文件扩展名:
        /// .css
        ///	.gif
        /// .png
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        public virtual bool IsStaticResource(HttpRequest request)
        {
            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            return _staticFileExtensions.Contains(extension);
        }

        /// <summary>
        /// 通过key获取Get请求参数
        /// </summary>
        /// <param name="name">Get请求参数Key</param>
        /// <returns>Get请求参数数值</returns>
        public virtual string QueryString(string name)
        {
            string queryParam = null;
            if (_httpContext.IsRequestAvailable() && _httpContext.Request.QueryString[name] != null)
                queryParam = _httpContext.Request.QueryString[name];

            return queryParam;
        }

        /// <summary>
        /// 根据名字获取Web服务器变量的集合
        /// </summary>
        /// <param name="name">Web服务器变量名称</param>
        /// <returns>数值</returns>
        public virtual string ServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                if (!_httpContext.IsRequestAvailable())
                    return result;
                if (_httpContext.Request.ServerVariables[name] != null)
                {
                    result = _httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        
        #endregion Methods
    }
}