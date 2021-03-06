﻿namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    using DotNet2.Utilities.Common;
    using DotNet2.Utilities.Enum;
    using DotNet2.Utilities.Model;
    using DotNet2.Utilities.Result;

    using Model;

    /// <summary>
    /// ASP.NET 图片上传
    /// </summary>
    public class WebUploadImage
    {
        #region Constructors

        /// <summary>
        /// 构造方法
        /// </summary>
        public WebUploadImage()
        {
            SetAllowFormat = ImageHelper.AllowExt;   //允许图片格式
            SetAllowSize = 5;       //允许上传图片大小,默认为5MB
            SetPositionWater = SetWaterPosition.bottomRight;
            SetCutImage = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 允许图片格式
        /// </summary>
        public string SetAllowFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 允许上传图片大小
        /// </summary>
        public double SetAllowSize
        {
            get;
            set;
        }

        /// <summary>
        /// 是否剪裁图片，默认true
        /// </summary>
        public bool SetCutImage
        {
            get;
            set;
        }

        /// <summary>
        /// 是否限制最大宽度，默认为true
        /// </summary>
        public bool SetLimitWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 最大宽度尺寸，默认为600
        /// </summary>
        public int SetMaxWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 限制图片最小宽度，0表示不限制
        /// </summary>
        public int SetMinWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        public string SetPicWater
        {
            get;
            set;
        }

        /// <summary>
        /// 水印图片的位置 0居中、1左上角、2右上角、3左下角、4右下角
        /// </summary>
        public SetWaterPosition SetPositionWater
        {
            get;
            set;
        }

        /// <summary>
        /// 缩略图高度多个逗号格开（例如:200,100）
        /// </summary>
        public int[] SetSmallImgHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 缩略图宽度多个逗号格开（例如:200,100）
        /// </summary>
        public int[] SetSmallImgWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 文字水印字符
        /// </summary>
        public string SetWordWater
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 通用图片上传类
        /// </summary>
        /// <param name="postedFile">HttpPostedFile控件</param>
        /// <param name="savePath">保存路径</param>
        /// <returns>返回上传信息</returns>
        public OperatedResult<UploadImageInfo> FileSaveAs(HttpPostedFile postedFile, string savePath)
        {
            try
            {
                OperatedResult<UploadImageInfo> _uploadImageInfo = GetUploadImageInfo(postedFile, savePath);

                if(_uploadImageInfo.State)
                {
                    string _fullPath = _uploadImageInfo.Data.FilePath,
                           _fileEx = _uploadImageInfo.Data.FileEx,
                           _fileName = _uploadImageInfo.Data.FileName;
                    int _sourceWidth = _uploadImageInfo.Data.SourceWidth,
                        _sourceHeight = _uploadImageInfo.Data.SourceHeight;
                    HanlderImageSourceWidthOverMax(savePath, _sourceWidth, _sourceHeight, _fileEx, _fullPath);
                    HanlderImageZip(_fullPath, _fileEx);
                    HanlderThumbnailImage(_fullPath, savePath, _fileName, _fileEx, _sourceWidth, _sourceHeight, _uploadImageInfo.Data);
                    HanlderSetPicWater(_fullPath, _uploadImageInfo.Data);
                    HanlderSetTextWater(_fullPath, _uploadImageInfo.Data);
                    return OperatedResult<UploadImageInfo>.Success(_uploadImageInfo.Data);
                }

                return _uploadImageInfo;
            }

            catch(Exception ex)
            {
                return OperatedResult<UploadImageInfo>.Fail(ex.Message);
            }
        }

        private CheckResult CheckedPostFile(HttpPostedFile postedFile)
        {
            if(postedFile == null && postedFile.ContentLength == 0)
            {
                return CheckResult.Fail(GetCodeMessage(4));
            }

            return CheckResult.Success();
        }

        private CheckResult CheckedUploadImageParamter(string fileEx, double fileSize)
        {
            if(!FileHelper.CheckValidExt(SetAllowFormat, fileEx))
            {
                return CheckResult.Fail(GetCodeMessage(2));
            }

            if(fileSize > SetAllowSize)
            {
                return CheckResult.Fail(GetCodeMessage(3));
            }

            return CheckResult.Success();
        }

        /// <summary>
        /// 图片上传错误编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetCodeMessage(int code)
        {
            Dictionary<int, string> _uploadImageCode = new Dictionary<int, string>()
            {
                {0, "系统配置错误"},
                {1, "上传图片成功"},
                {2, string.Format("对不起，上传格式错误！请上传{0}格式图片", SetAllowFormat)},
                {3, string.Format("超过文件上传大小,不得超过{0}M", SetAllowSize)},
                {4, "未上传文件"},
                {5, ""},
                {6, "缩略图长度和宽度配置错误"},
                {7, "检测图片宽度限制"}
            };
            return _uploadImageCode[code];
        }

        private OperatedResult<UploadImageInfo> GetUploadImageInfo(HttpPostedFile postedFile, string savePath)
        {
            CheckResult _checkedPostFileResult = CheckedPostFile(postedFile);

            if(!_checkedPostFileResult.State)
                return OperatedResult<UploadImageInfo>.Fail(_checkedPostFileResult.Message);

            int _randomNumber = RandomHelper.NextNumber(1000, 9999);
            string _fileName = DateTime.Now.FormatDate(12) + _randomNumber,
                   _fileEx = Path.GetExtension(postedFile.FileName);
            double _fileSize = postedFile.ContentLength / 1024.0 / 1024.0;
            CheckResult _checkedUploadImageResult = CheckedUploadImageParamter(_fileEx, _fileSize);

            if(!_checkedUploadImageResult.State)
                return OperatedResult<UploadImageInfo>.Fail(_checkedUploadImageResult.Message);

            UploadImageInfo _uploadImageInfo = new UploadImageInfo();
            _uploadImageInfo.FileName = _fileName + _fileEx;
            _uploadImageInfo.FilePath = savePath.Trim('\\') + "\\" + _uploadImageInfo.FileName;
            _uploadImageInfo.WebPath = "/" + _uploadImageInfo.FilePath.Replace(HttpContext.Current.Server.MapPath("~/"), "").Replace("\\", "/");
            _uploadImageInfo.Size = _fileSize;
            _uploadImageInfo.FileEx = _fileEx;
            _uploadImageInfo.IsSetPicWater = !string.IsNullOrEmpty(SetPicWater);
            _uploadImageInfo.IsSetWordWater = !string.IsNullOrEmpty(SetWordWater);
            BitmapInfo _iamgeInfo = ImageHelper.GetBitmapInfo(postedFile.FileName);
            _uploadImageInfo.SourceWidth = _iamgeInfo.Width;
            _uploadImageInfo.SourceHeight = _iamgeInfo.Height;
            _uploadImageInfo.IsCreateThumbnail = SetSmallImgWidth.Length != 0;
            _uploadImageInfo.ThumbnailHeight = SetSmallImgHeight;
            _uploadImageInfo.ThumbnailWidth = SetSmallImgWidth;

            if(_uploadImageInfo.IsCreateThumbnail && SetSmallImgHeight.Length != SetSmallImgHeight.Length)
            {
                return OperatedResult<UploadImageInfo>.Fail(GetCodeMessage(6));
            }

            FileHelper.CreateDirectory(savePath);
            postedFile.SaveAs(_uploadImageInfo.FilePath);
            return OperatedResult<UploadImageInfo>.Success(_uploadImageInfo);
        }

        /// <summary>
        /// 如果设置图片最大宽度，如果原始图片超过则处理
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="sourceWidth">图片原始宽度</param>
        /// <param name="sourceHeight">图片原始高度</param>
        /// <param name="fileEx">图片后缀</param>
        /// <param name="fullPath">图片原始路径</param>
        private void HanlderImageSourceWidthOverMax(string savePath, int sourceWidth, int sourceHeight, string fileEx, string fullPath)
        {
            if(SetLimitWidth && sourceWidth > SetMaxWidth)
            {
                int _width = SetMaxWidth;
                int _height = _width * sourceHeight / sourceWidth;
                string _tempFile = savePath + Guid.NewGuid().ToString() + fileEx;
                File.Move(fullPath, _tempFile);
                ImageHelper.CreateSmallPhoto(_tempFile, _width, _height, fullPath);
                File.Delete(_tempFile);
            }
        }

        /// <summary>
        /// 压缩图片存储尺寸
        /// </summary>
        /// <param name="fullPath">图片原始路径</param>
        /// <param name="fileEx">图片后缀</param>
        private void HanlderImageZip(string fullPath, string fileEx)
        {
            if(fileEx.ToLower() != ".gif")
            {
                ImageHelper.CompressPhoto(fullPath, 100);
            }
        }

        private void HanlderSetPicWater(string fullPath, UploadImageInfo uploadImageInfo)
        {
            if(uploadImageInfo.IsSetPicWater)
                ImageHelper.AttachPng(SetPicWater, fullPath, SetPositionWater);
        }

        private void HanlderSetTextWater(string fullPath, UploadImageInfo uploadImageInfo)
        {
            if(uploadImageInfo.IsSetPicWater)
                ImageHelper.AttachText(SetWordWater, fullPath);
        }

        private void HanlderThumbnailImage(string fullPath, string savePath, string fileName, string fileEx, int sourceWidth, int sourceHeight, UploadImageInfo uploadImageInfo)
        {
            if(uploadImageInfo.IsCreateThumbnail)
            {
                int[] _thumbnailWidthRule = uploadImageInfo.ThumbnailWidth,
                      _thumbnailHeightRule = uploadImageInfo.ThumbnailHeight;

                for(int i = 0; i < _thumbnailWidthRule.Length; i++)
                {
                    if(_thumbnailWidthRule[i] <= 0 || _thumbnailHeightRule[i] <= 0)
                        continue;

                    string _descFile = savePath.TrimEnd('\\') + '\\' + fileName + "_" + i.ToString() + fileEx;

                    //判断图片高宽是否大于生成高宽。否则用原图
                    if(sourceWidth > Convert.ToInt32(_thumbnailWidthRule[i]))
                    {
                        if(SetCutImage)
                        {
                            ImageHelper.CreateSmallPhoto(fullPath, _thumbnailWidthRule[i], _thumbnailHeightRule[i], _descFile);
                        }

                        else
                        {
                            ImageHelper.CreateSmallPhoto(fullPath, _thumbnailWidthRule[i], _thumbnailHeightRule[i], _descFile, CutType.CutNo);
                        }
                    }

                    else
                    {
                        if(SetCutImage)
                        {
                            ImageHelper.CreateSmallPhoto(fullPath, sourceWidth, sourceHeight, _descFile);
                        }

                        else
                        {
                            ImageHelper.CreateSmallPhoto(fullPath, sourceWidth, sourceHeight, _descFile, CutType.CutNo);
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}