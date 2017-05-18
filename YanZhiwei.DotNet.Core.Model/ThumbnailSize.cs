using System;
using System.Xml.Serialization;
using YanZhiwei.DotNet3._5.Utilities.Enum;

namespace YanZhiwei.DotNet.Core.Model
{
    [Serializable]
    public class ThumbnailSize
    {
        public ThumbnailSize()
        {
            this.Quality = 88;
            this.Mode = ThumbnailImageCutMode.Cut;
            this.Timming = Timming.Lazy;
            this.WaterMarkerPosition = WatermarkImagesPosition.Default;
        }

        [XmlAttribute("Width")]
        public int Width
        {
            get;
            set;
        }

        [XmlAttribute("Height")]
        public int Height
        {
            get;
            set;
        }

        [XmlAttribute("Quality")]
        public int Quality
        {
            get;
            set;
        }

        [XmlAttribute("AddWaterMarker")]
        public bool AddWaterMarker
        {
            get;
            set;
        }

        [XmlAttribute("WaterMarkerPosition")]
        public WatermarkImagesPosition WaterMarkerPosition
        {
            get;
            set;
        }

        [XmlAttribute("WaterMarkerPath")]
        public string WaterMarkerPath
        {
            get;
            set;
        }

        [XmlAttribute("Mode")]
        public ThumbnailImageCutMode Mode
        {
            get;
            set;
        }

        [XmlAttribute("Timming")]
        public Timming Timming
        {
            get;
            set;
        }

        [XmlAttribute("IsReplace")]
        public bool IsReplace
        {
            get;
            set;
        }
    }
}