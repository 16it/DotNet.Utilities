using System;
using System.Xml.Serialization;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    [Serializable]
    public class AdminMenu
    {
        [XmlAttribute("id")]
        public string Id
        {
            get;
            set;
        }

        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute("url")]
        public string Url
        {
            get;
            set;
        }

        [XmlAttribute("info")]
        public string Info
        {
            get;
            set;
        }

        [XmlAttribute("permission")]
        public string Permission
        {
            get;
            set;
        }
    }
}