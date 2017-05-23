﻿namespace YanZhiwei.DotNet.Framework.Contract
{
    using System;

    /// <summary>
    /// 操作者
    /// </summary>
    /// 时间：2016-01-13 17:19
    /// 备注：
    [Serializable]
    public class Operater
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// 时间：2016-01-13 17:19
        /// 备注：
        public Operater()
        {
            this.Name = "Anonymous";
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 访问Ip
        /// </summary>
        public string IP
        {
            get;
            set;
        }

        /// <summary>
        /// Method
        /// </summary>
        public string Method
        {
            get;
            set;
        }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time
        {
            get;
            set;
        }

        /// <summary>
        /// 凭据
        /// </summary>
        public Guid Token
        {
            get;
            set;
        }

        /// <summary>
        /// 用户主键
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户主键GUID
        /// </summary>
        /// 时间：2016/11/8 15:13
        /// 备注：
        public Guid UserGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 用户主键Base64字符串
        /// </summary>
        public string UserBase64
        {
            get;
            set;
        }

        #endregion Properties
    }
}