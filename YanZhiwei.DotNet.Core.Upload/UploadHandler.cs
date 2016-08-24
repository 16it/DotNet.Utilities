using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using YanZhiwei.DotNet.Core.Model;

namespace YanZhiwei.DotNet.Core.Upload
{
    /// <summary>
    /// 处理上传请求
    /// </summary>
    public abstract class UploadHandler : IHttpHandler
    {
        /// <summary>
        /// IsReusable
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
        /// <summary>
        /// 文件名称
        /// </summary>
        public virtual string FileInputName
        {
            get
            {
                return "filedata";
            }
        }
        
        /// <summary>
        /// 上传路径
        /// </summary>
        public string UploadPath
        {
            get
            {
                return UploadConfigContext.UploadPath;
            }
        }
        
        /// <summary>
        /// 最大文件大小，10M
        /// </summary>
        public int MaxFilesize
        {
            //10M
            get
            {
                return 10971520;
            }
        }
        
        /// <summary>
        /// 允许上传文件后缀
        /// <para>txt, rar, zip, jpg, jpeg, gif, png, swf</para>
        /// </summary>
        public virtual string[] AllowExt
        {
            get
            {
                return new string[] { "txt", "rar", "zip", "jpg", "jpeg", "gif", "png", "swf" };
            }
        }
        
        /// <summary>
        /// 运行上传图片后缀
        /// <para>jpg,jpeg,gif,png</para>
        /// </summary>
        public virtual string[] ImageExt
        {
            get
            {
                return new string[] { "jpg", "jpeg", "gif", "png" };
            }
        }
        
        /// <summary>
        /// 处理上传结果抽象方法
        /// </summary>
        /// <param name="localFileName">本地文件名称</param>
        /// <param name="uploadFilePath">上传文件路径</param>
        /// <param name="err">错误信息</param>
        /// <returns>响应字符串</returns>
        public abstract string GetResult(string localFileName, string uploadFilePath, string err);
        
        /// <summary>
        ///上传文件完成抽象方法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="filePath">文件路径</param>
        public abstract void OnUploaded(HttpContext context, string filePath);
        
        private static string CombinePaths(params string[] paths)
        {
            return paths.Aggregate(Path.Combine);
        }
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Charset = "UTF-8";
            byte[] _fileBuffer;
            string _localFileName = string.Empty;
            string _errMessage = string.Empty;
            string _subFolder = string.Empty;
            string _fileFolder = string.Empty;
            string _filePath = string.Empty; ;
            var disposition = context.Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
            
            if(disposition != null)
            {
                // HTML5上传
                _fileBuffer = context.Request.BinaryRead(context.Request.TotalBytes);
                _localFileName = Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value;// 读取原始文件名
            }
            else
            {
                HttpFileCollection _filecollection = context.Request.Files;
                HttpPostedFile _postedfile = _filecollection.Get(this.FileInputName);
                // 读取原始文件名
                _localFileName = Path.GetFileName(_postedfile.FileName);
                // 初始化byte长度.
                _fileBuffer = new byte[_postedfile.ContentLength];
                // 转换为byte类型
                using(Stream stream = _postedfile.InputStream)
                {
                    stream.Read(_fileBuffer, 0, _postedfile.ContentLength);
                    _filecollection = null;
                }
            }
            
            string _fileExt = _localFileName.Substring(_localFileName.LastIndexOf('.') + 1).ToLower();
            
            if(_fileBuffer.Length == 0)
            {
                _errMessage = "无数据提交";
            }
            else if(_fileBuffer.Length > this.MaxFilesize)
            {
                _errMessage = "文件大小超过" + this.MaxFilesize + "字节";
            }
            else if(!AllowExt.Contains(_fileExt))
            {
                _errMessage = "上传文件扩展名必需为：" + string.Join(",", AllowExt);
            }
            else
            {
                string _folder = context.Request["subfolder"] ?? "default";
                UploadFolder _uploadFolderConfig = UploadConfigContext.UploadConfig.UploadFolders.FirstOrDefault(u => string.Equals(_folder, u.Path, StringComparison.OrdinalIgnoreCase));
                DirType _dirType = _uploadFolderConfig == null ? DirType.Day : _uploadFolderConfig.DirType;
                
                //根据配置里的DirType决定子文件夹的层次（月，天，扩展名）
                switch(_dirType)
                {
                    case DirType.Month:
                        _subFolder = "month_" + DateTime.Now.ToString("yyMM");
                        break;
                        
                    case DirType.Ext:
                        _subFolder = "ext_" + _fileExt;
                        break;
                        
                    case DirType.Day:
                        _subFolder = "day_" + DateTime.Now.ToString("yyMMdd");
                        break;
                }
                
                //fileFolder = Path.Combine(UploadConfigContext.UploadPath,
                //                          folder,
                //                          subFolder
                //                         );
                _fileFolder = CombinePaths(UploadConfigContext.UploadPath,
                                           _folder,
                                           _subFolder);
                _filePath = Path.Combine(_fileFolder,
                                         string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random(DateTime.Now.Millisecond).Next(10000), _fileExt)
                                        );
                                        
                if(!Directory.Exists(_fileFolder))
                    Directory.CreateDirectory(_fileFolder);
                    
                using(FileStream fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(_fileBuffer, 0, _fileBuffer.Length);
                    fs.Flush();
                }
                
                //是图片，即使生成对应尺寸
                if(ImageExt.Contains(_fileExt))
                    ThumbnailService.HandleImmediateThumbnail(_filePath);
                    
                this.OnUploaded(context, _filePath);
            }
            
            _fileBuffer = null;
            context.Response.Write(this.GetResult(_localFileName, _filePath, _errMessage));
            context.Response.End();
        }
    }
}