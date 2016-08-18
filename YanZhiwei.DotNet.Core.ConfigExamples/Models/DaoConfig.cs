using System;
using YanZhiwei.DotNet.Core.Model;

namespace YanZhiwei.DotNet.Core.ConfigExamples.Models
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

        public string Account
        {
            get;
            set;
        }

        public string Log
        {
            get;
            set;
        }

        public string Cms
        {
            get;
            set;
        }

        public string Crm
        {
            get;
            set;
        }

        public string OA
        {
            get;
            set;
        }

        #endregion 序列化属性
    }
}