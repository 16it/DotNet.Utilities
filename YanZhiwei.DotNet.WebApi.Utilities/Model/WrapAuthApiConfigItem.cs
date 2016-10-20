namespace YanZhiwei.DotNet.WebApi.Utilities.Model
{
    internal class WrapAuthApiConfigItem
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
    }
}