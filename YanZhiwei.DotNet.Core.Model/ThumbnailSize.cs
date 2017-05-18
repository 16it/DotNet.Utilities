using System;
using System.Xml.Serialization;
using YanZhiwei.DotNet3._5.Utilities.Enum;

namespace YanZhiwei.DotNet.Core.Model
{
    /// <summary>
    /// 缩略图配置规则
    /// </summary>
    [Serializable]
    public class ThumbnailSize
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ThumbnailSize()
        {
            this.Quality = 88;
            this.Mode = ThumbnailImageCutMode.Cut;
            this.Timming = ThumbnailTimming.Lazy;
            this.WaterMarkerPosition = WatermarkImagesPosition.Default;
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [XmlAttribute("Width")]
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// 高度
        /// </summary>
        [XmlAttribute("Height")]
        public int Height
        {
            get;
            set;
        }

        /// <summary>
        /// 图片质量
        /// </summary>
        [XmlAttribute("Quality")]
        public int Quality
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否添加水印图片
        /// </summary>
        [XmlAttribute("AddWaterMarker")]
        public bool AddWaterMarker
        {
            get;
            set;
        }

        /// <summary>
        /// 水印图片的位置
        /// </summary>
        [XmlAttribute("WaterMarkerPosition")]
        public WatermarkImagesPosition WaterMarkerPosition
        {
            get;
            set;
        }

        /// <summary>
        /// 水印图片物理路径
        /// </summary>
        [XmlAttribute("WaterMarkerPath")]
        public string WaterMarkerPath
        {
            get;
            set;
        }
        /// <summary>
        /// 缩略图处理模式
        /// </summary>
        [XmlAttribute("Mode")]
        public ThumbnailImageCutMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// 缩略图生成时序
        /// </summary>
        [XmlAttribute("Timming")]
        public ThumbnailTimming Timming
        {
            get;
            set;
        }

        /// <summary>
        /// 是否替换
        /// </summary>
        [XmlAttribute("IsReplace")]
        public bool IsReplace
        {
            get;
            set;
        }
    }
}