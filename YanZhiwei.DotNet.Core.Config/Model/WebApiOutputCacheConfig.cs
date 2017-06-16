using System;

namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// WebApi缓存配置项
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Config.Model.ConfigFileBase" />
    [Serializable]
    public class WebApiOutputCacheConfig : ConfigFileBase
    {
        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public bool EnableOutputCache
        {
            get;
            set;
        }
    }
}