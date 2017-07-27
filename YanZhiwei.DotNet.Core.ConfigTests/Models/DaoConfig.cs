using System;
using YanZhiwei.DotNet.Core.Config.Model;

namespace YanZhiwei.DotNet.Core.Config.Tests.Models
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

        public string Log
        {
            get;
            set;
        }

        #endregion 序列化属性
    }
}