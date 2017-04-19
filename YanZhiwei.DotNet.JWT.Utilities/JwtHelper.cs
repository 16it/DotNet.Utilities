using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System.Collections.Generic;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet.JWT.Utilities
{
    /// <summary>
    /// JSON Web Token辅助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="payload">负载数据</param>
        /// <returns>Token令牌</returns>
        public static string CreateTokens(string secret, Dictionary<string, object> payload)
        {
            ValidateOperator.Begin().NotNull(payload, "负载数据").NotNullOrEmpty(secret, "密钥");
            IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
            IJsonSerializer _serializer = new JsonNetSerializer();
            IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder _encoder = new JwtEncoder(_algorithm, _serializer, _urlEncoder);
            return _encoder.Encode(payload, secret);
        }
        
        /// <summary>
        /// 转换Token
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="secret">密钥</param>
        /// <returns>Token以及负载数据</returns>
        public static string ParseTokens(string token, string secret)
        {
            ValidateOperator.Begin().NotNullOrEmpty(token, "令牌").NotNullOrEmpty(secret, "密钥");
            IJsonSerializer _serializer = new JsonNetSerializer();
            IDateTimeProvider _provider = new UtcDateTimeProvider();
            IJwtValidator _validator = new JwtValidator(_serializer, _provider);
            IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder _decoder = new JwtDecoder(_serializer, _validator, _urlEncoder);
            return _decoder.Decode(token, secret, true);
        }
    }
}