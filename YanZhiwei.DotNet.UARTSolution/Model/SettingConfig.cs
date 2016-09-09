using System;
using YanZhiwei.DotNet.Core.Model;

namespace YanZhiwei.DotNet.UARTSolution.Model
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    [Serializable]
    public class SettingConfig : ConfigFileBase
    {
        public SettingConfig()
        {
        }
        
        #region 序列化属性
        
        public string WebSiteTitle
        {
            get;
            set;
        }
        
        public string WebSiteDescription
        {
            get;
            set;
        }
        
        public string WebSiteKeywords
        {
            get;
            set;
        }
        
        #endregion 序列化属性
    }
}