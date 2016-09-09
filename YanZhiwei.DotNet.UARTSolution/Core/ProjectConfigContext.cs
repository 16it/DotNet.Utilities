using YanZhiwei.DotNet.Core.Config;
using YanZhiwei.DotNet.UARTSolution.Model;

namespace YanZhiwei.DotNet.UARTSolution.Core
{
    /// <summary>
    /// 项目配置上下文
    /// </summary>
    public class ProjectConfigContext
    {
        /// <summary>
        /// 系统设置
        /// </summary>
        public static SettingConfig SettingConfig
        {
            get
            {
                return CachedConfigContext.Current.Get<SettingConfig>();
            }
        }
    }
}