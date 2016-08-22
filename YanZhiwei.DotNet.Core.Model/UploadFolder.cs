using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YanZhiwei.DotNet.Core.Model
{
    [Serializable]
    public class UploadFolder
    {
        public UploadFolder()
        {
            this.Path = "Default";
            this.DirType = DirType.Day;
        }
        
        public List<ThumbnailSize> ThumbnailSizes
        {
            get;
            set;
        }
        
        [XmlAttribute("Path")]
        public string Path
        {
            get;
            set;
        }
        
        [XmlAttribute("DirType")]
        public DirType DirType
        {
            get;
            set;
        }
    }
}