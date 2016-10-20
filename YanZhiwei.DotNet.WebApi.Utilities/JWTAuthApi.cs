using JWT;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using YanZhiwei.DotNet.WebApi.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Encryptor;

namespace YanZhiwei.DotNet.WebApi.Utilities
{
    /// <summary>
    /// 采用JWT生成令牌WEB API验证类
    /// </summary>
    /// 时间：2016/10/20 14:11
    /// 备注：
    public class JWTAuthApi
    {
        /// <summary>
        /// WEB API验证配置项目
        /// </summary>
        /// 时间：2016/10/20 14:38
        /// 备注：
        internal readonly WrapAuthApiConfigItem configItem;
        
        private readonly string SharedKey;
        
        internal JWTAuthApi(WrapAuthApiConfigItem authApiConfigItem, string sharedKey)
        {
            configItem = authApiConfigItem;
            SharedKey = sharedKey;
        }
        
        /// <summary>
        /// 注册用户获取访问令牌接口
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appid">应用接入ID</param>
        /// <param name="appSecret">应用接入ID对应Key</param>
        /// <returns>
        /// 令牌
        /// </returns>
        /// 时间：2016/10/20 13:35
        /// 备注：
        public TokenResult GetAccessToken(string userId, string signature, string timestamp, string nonce, string appid, string appSecret)
        {
            TokenResult _result = new TokenResult();
            Tuple<bool, string> _checkedResult = ValidateSignature(signature, timestamp, nonce, appid, appSecret);
            
            if(_checkedResult.Item1)
            {
                Dictionary<string, object> _payload = new Dictionary<string, object>()
                {
                    { "userId", userId},
                    { "claim", UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds}
                };
                string _token = JsonWebToken.Encode(_payload, SharedKey, JwtHashAlgorithm.HS256);
                _result.Access_token = _token;
                _result.Expires_in = configItem.TimspanExpiredMinutes * 24 * 3600;
            }
            
            return _result;
        }
        
        /// <summary>
        /// 检查用户令牌
        /// </summary>
        /// <param name="token">用户令牌</param>
        /// <returns></returns>
        /// 时间：2016/10/20 15:30
        /// 备注：
        /// <exception cref="System.ArgumentException">用户令牌失效.</exception>
        public Tuple<bool, string> ValidateToken(string token)
        {
            //返回的结果对象
            Tuple<bool, string> _checkeResult = new Tuple<bool, string>(false, "数据完整性检查不通过");
            
            if(!string.IsNullOrEmpty(token))
            {
                string _decodedJwt = JsonWebToken.Decode(token, SharedKey);
                
                if(!string.IsNullOrEmpty(_decodedJwt))
                {
                    dynamic _root = JObject.Parse(_decodedJwt);
                    string _userid = _root.userId;
                    int _jwtcreated = (int)_root.claim;
                    
                    if(UnixEpochHelper.GetCurrentUnixTimestamp().TotalDays - _jwtcreated > configItem.TokenExpiredDays)
                    {
                        _checkeResult = new Tuple<bool, string>(false, "用户令牌失效.");
                    }
                    
                    _checkeResult = new Tuple<bool, string>(true, _userid);
                }
            }
            
            return _checkeResult;
        }
        
        private Tuple<bool, string> ValidateSignature(string signature, string timestamp, string nonce, string appid, string appSecret)
        {
            Tuple<bool, string> _checkeResult = new Tuple<bool, string>(false, "数据完整性检查不通过");
            string[] _arrayParamter = { appSecret, timestamp, nonce };
            Array.Sort(_arrayParamter);
            string _signatureString = string.Join("", _arrayParamter);
            _signatureString = MD5Encryptor.Encrypt(_signatureString);
            
            if(signature.CompareIgnoreCase(signature) && CheckHelper.IsNumber(timestamp))
            {
                DateTime _timestampMillis = UnixEpochHelper.DateTimeFromUnixTimestampMillis(timestamp.ToInt32OrDefault(0));
                double _minutes = DateTime.Now.Subtract(_timestampMillis).TotalMinutes;
                
                if(_minutes > configItem.TimspanExpiredMinutes)
                {
                    _checkeResult = new Tuple<bool, string>(false, "签名时间戳失效");
                }
                else
                {
                    _checkeResult = new Tuple<bool, string>(true, string.Empty);
                }
            }
            
            return _checkeResult;
        }
    }
}