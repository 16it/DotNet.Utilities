using System;

namespace YanZhiwei.DotNet.Core.Model
{
    /// <summary>
    /// WEB API 用户令牌验证实体类
    /// </summary>
    /// 时间：2016/10/26 9:56
    /// 备注：
    [Serializable]
    public class AuthWebApiConfig : ConfigFileBase
    {
        /// <summary>
        /// 时间戳过期时间【分钟】
        /// </summary>
        public int TimspanExpiredMinutes
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用户令牌【天】
        /// </summary>
        public int TokenExpiredDays
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用于加密解密签名以及用户令牌的Key
        /// </summary>
        public string SharedKey
        {
            get;
            set;
        }
    }
}