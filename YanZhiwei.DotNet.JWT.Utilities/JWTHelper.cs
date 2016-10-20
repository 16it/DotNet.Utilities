using JWT;
using System.Collections.Generic;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.JWT.Utilities
{
    /// <summary>
    /// JWT 辅助类
    /// </summary>
    /// 时间：2016/10/20 13:17
    /// 备注：
    public class JWTHelper
    {
        public static TokenResult Create(object userId, string secretKey, int expiresDays)
        {
            TokenResult _result = new TokenResult();
            var payload = new Dictionary<string, object>()
            {
                { "userId", userId},
                { "claim", DateHelper.GetTimeStamp().ToString()  }
            };
            string _token = JsonWebToken.Encode(payload, secretKey, JwtHashAlgorithm.HS256);
            _result.access_token = _token;
            _result.expires_in = expiresDays * 24 * 3600;
            return _result;
        }
    }
}