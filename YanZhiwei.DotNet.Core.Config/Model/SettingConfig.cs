using System;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// 网站基本信息
    /// </summary>
    [Serializable]
    public class SettingConfig : ConfigFileBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingConfig()
        {
        }

        #region 序列化属性

        /// <summary>
        /// 网站标题
        /// </summary>
        public String WebSiteTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 网站描述
        /// </summary>
        public String WebSiteDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 网站关键字
        /// </summary>
        public String WebSiteKeywords
        {
            get;
            set;
        }

        #endregion 序列化属性
    }
}