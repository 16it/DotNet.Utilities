using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// 后台菜单分类
    /// </summary>
    [Serializable]
    public class AdminMenuGroup
    {
        /// <summary>
        /// 后台菜单分类子集合
        /// </summary>
        public List<AdminMenu> AdminMenuArray
        {
            get;
            set;
        }
        
        /// <summary>
        /// Id
        /// </summary>
        [XmlAttribute("id")]
        public string Id
        {
            get;
            set;
        }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }
        
        /// <summary>
        ///链接
        /// </summary>
        [XmlAttribute("url")]
        public string Url
        {
            get;
            set;
        }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        [XmlAttribute("icon")]
        public string Icon
        {
            get;
            set;
        }
        
        /// <summary>
        /// 访问权限
        /// </summary>
        [XmlAttribute("permission")]
        public string Permission
        {
            get;
            set;
        }
        
        /// <summary>
        ///菜单信息
        /// </summary>
        [XmlAttribute("info")]
        public string Info
        {
            get;
            set;
        }
    }
}