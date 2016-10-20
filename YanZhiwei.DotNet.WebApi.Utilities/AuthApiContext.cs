using YanZhiwei.DotNet.WebApi.Utilities.Model;

namespace YanZhiwei.DotNet.WebApi.Utilities
{
    /// <summary>
    /// AuthApi配置上下文
    /// </summary>
    /// 时间：2016/10/20 13:58
    /// 备注：
    public class AuthApiContext
    {
        /// <summary>
        /// CacheConfig
        /// </summary>
        internal static WrapAuthApiConfigItem ConfigItem
        {
            get
            {
                WrapAuthApiConfigItem _item = new WrapAuthApiConfigItem();
                _item.AppId = "";
                _item.TimspanExpiredMinutes = 5;
                return _item;
            }
        }
    }
}