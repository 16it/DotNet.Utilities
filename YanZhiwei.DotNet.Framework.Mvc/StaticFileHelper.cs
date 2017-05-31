using System;
using System.Web;
using System.Web.Mvc;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Enum;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet.Framework.Mvc
{
    /// <summary>
    ///  文件服务器分离，需要得到文件服务器上文件的地址
    /// </summary>
    public static class StaticFileHelper
    {
        #region Fields
        
        /// <summary>
        /// 取得静态服务器的网址
        /// 如果是https网站，跨域调用静态资源需要欺骗浏览器如：http://content..../.png 改成 //content..../.png
        /// </summary>
        /// <returns></returns>
        private static string staticServiceUri = null;
        
        #endregion Fields
        
        #region Methods
        
        /// <summary>
        /// 取得服务器静态网址
        /// </summary>
        /// <returns>服务器静态网</returns>
        public static string GetStaticServiceUri()
        {
            //使用本地图片，而不做资源分离，暂时取本地地址：
            if(staticServiceUri == null)
                staticServiceUri = "http://" + HttpContext.Current.Request.Url.Authority;
                
            return staticServiceUri;
        }
        
        /// <summary>
        /// 得到图片文件，以及缩略图
        /// </summary>
        /// <param name="helper">UrlHelper</param>
        /// <param name="path">图片路径</param>
        /// <param name="size">缩略图尺寸</param>
        /// <returns>路径</returns>
        public static string ImageFile(this UrlHelper helper, string path, string size = null)
        {
            if(string.IsNullOrEmpty(path))
                return helper.StaticFile(@"/content/images/no_picture.jpg");
                
            if(size == null)
                return helper.StaticFile(path);
                
            var _ext = path.Substring(path.LastIndexOf('.'));
            var _head = path.Substring(0, path.LastIndexOf('.'));
            var _url = string.Format("{0}{1}_{2}{3}", GetStaticServiceUri(), _head, size, _ext);
            return _url;
        }
        
        /// <summary>
        /// 得到静态文件+版本号
        /// <para>配置文件需设置：JsAndCssFileEdition</para>
        /// </summary>
        /// <param name="helper">UrlHelper</param>
        /// <param name="path">Js 或者CSS路径</param>
        /// <returns>路径</returns>
        public static string JsCssFile(this UrlHelper helper, string path)
        {
            ConfigFileOperator _appConfig = new ConfigFileOperator(ProgramMode.WebForm);
            var _jsAndCssFileEdition = _appConfig.GetSetting("JsAndCssFileEdition");
            
            if(string.IsNullOrEmpty(_jsAndCssFileEdition))
                _jsAndCssFileEdition = StringHelper.Unique();
                
            path += string.Format("?v={0}", _jsAndCssFileEdition);
            return helper.StaticFile(path);
        }
        
        /// <summary>
        /// 得到静态文件
        /// </summary>
        /// <param name="helper">UrlHelper</param>
        /// <param name="path">路径</param>
        /// <returns>静态文件</returns>
        public static string StaticFile(this UrlHelper helper, string path)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            
            if(path.StartsWith("~", StringComparison.OrdinalIgnoreCase))
                return helper.Content(path);
                
            else
                return GetStaticServiceUri() + path;
        }
        
        /// <summary>
        /// 得到文件服务器根网址
        /// </summary>
        /// <param name="helper">UrlHelper</param>
        /// <returns>服务器根网址</returns>
        public static string StaticFile(this UrlHelper helper)
        {
            return GetStaticServiceUri();
        }
        
        #endregion Methods
    }
}