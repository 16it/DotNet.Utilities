namespace YanZhiwei.DotNet2.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    using Encryptor;
    
    /// <summary>
    /// 加密处理帮助类
    /// </summary>
    /// 时间：2016/9/22 10:14
    /// 备注：
    public class EncryptorHelper
    {
        #region Methods
        
        /// <summary>
        /// 创建签名
        /// <para>使用http请求的queryString然后加上时间戳还有随机数来作为签名的参数。</para>
        /// </summary>
        /// <param name="queryString">请求参数</param>
        /// <param name="dateTime">时间戳</param>
        /// <param name="rumdon">随机数</param>
        /// <returns>签名参数字符串</returns>
        /// 时间：2016/9/22 10:16
        /// 备注：
        public static string CreateSign(SortedDictionary<string, string> queryString, string dateTime, string rumdon)
        {
            StringBuilder _builder = new StringBuilder();
            
            foreach(var keyValue in queryString)
            {
                _builder.AppendFormat("{0}={1}&", keyValue.Key, keyValue.Value);
            }
            
            if(_builder.Length > 1)
            {
                _builder.Remove(_builder.Length - 1, 1);
            }
            
            _builder.Append(dateTime);
            _builder.Append(rumdon);
            return _builder.ToString().ToUpper();
        }
        
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="requestSign">请求签名</param>
        /// <param name="signPlain">需要加密字符串</param>
        /// <param name="time">时间戳</param>
        /// <param name="secretKey">加密密钥</param>
        /// <returns>签名是否合法</returns>
        /// 时间：2016/9/22 10:18
        /// 备注：
        public static bool ValidSign(string requestSign, string signPlain, string time, string secretKey)
        {
            if(string.IsNullOrEmpty(time) || string.IsNullOrEmpty(requestSign) || string.IsNullOrEmpty(signPlain))
            {
                return false;
            }
            
            long _requestTime = 0;
            DateTime _curDateTime = DateTime.Now;
            
            if(long.TryParse(time, out _requestTime))
            {
                string _maxDateTime = _curDateTime.AddMinutes(5).ToString("yyyyMMddHHmmss");
                string _minDateTime = _curDateTime.AddMinutes(-5).ToString("yyyyMMddHHmmss");
                
                if(!(long.Parse(_maxDateTime) >= _requestTime && long.Parse(_minDateTime) <= _requestTime))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
            string _serverSign = SHA256Encryptor.Encrypt(secretKey, signPlain);
            return string.Compare(_serverSign, requestSign, true) == 0;
        }
        
        #endregion Methods
    }
}