using System;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    [Serializable]
    public class SystemConfig : ConfigFileBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemConfig()
        {
        }
        
        /// <summary>
        /// 用户登录超时时间
        /// </summary>
        
        #region 序列化属性
        
        public int UserLoginTimeoutMinutes
        {
            get;
            set;
        }
        
        #endregion 序列化属性
    }
}