namespace YanZhiwei.DotNet.Core.Upload
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    using YanZhiwei.DotNet.Core.Model;
    using YanZhiwei.DotNet2.Utilities.Result;

    /// <summary>
    /// 处理上传请求
    /// </summary>
    public abstract class UploadHandler : IHttpHandler
    {
        #region Properties

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
        /// 允许上传图片后缀
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
        /// 上传路径
        /// </summary>
        public string UploadPath
        {
            get
            {
                return UploadConfigContext.UploadPath;
            }
        }

        #endregion Properties

        #region Methods

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

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void ProcessRequest(HttpContext context)
        {
            //Content - disposition 是 MIME 协议的扩展，MIME 协议指示 MIME 用户代理如何显示附加的文件。当 Internet Explorer 接收到头时，它会激活文件下载对话框，它的文件名框自动填充了头中指定的文件名。（请注意，这是设计导致的；无法使用此功能将文档保存到用户的计算机上，而不向用户询问保存位置。）
            //Content - Disposition就是当用户想把请求所得的内容存为一个文件的时候提供一个默认的文件名。具体的定义如下
            //content - disposition = “Content - Disposition” “:”
            //disposition - type * ( “;” disposition - parm )
            //disposition - type = “attachment” | disp - extension - token
            //disposition - parm = filename - parm | disp - extension - parm
            //filename - parm = “filename” “=” quoted - string
            //disp - extension - token = token
            //disp - extension - parm = token “=” (token | quoted - string)
            //那么由上可知具体的例子：Content - Disposition: attachment; filename =“filename.xls”
            //当然filename参数可以包含路径信息，但User - Agnet会忽略掉这些信息，只会把路径信息的最后一部分做为文件名。当你在响应类型为application / octet - stream情况下使用了这个头信息的话，那就意味着你不想直接显示内容，而是弹出一个”文件下载”的对话框，接下来就是由你来决定“打开”还是“保存”了。
            //如: Response.AppendHeader("Content-Disposition", "attachment;filename=MyExcel.xls");
            context.Response.Charset = "UTF-8";
            byte[] _fileBuffer;
            string _localFileName = string.Empty;
            string _errMessage = string.Empty;
            string _filePath = string.Empty;
            string _disposition = context.Request.ServerVariables["HTTP_CONTENT_DISPOSITION"];
            _fileBuffer = HanlderHtml5UploadType(context, _disposition, out _localFileName);
            _fileBuffer = HanlderNormalUploadType(context, _disposition, out _localFileName);
            string _fileExt = _localFileName.Substring(_localFileName.LastIndexOf('.') + 1).ToLower();
            CheckResult _checkUploadFileResult = CheckedUploadFile(_fileBuffer, _fileExt);
            _errMessage = _checkUploadFileResult.Message;

            if(_checkUploadFileResult.State)
            {
                _filePath = BuilderUploadFilePath(context, _fileExt);
                ReceiveUploadFile(_fileBuffer, _filePath);
                HanlderUploadImageFile(_filePath, _fileExt);
                this.OnUploaded(context, _filePath);
            }

            _fileBuffer = null;
            context.Response.Write(this.GetResult(_localFileName, _filePath, _errMessage));
            context.Response.End();
        }

        private static string CombinePaths(params string[] paths)
        {
            return paths.Aggregate(Path.Combine);
        }

        /// <summary>
        /// 根据配置规则生成上传文件存储路径
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileExt">文件后缀</param>
        /// <returns>文件路径</returns>
        private string BuilderUploadFilePath(HttpContext context, string fileExt)
        {
            string _filePath = string.Empty;
            string _folder = context.Request["subfolder"] ?? "default";
            UploadFolder _uploadFolderConfig = UploadConfigContext.UploadConfig.UploadFolders.FirstOrDefault(u => string.Equals(_folder, u.Path, StringComparison.OrdinalIgnoreCase));
            DirType _dirType = _uploadFolderConfig == null ? DirType.Day : _uploadFolderConfig.DirType;
            string _subFolder = string.Empty;
            string _fileFolder = string.Empty;

            //根据配置里的DirType决定子文件夹的层次（月，天，扩展名）
            switch(_dirType)
            {
                case DirType.Month:
                    _subFolder = "month_" + DateTime.Now.ToString("yyMM");
                    break;

                case DirType.Ext:
                    _subFolder = "ext_" + fileExt;
                    break;

                case DirType.Day:
                    _subFolder = "day_" + DateTime.Now.ToString("yyMMdd");
                    break;
            }

            _fileFolder = CombinePaths(UploadConfigContext.UploadPath,
                                       _folder,
                                       _subFolder);
            _filePath = Path.Combine(_fileFolder,
                                     string.Format("{0}{1}.{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random(DateTime.Now.Millisecond).Next(10000), fileExt)
                                    );

            if(!Directory.Exists(_fileFolder))
                Directory.CreateDirectory(_fileFolder);

            return _filePath;
        }

        /// <summary>
        /// 检查上传文件是否合法
        /// </summary>
        /// <param name="fileBuffer">文件流</param>
        /// <param name="fileExt">文件后缀</param>
        /// <returns>是否合法</returns>
        private CheckResult CheckedUploadFile(byte[] fileBuffer, string fileExt)
        {
            if(fileBuffer.Length == 0)
            {
                return CheckResult.Fail("无数据提交");
            }

            if(fileBuffer.Length > this.MaxFilesize)
            {
                return CheckResult.Fail("文件大小超过" + this.MaxFilesize + "字节");
            }

            if(!AllowExt.Contains(fileExt))
            {
                return CheckResult.Fail("上传文件扩展名必需为：" + string.Join(",", AllowExt));
            }

            return CheckResult.Success();
        }

        /// <summary>
        /// 处理Html5方式上传
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="disposition">HTTP_CONTENT_DISPOSITION</param>
        /// <param name="localFileName">原始文件名</param>
        /// <returns>文件流</returns>
        private byte[] HanlderHtml5UploadType(HttpContext context, string disposition, out string localFileName)
        {
            byte[] _fileBuffer = null;
            localFileName = string.Empty;

            if(disposition != null)
            {
                // HTML5上传
                _fileBuffer = context.Request.BinaryRead(context.Request.TotalBytes);
                localFileName = Regex.Match(disposition, "filename=\"(.+?)\"").Groups[1].Value;
            }

            return _fileBuffer;
        }

        /// <summary>
        /// 普通形式文件上传
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="disposition">HTTP_CONTENT_DISPOSITION</param>
        /// <param name="localFileName">原始文件名</param>
        /// <returns>文件流</returns>
        private byte[] HanlderNormalUploadType(HttpContext context, string disposition, out string localFileName)
        {
            localFileName = string.Empty;
            byte[] _fileBuffer = null;

            if(disposition == null)
            {
                HttpFileCollection _filecollection = context.Request.Files;
                HttpPostedFile _postedfile = _filecollection.Get(this.FileInputName);
                localFileName = Path.GetFileName(_postedfile.FileName);
                _fileBuffer = new byte[_postedfile.ContentLength];

                using(Stream stream = _postedfile.InputStream)
                {
                    stream.Read(_fileBuffer, 0, _postedfile.ContentLength);

                    if(_filecollection != null)
                        _filecollection = null;
                }
            }

            return _fileBuffer;
        }

        /// <summary>
        /// 处理上传文件是图片类型的时候
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="fileExt">文件后缀</param>
        private void HanlderUploadImageFile(string filePath, string fileExt)
        {
            //是图片，即使生成对应尺寸
            if(ImageExt.Contains(fileExt))
                ThumbnailService.HandleImmediateThumbnail(filePath);
        }

        /// <summary>
        /// 将上传文件写入服务器路径
        /// </summary>
        /// <param name="fileBuffer">文件流</param>
        /// <param name="filePath">保存路径</param>
        private void ReceiveUploadFile(byte[] fileBuffer, string filePath)
        {
            using(FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(fileBuffer, 0, fileBuffer.Length);
                fileStream.Flush();
            }
        }

        #endregion Methods
    }
}