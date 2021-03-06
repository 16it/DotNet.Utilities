﻿namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Core
{
    using DotNet2.Utilities.Common;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using YanZhiwei.DotNet2.Utilities.Operator;
    using YanZhiwei.DotNet2.Utilities.Result;
    using YanZhiwei.DotNet3._5.Utilities.Enum;
    using YanZhiwei.DotNet3._5.Utilities.Model;

    /// <summary>
    /// 单文件上传
    /// </summary>
    public class WebUploadFile
    {
        #region Fields

        private UploadFileConfig uploadFileSetting;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// 时间：2015-12-17 11:13
        /// 备注：
        public WebUploadFile()
        {
            uploadFileSetting = new UploadFileConfig()
            {
                FileDirectory = "/upload",
                FileType = ".pdf,.xls,.xlsx,.doc,.docx,.txt,.png,.jpg,.gif",
                MaxSizeM = 10,
                PathSaveType = UploadFileSaveType.DateTimeNow,
                IsRenameSameFile = true
            };
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 若以文件夹编号命名的时候，采用编号
        /// </summary>
        private string folderNumber
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 保存表单文件
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <returns>上传返回信息</returns>
        public OperatedResult<UploadFileInfo> Save(HttpPostedFile postFile)
        {
            return SaveUploadFile(postFile);
        }

        /// <summary>
        /// 保存表单文件,根据编号创建子文件夹
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <param name="number">编号</param>
        /// <returns>上传返回信息</returns>
        public OperatedResult<UploadFileInfo> Save(HttpPostedFile postFile, string number)
        {
            uploadFileSetting.PathSaveType = UploadFileSaveType.Code;
            folderNumber = number;
            return SaveUploadFile(postFile);
        }

        /// <summary>
        /// 文件保存路径(默认:/upload)
        /// </summary>
        public void SetFileDirectory(string fileDirectory)
        {
            ValidateOperator.Begin().NotNullOrEmpty(fileDirectory, "保存路径");
            bool _mapServerPath = Regex.IsMatch(fileDirectory, @"[a-z]\:\\", RegexOptions.IgnoreCase);
            uploadFileSetting.FileDirectory = _mapServerPath == true ? GetRelativePath(fileDirectory) : fileDirectory;
        }

        /// <summary>
        /// 允许上传的文件类型, 逗号分割,必须全部小写! *表示所有 (默认值: .pdf,.xls,.xlsx,.doc,.docx,.txt,.png,.jpg,.gif )
        /// </summary>
        /// <param name="fileType">允许上传文件类型</param>
        public void SetFileType(string fileType)
        {
            uploadFileSetting.FileType = fileType;
        }

        /// <summary>
        /// 重命名同名文件？
        /// </summary>
        /// <param name="isRenameSameFile">true:重命名,false覆盖</param>
        public void SetIsRenameSameFile(bool isRenameSameFile)
        {
            uploadFileSetting.IsRenameSameFile = isRenameSameFile;
        }

        /// <summary>
        /// 是否使用原始文件名作为新文件的文件名(默认:true)
        /// </summary>
        /// <param name="isUseOldFileName">true原始文件名,false系统生成新文件名</param>
        public void SetIsUseOldFileName(bool isUseOldFileName)
        {
            uploadFileSetting.IsUseOldFileName = isUseOldFileName;
        }

        /// <summary>
        /// 允许上传多少大小(单位：M)
        /// </summary>
        /// <param name="maxSizeM">(单位：M)</param>
        public void SetMaxSizeM(double maxSizeM)
        {
            uploadFileSetting.MaxSizeM = maxSizeM;
        }

        /// <summary>
        /// 根据物理路径获取相对路径
        /// </summary>
        /// <param name="fileDirectory">文件夹名称</param>
        /// <returns>相对路径</returns>
        private static string GetRelativePath(string fileDirectory)
        {
            fileDirectory = "/" + fileDirectory.Replace(HttpContext.Current.Server.MapPath("~/"), "").TrimStart('/').Replace('\\', '/');
            return fileDirectory;
        }

        private CheckResult<string> CheckedFileParamter(HttpPostedFile postFile)
        {
            if (postFile == null && postFile.ContentLength == 0)
            {
                return CheckResult<string>.Fail("没有文件");
            }

            //文件名
            string _fileName = uploadFileSetting.IsUseOldFileName ? postFile.FileName : DateTime.Now.FormatDate(12) + Path.GetExtension(postFile.FileName);
            //验证格式
            CheckResult<string> _checkFileTypeResult = CheckingType(postFile.FileName);

            if (!_checkFileTypeResult.State)
            {
                return _checkFileTypeResult;
            }

            //验证大小
            CheckResult<string> _checkFileSizeResult = CheckSize(postFile);

            if (!_checkFileSizeResult.State)
            {
                return _checkFileSizeResult;
            }

            return CheckResult<string>.Success(_fileName);
        }

        /// <summary>
        /// 验证文件类型
        /// </summary>
        /// <param name="fileName">文件名称.</param>
        private CheckResult<string> CheckingType(string fileName)
        {
            if (uploadFileSetting.FileType != "*")
            {
                // 获取允许允许上传类型列表
                string[] _typeList = uploadFileSetting.FileType.Split(',');
                // 获取上传文件类型(小写)
                string _type = Path.GetExtension(fileName).ToLowerInvariant(); 

                // 验证类型
                if (_typeList.Contains(_type) == false)
                    return CheckResult<string>.Fail("文件类型非法");
            }

            return CheckResult<string>.Success();
        }

        /// <summary>
        /// 检查文件大小
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        private CheckResult<string> CheckSize(HttpPostedFile postFile)
        {
            if (postFile.ContentLength / 1024.0 / 1024.0 > uploadFileSetting.MaxSizeM)
            {
                return CheckResult<string>.Fail(string.Format("对不起上传文件过大，不能超过{0}M！", uploadFileSetting.MaxSizeM));
            }

            return CheckResult<string>.Success();
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="webDir">网络目录名称</param>
        /// <returns>目录</returns>
        private string GetDirectory(ref string webDir)
        {
            // 存储目录
            string _directory = uploadFileSetting.FileDirectory;
            // 目录格式
            string _childDirectory = DateTime.Now.ToString("yyyy-MM/dd");

            if (uploadFileSetting.PathSaveType == UploadFileSaveType.Code)
            {
                _childDirectory = folderNumber;
            }

            webDir = _directory.TrimEnd('/') + "/" + _childDirectory + '/';
            string _dir = HttpContext.Current.Server.MapPath(webDir);

            // 创建目录
            if (!Directory.Exists(_dir))
                Directory.CreateDirectory(_dir);

            return _dir;
        }

        /// <summary>
        /// 保存表单文件,根据HttpPostedFile
        /// </summary>
        /// <param name="postFile">HttpPostedFile</param>
        /// <returns>上传返回信息</returns>
        private OperatedResult<UploadFileInfo> SaveUploadFile(HttpPostedFile postFile)
        {
            try
            {
                CheckResult<string> _checkedFileParamter = CheckedFileParamter(postFile);

                if (!_checkedFileParamter.State)
                    return OperatedResult<UploadFileInfo>.Fail(_checkedFileParamter.Message);

                string _fileName = _checkedFileParamter.Data;
                string _webDir = string.Empty;
                // 获取存储目录
                string _directory = this.GetDirectory(ref _webDir),
                       _filePath = _directory + _fileName;

                if (File.Exists(_filePath))
                {
                    if (uploadFileSetting.IsRenameSameFile)
                    {
                        _filePath = _directory + DateTime.Now.FormatDate(12) + "-" + _fileName;
                    }
                    else
                    {
                        File.Delete(_filePath);
                    }
                }

                // 保存文件
                postFile.SaveAs(_filePath);
                UploadFileInfo _uploadFileInfo = new UploadFileInfo();
                _uploadFileInfo.FilePath = _filePath;
                _uploadFileInfo.FilePath = _webDir + _fileName;
                _uploadFileInfo.FileName = _fileName;
                _uploadFileInfo.WebFilePath = _webDir + _fileName;
                return OperatedResult<UploadFileInfo>.Success(_uploadFileInfo);
            }
            catch (Exception ex)
            {
                return OperatedResult<UploadFileInfo>.Fail(ex.Message);
            }
        }

        #endregion Methods
    }
}