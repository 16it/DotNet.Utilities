using System.IO;

namespace YanZhiwei.DotNet3._5.Utilities.Model
{
    /// <summary>
    /// 上传图片返回信息
    /// </summary>
    public class UploadImageInfo
    {
        #region Properties
        
        /// <summary>
        /// 图片目录
        /// </summary>
        public string Directory
        {
            get
            {
                if(WebPath == null) return null;
                
                return WebPath.Replace(FileName, "");
            }
        }
        
        /// <summary>
        /// 图片名
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        
        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否遇到错误
        /// </summary>
        public bool IsError
        {
            get;
            set;
        }
        
        /// <summary>
        /// 反回消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }
        
        /// <summary>
        /// 文件大小
        /// </summary>
        public double Size
        {
            get;
            set;
        }
        
        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileEx
        {
            get;
            set;
        }
        
        /// <summary>
        /// web路径
        /// </summary>
        public string WebPath
        {
            get;
            set;
        }
        
        /// <summary>
        ///  缩略图宽度规则
        /// </summary>
        public int[] ThumbnailWidth
        {
            get;
            set;
        }
        
        /// <summary>
        ///  缩略图高度规则
        /// </summary>
        public int[] ThumbnailHeight
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否创建缩略图
        /// </summary>
        public bool IsCreateThumbnail
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否设置图片水印
        /// </summary>
        public bool IsSetPicWater
        {
            get;
            set;
        }
        /// <summary>
        /// 是否设置文字水印
        /// </summary>
        public bool IsSetWordWater
        {
            get;
            set;
        }
        /// <summary>
        /// 图片原始宽度
        /// </summary>
        public int SourceWidth
        {
            get;
            set;
        }
        
        /// <summary>
        /// 图片原始高度
        /// </summary>
        public int SourceHeight
        {
            get;
            set;
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>
        /// 缩略图路径
        /// </summary>
        public string SmallPath(int index)
        {
            return string.Format("{0}{1}_{2}{3}", Directory, Path.GetFileNameWithoutExtension(FileName), index, Path.GetExtension(FileName));
        }
        
        #endregion Methods
    }
}