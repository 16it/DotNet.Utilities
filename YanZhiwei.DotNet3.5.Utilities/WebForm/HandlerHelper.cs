namespace YanZhiwei.DotNet3._5.Utilities.WebForm
{
    using System;
    using System.Net;
    using System.Web;

    using YanZhiwei.DotNet3._5.Utilities.Common;
    using YanZhiwei.DotNet3._5.Utilities.Model;

    /// <summary>
    /// HttpHandler帮助类
    /// </summary>
    /// 创建时间:2015-06-08 11:46
    /// 备注说明:<c>null</c>
    public static class HandlerHelper
    {
        #region Methods

        /// <summary>
        /// 创建文件全路径
        /// <para>eg: context.CreateFilePath("images/1616/LampGroup/lampGroup.jpg")</para>
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="file">The file.eg:"images/1616/LampGroup/lampGroup.jpg"</param>
        /// <returns>文件路径</returns>
        /// 创建时间:2015-06-09 11:15
        /// 备注说明:<c>null</c>
        public static string CreateFilePath(this HttpContext context, string file)
        {
            string _fullPath = @"http://" + context.Request.Url.Authority
                               + (context.Request.ApplicationPath == "/" ? "/" : context.Request.ApplicationPath + "/")
                               + file;
            return _fullPath;
        }

        /// <summary>
        /// 创建Response响应
        /// <para>eg:context.CreateResponse(_zNodeList, HttpStatusCode.OK);</para>
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="content">响应内容</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="errorCode">T错误代码</param>
        /// 时间：2016-05-04 17:32
        /// 备注：
        public static void CreateResponse(this HttpContext context, object content, HttpStatusCode statusCode, int errorCode)
        {
            JsonResult _jsonResult = new JsonResult();
            _jsonResult.StatusCode = (int)statusCode;
            _jsonResult.Content = content;
            _jsonResult.ErrorCode = errorCode;
            string _jsonString = SerializeHelper.JsonSerialize<JsonResult>(_jsonResult);
            context.Response.Write(_jsonString);
            context.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 创建Response响应
        /// <para>eg:context.CreateResponse(_zNodeList, HttpStatusCode.OK);</para>
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="content">响应内容</param>
        /// <param name="statusCode">状态码</param>
        /// 时间：2016-05-04 17:32
        /// 备注：
        public static void CreateResponse(this HttpContext context, object content, HttpStatusCode statusCode)
        {
            CreateResponse(context, content, statusCode, 0);
        }

        /// <summary>
        /// 自从上次请求后，请求的网页未修改过。 服务器返回此响应时，不会返回网页内容。
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// 时间：2016/8/23 9:05
        /// 备注：
        public static void Set304Cache(this HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetLastModified(DateTime.UtcNow);
            context.Response.AddHeader("If-Modified-Since", DateTime.UtcNow.ToString());
            int _maxDay = 86400 * 14; // 14 Day
            context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(_maxDay));
            context.Response.Cache.SetMaxAge(new TimeSpan(0, 0, _maxDay));
            context.Response.CacheControl = "private";
            context.Response.Cache.SetValidUntilExpires(true);
        }

        /// <summary>
        /// 获取图片类型contentType
        /// </summary>
        /// <param name="ext">文件后缀</param>
        /// <returns>contentType</returns>
        /// 时间：2016/8/23 9:31
        /// 备注：
        public static string GetImageContentType(string ext)
        {
            string _contentType = null;

            switch (ext.ToLower())
            {
                case "gif":
                    _contentType = "image/gif";
                    break;

                case "jpg":
                case "jpe":
                case "jpeg":
                    _contentType = "image/jpeg";
                    break;

                case "bmp":
                    _contentType = "image/bmp";
                    break;

                case "tif":
                case "tiff":
                    _contentType = "image/tiff";
                    break;

                case "eps":
                    _contentType = "application/postscript";
                    break;

                default:
                    break;
            }

            return _contentType;
        }
    }

    #endregion Methods
}
