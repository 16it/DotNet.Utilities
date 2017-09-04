using System;

namespace YanZhiwei.DotNet.Dapper.UtilitiesTests
{
    public sealed class EventModel : ModelBase
    {
        /// <summary>
        /// 协议的命令字
        /// </summary>
        public byte OrderCmd { get; set; }

        /// <summary>
        /// 协议流水号
        /// </summary>
        public ushort OrderSeqNo { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime TimeStamps { get; set; }

        /// <summary>
        /// 协议号
        /// </summary>
        public string ProtocolVer { get; set; }

        /// <summary>
        /// 源地址
        /// </summary>
        public string SourceAddr { get; set; }

        /// <summary>
        /// 目的地址
        /// </summary>
        public string DescAddr { get; set; }

        /// <summary>
        /// 系统地址
        /// </summary>
        public byte SystemAddr { get; set; }

        /// <summary>
        /// 数据报文对应十六进制
        /// </summary>
        public string FullPackageDataHexString { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public byte ComponentAddr
        {
            get;
            set;
        }

        /// <summary>
        /// 系统标识
        /// </summary>
        public byte ComponentType { get; set; }

        /// <summary>
        /// 回路
        /// </summary>
        public byte CtuCh
        {
            get;
            set;
        }

        /// <summary>
        /// 网络节点地址
        /// </summary>
        public byte NetworkNodeAddr
        {
            get;
            set;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 属性数值
        /// </summary>
        public ushort PropertyValue
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有属性数值
        /// </summary>
        public bool HasPropertyValue
        {
            get;
            set;
        }
    }
}