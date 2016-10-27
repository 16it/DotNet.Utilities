using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using YanZhiwei.DotNet.Core.Model;
using YanZhiwei.DotNet3._5.Utilities.WebForm;

namespace YanZhiwei.DotNet.Core.Upload
{
    /// <summary>
    /// 对按需(OnDemand)生成的图片进行拦截生成缩略图
    /// </summary>
    public class ThumbnailHandler : IHttpHandler
    {
        /// <summary>
        /// Gets a value indicating whether this instance is reusable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is reusable; otherwise, <c>false</c>.
        /// </value>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        public void ProcessRequest(HttpContext context)
        {
            //如果304已缓存了，则返回
            if(!string.IsNullOrEmpty(context.Request.Headers["If-Modified-Since"]))
            {
                context.Response.StatusCode = 304;
                context.Response.StatusDescription = "Not Modified";
                return;
            }
            
            string _path = context.Request.CurrentExecutionFilePath;
            
            if(!_path.EndsWith(".axd") && !_path.StartsWith("/Upload", StringComparison.OrdinalIgnoreCase))
                return;
                
            //正则从Url里匹配出上传的文件夹目录.....
            Match _uploadfolder = Regex.Match(_path, @"upload/(.+)/(day_\d+)/thumb/(\d+)_(\d+)_(\d+)\.([A-Za-z]+)\.axd$", RegexOptions.IgnoreCase);
            
            if(!_uploadfolder.Success)
                return;
                
            string _folder = _uploadfolder.Groups[1].Value,
                   _subFolder = _uploadfolder.Groups[2].Value,
                   _fileName = _uploadfolder.Groups[3].Value,
                   _width = _uploadfolder.Groups[4].Value,
                   _height = _uploadfolder.Groups[5].Value,
                   _fileExt = _uploadfolder.Groups[6].Value;
            //如果在配置找不到需要按需生成的，则返回，这样能防止任何人随便敲个尺寸就生成
            string _key = string.Format("{0}_{1}_{2}", _folder, _width, _height).ToLower();
            bool isOnDemandSize = UploadConfigContext.ThumbnailConfigDic.ContainsKey(_key) && UploadConfigContext.ThumbnailConfigDic[_key].Timming == Timming.OnDemand;
            
            if(!isOnDemandSize)
                return;
                
            string _thumbnailFilePath = string.Format(@"{0}\{1}\Thumb\{2}_{4}_{5}.{3}", _folder, _subFolder, _fileName, _fileExt, _width, _height);
            _thumbnailFilePath = Path.Combine(UploadConfigContext.UploadPath, _thumbnailFilePath);
            string _filePath = string.Format(@"{0}\{1}\{2}.{3}", _folder, _subFolder, _fileName, _fileExt);
            _filePath = Path.Combine(UploadConfigContext.UploadPath, _filePath);
            
            if(!File.Exists(_filePath))
                return;
                
            //如果不存在缩略图，则生成
            if(!File.Exists(_thumbnailFilePath))
            {
                string _thumbnailFileFolder = string.Format(@"{0}\{1}\Thumb", _folder, _subFolder);
                _thumbnailFileFolder = Path.Combine(UploadConfigContext.UploadPath, _thumbnailFileFolder);
                
                if(!Directory.Exists(_thumbnailFileFolder))
                    Directory.CreateDirectory(_thumbnailFileFolder);
                    
                ThumbnailHelper.MakeThumbnail(_filePath, _thumbnailFilePath, UploadConfigContext.ThumbnailConfigDic[_key]);
            }
            
            //缩略图存在了，返回图片字节，并输出304标记
            context.Response.Clear();
            context.Response.ContentType = HttpContextHelper.GetImageContentType(_fileExt);
            byte[] _responseImage = File.ReadAllBytes(_thumbnailFilePath);
            context.Response.BinaryWrite(_responseImage);
            context.Set304Cache();
            context.Response.Flush();
        }
    }
}