using System;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// 后台菜单配置
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Config.Model.ConfigFileBase" />
    [Serializable]
    public class AdminMenuConfig : ConfigFileBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AdminMenuConfig()
        {
        }
        
        /// <summary>
        /// 后台菜单分类
        /// </summary>
        public AdminMenuGroup[] AdminMenuGroups
        {
            get;
            set;
        }
    }
}