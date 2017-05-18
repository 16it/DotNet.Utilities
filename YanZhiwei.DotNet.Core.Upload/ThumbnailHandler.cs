using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using YanZhiwei.DotNet.Core.Model;
using YanZhiwei.DotNet2.Utilities.Result;
using YanZhiwei.DotNet3._5.Utilities.WebForm;

namespace YanZhiwei.DotNet.Core.Upload
{
    /// <summary>
    /// 对按需(OnDemand)生成的图片进行拦截生成缩略图
    /// </summary>
    public class ThumbnailHandler : IHttpHandler
    {
        /// <summary>
        /// 指示其他请求是否可以使用 IHttpHandler 实例。
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // 后缀为axd 的文件
        //其实扩展名为ashx与为axd基本上是一样的，都是用于写web handler，可以通过它来调用IHttpHandler类，它免去了普通.aspx页面的控件解析以及页面处理的过程。
        //唯一不同的地方是：axd扩展名的必须要在web.config中的<httpHandlers> 中进行注册，而ashx直接在项目中当成aspx那样添加使用即可。

        //所以在项目的添加文件中，向导只有添加ashx文件的模板，而没有添加axd文件的模板。那微软为什么这么无聊搞两个后缀，全部使用ashx不就行了么？干脆利落。原来，如果你的web handler代码不在Web的项目中的话，那你就不能使用ashx了，因为如果不在web.config中注册的话，系统根本不知道要在那个dll库中才能找到相应的代码。
        //如：
        //<add verb = "*" path="OpenSearch.axd" type="Company.Components.HttpHandler.OpenSearchHandler, （命名空间.类名）Company.Extensions（.dll文件名）" validate="false"/>
        //只有注册了，web才知道OpenSearch.axd原来是在Company.Extensions.dll中，使用Company.Components.HttpHandler.OpenSearchHandler类处理。
        //当然你搞个<add verb="*" path="OpenSearch.ashx" type=.... 那也未免不可，习惯规范而已。
        //在服务器的IIS里有个默认的映射：就是将*.axd映射到aspnet_isapi.dll上。
        //  webconfig里那么写的原理是，首先iis会把.axd的文件handle，然后就交给FreeTextBoxControls.AssemblyResourceHandler, FreeTextBox这个命名的类来处理而不是让aspnet去处理。
        //  但是你的服务器提供商可能为了安全起见，把.axd到aspnet_isapi.dll的映射去掉了，所以你在服务器运行就错误了。你现在唯一能做的就是联系你的服务器提供商，让他们恢复这个映射。
        //  扩展名：   .axd
        //  执行文件：C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll
        //限制为：GET, HEAD, POST, private DEBUG
        //脚本引擎打勾

        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void ProcessRequest(HttpContext context)
        {
            CheckResult _checkedRequestImageCache = CheckedRequestImageCache(context);

            if (_checkedRequestImageCache.State)
                return;

            string _path = context.Request.CurrentExecutionFilePath;

            if (!_path.EndsWith(".axd", StringComparison.OrdinalIgnoreCase) && !_path.StartsWith("/Upload", StringComparison.OrdinalIgnoreCase))
                return;

            //正则从Url里匹配出上传的文件夹目录信息
            Match _uploadfolder = Regex.Match(_path, @"upload/(.+)/(day_\d+)/thumb/(\d+)_(\d+)_(\d+)\.([A-Za-z]+)\.axd$", RegexOptions.IgnoreCase);

            if (!_uploadfolder.Success)
                return;

            string _folder = _uploadfolder.Groups[1].Value,
                   _subFolder = _uploadfolder.Groups[2].Value,
                   _fileName = _uploadfolder.Groups[3].Value,
                   _width = _uploadfolder.Groups[4].Value,
                   _height = _uploadfolder.Groups[5].Value,
                   _fileExt = _uploadfolder.Groups[6].Value;
            string _thumbnailKey = null;//按需生成缩略图的Key

            if (!CheckedOnDemandCreateThumbnail(_folder, _width, _height, out _thumbnailKey))
                return;

            string _filePath = string.Empty;//原始图片路径
            string _thumbnailFilePath = GetOnDemandThumbnailFile(_folder, _subFolder, _fileName, _fileExt, _width, _height, out _filePath);//缩略图存储路径

            if (!File.Exists(_filePath))
                return;

            OnDemandCreateThumbnailImage(_filePath, _thumbnailFilePath, _folder, _subFolder, _thumbnailKey);//按需生成缩略图，若缩略图不存在
            //缩略图存在了，返回图片字节，并输出304标记
            context.Response.Clear();
            context.Response.ContentType = HttpContextHelper.GetImageContentType(_fileExt);
            byte[] _responseImage = File.ReadAllBytes(_thumbnailFilePath);
            context.Response.BinaryWrite(_responseImage);
            context.Set304Cache();
            context.Response.Flush();
        }

        /// <summary>
        /// 按需生成缩略图
        /// </summary>
        private void OnDemandCreateThumbnailImage(string filePath, string thumbnailFilePath, string folder, string subFolder, string thumbnailKey)
        {
            //若缩略图不存在，则生成
            if (!File.Exists(thumbnailFilePath))
            {
                string _thumbnailFileFolder = string.Format(@"{0}\{1}\Thumb", folder, subFolder);
                _thumbnailFileFolder = Path.Combine(UploadConfigContext.UploadPath, _thumbnailFileFolder);

                if (!Directory.Exists(_thumbnailFileFolder))
                    Directory.CreateDirectory(_thumbnailFileFolder);

                ThumbnailHelper.BuilderThumbnail(filePath, thumbnailFilePath, UploadConfigContext.ThumbnailConfigDic[thumbnailKey]);
            }
        }

        /// <summary>
        /// 获取新的缩略图存储路径以及原始图片的路径
        /// </summary>
        private string GetOnDemandThumbnailFile(string folder, string subFolder, string fileName, string fileExt, string width, string height, out string filePath)
        {
            filePath = string.Empty;
            string _thumbnailFilePath = string.Format(@"{0}\{1}\Thumb\{2}_{4}_{5}.{3}", folder, subFolder, fileName, fileExt, width, height);
            _thumbnailFilePath = Path.Combine(UploadConfigContext.UploadPath, _thumbnailFilePath);
            filePath = string.Format(@"{0}\{1}\{2}.{3}", folder, subFolder, fileName, fileExt);
            filePath = Path.Combine(UploadConfigContext.UploadPath, filePath);
            return _thumbnailFilePath;
        }

        /// <summary>
        /// 检查配置，当前尺寸是否按需请求生成缩略图
        /// </summary>
        private bool CheckedOnDemandCreateThumbnail(string folder, string width, string height, out string key)
        {
            key = string.Format("{0}_{1}_{2}", folder, width, height).ToLower();
            return UploadConfigContext.ThumbnailConfigDic.ContainsKey(key) && UploadConfigContext.ThumbnailConfigDic[key].Timming == ThumbnailTimming.OnDemand;
        }

        /// <summary>
        /// 判断请求图片的缓存是否存在
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>是否存在</returns>
        private CheckResult CheckedRequestImageCache(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.Headers["If-Modified-Since"]))
            {
                context.Response.StatusCode = 304;
                context.Response.StatusDescription = "Not Modified";
                return CheckResult.Success();
            }

            return CheckResult.Fail(null);
        }
    }
}