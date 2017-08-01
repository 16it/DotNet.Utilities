using System.Web;

namespace YanZhiwei.DotNet.Core.Web
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public interface IWebHelper
    {
        /// <summary>
        /// 获取URL地址
        /// </summary>
        /// <returns>URL地址</returns>
        string GetUrlReferrer();

        /// <summary>
        /// 获取当前请求IP地址
        /// </summary>
        /// <returns>当前请求IP地址</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// 获取当前页面的URL
        /// </summary>
        /// <param name="includeQueryString">是否包含请求参数</param>
        /// <returns>URL</returns>
        string GetThisPageUrl(bool includeQueryString);

        /// <summary>
        /// 获取当前页面的URL
        /// </summary>
        /// <param name="includeQueryString">是否包含请求参数</param>
        /// <param name="useSsl">是否使用ssl</param>
        /// <returns>URL</returns>
        string GetThisPageUrl(bool includeQueryString, bool useSsl);

        /// <summary>
        /// 当前请求连接是否加密
        /// </summary>
        /// <returns>当前请求连接是否加密</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// 按名称获取服务器变量
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>服务器变量</returns>
        string ServerVariables(string name);

        /// <summary>
        /// 获取存储主机位置
        /// </summary>
        /// <param name="useSsl">是否使用ssl</param>
        /// <returns>存储主机位置</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// 获取存储主机位置
        /// </summary>
        /// <returns>存储主机位置</returns>
        string GetStoreLocation();

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store location</returns>
        string GetStoreLocation(bool useSsl);

        /// <summary>
        /// 是否是静态资源
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>是否是静态资源</returns>
        /// <remarks>
        /// 静态资源后缀:
        /// .css
        ///	.gif
        /// .png
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        bool IsStaticResource(HttpRequest request);

        /// <summary>
        /// 修改请求参数
        /// </summary>
        string ModifyQueryString(string url, string queryStringModification, string anchor);


        string RemoveQueryString(string url, string queryString);


        T QueryString<T>(string name);


        void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "");

  
        bool IsRequestBeingRedirected { get; }


        bool IsPostBeingDone { get; set; }
    }
}