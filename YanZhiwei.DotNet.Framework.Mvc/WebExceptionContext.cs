using System.Collections.Specialized;
using System.Web.Routing;

namespace YanZhiwei.DotNet.Core.Mvc
{
    /// <summary>
    /// 网页异常上下文
    /// </summary>
    /// 时间：2016/9/6 15:58
    /// 备注：
    public class WebExceptionContext
    {
        /// <summary>
        /// IP
        /// </summary>
        public string IP
        {
            get;
            set;
        }
        
        /// <summary>
        /// 当前链接
        /// </summary>
        public string CurrentUrl
        {
            get;
            set;
        }
        
        /// <summary>
        /// 引用链接
        /// </summary>
        public string RefUrl
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否是ajax请求
        /// </summary>
        public bool IsAjaxRequest
        {
            get;
            set;
        }
        
        /// <summary>
        /// FormData
        /// </summary>
        public NameValueCollection FormData
        {
            get;
            set;
        }
        
        /// <summary>
        /// QueryData
        /// </summary>
        public NameValueCollection QueryData
        {
            get;
            set;
        }
        
        /// <summary>
        /// RouteData
        /// </summary>
        public RouteValueDictionary RouteData
        {
            get;
            set;
        }
    }
}