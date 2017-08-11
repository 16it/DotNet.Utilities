using System;
using System.Xml.Serialization;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// Redis 配置
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Config.Model.ConfigFileBase" />
    [Serializable]
    public class RedisConfig : ConfigFileBase
    {
        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public string WriteServerList
        {
            get;
            set;
        }
        
        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public string ReadServerList
        {
            get;
            set;
        }
        
        /// <summary>
        /// 最大写链接数
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public int MaxWritePoolSize
        {
            get;
            set;
        }
        
        /// <summary>
        /// 最大读链接数
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public int MaxReadPoolSize
        {
            get;
            set;
        }
        
        /// <summary>
        /// 自动重启
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public bool AutoStart
        {
            get;
            set;
        }
        
        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public int LocalCacheTime
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        [XmlAttribute("WriteServerList")]
        public bool RecordeLog
        {
            get;
            set;
        }
    }
}