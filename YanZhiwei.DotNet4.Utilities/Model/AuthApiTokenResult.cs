using System.Net;
using YanZhiwei.DotNet3._5.Utilities.Common;
using YanZhiwei.DotNet3._5.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.Model
{
    /// <summary>
    /// WEB API 返回结果结果实体类
    /// </summary>
    /// 时间：2016/10/19 14:14
    /// 备注：
    public class AuthApiTokenResult : JsonResult
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        public AuthApiToken Result
        {
            get
            {
                if(StatusCode == (int)HttpStatusCode.OK)
                {
                    return SerializeHelper.JsonDeserialize<AuthApiToken>(Content == null ? string.Empty : Content.ToString());
                }
                
                return null;
            }
        }
    }
}