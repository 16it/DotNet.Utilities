using System;
using YanZhiwei.DotNet.Core.Model;

namespace YanZhiwei.DotNet.MVC.AdminPanel.Config.Model
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    [Serializable]
    public class DaoConfig : ConfigFileBase
    {
        public DaoConfig()
        {
        }

        #region 序列化属性

        public String AdminPanel { get; set; }

        #endregion 序列化属性
    }
}