using System.Web;

namespace YanZhiwei.DotNet.Core.Mvc
{
    /// <summary>
    /// Web请求通用接口
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
    }
}