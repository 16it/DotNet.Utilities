namespace YanZhiwei.DotNet.Core.Config.Model
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    
    /// <summary>
    /// 文件上传文件夹配置
    /// </summary>
    [Serializable]
    public class UploadFolder
    {
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public UploadFolder()
        {
            this.Path = "Default";
            this.DirType = UploadSaveDirType.Day;
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// 存储文件夹规则
        /// </summary>
        [XmlAttribute("DirType")]
        public UploadSaveDirType DirType
        {
            get;
            set;
        }
        
        /// <summary>
        /// 存储路径
        /// </summary>
        [XmlAttribute("Path")]
        public string Path
        {
            get;
            set;
        }
        
        /// <summary>
        /// 缩略图处理规则
        /// </summary>
        public List<ThumbnailSize> ThumbnailSizes
        {
            get;
            set;
        }
        
        #endregion Properties
    }
}