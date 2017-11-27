namespace YanZhiwei.DotNet.Core.WebApi
{
    using System;

    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.Encryptor;
    using YanZhiwei.DotNet2.Utilities.Result;

    /// <summary>
    /// WebApi 签名辅助类
    /// </summary>
    public sealed class SignatureHelper
    {
        #region Methods

        /// <summary>
        /// 生成签名字符串
        /// </summary>
        /// <param name="appSecret">签名加密键</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        public static string Create(string appSecret, string timestamp, string nonce)
        {
            string[] _array = { appSecret, timestamp, nonce };
            Array.Sort(_array);
            string _signatureString = string.Join("", _array);
            _signatureString = MD5Encryptor.Encrypt(_signatureString);
            return _signatureString;
        }

        /// <summary>
        /// 验证WebApi签名
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appSecret">签名加密键</param>
        /// <param name="signatureExpiredMinutes">签名过期分钟</param>
        /// <returns>CheckResult</returns>
        internal static CheckResult Validate(string signature, string timestamp, string nonce, string appSecret, int signatureExpiredMinutes)
        {
            string[] _arrayParamter = { appSecret, timestamp, nonce };
            Array.Sort(_arrayParamter);
            string _signatureString = string.Join("", _arrayParamter);
            _signatureString = MD5Encryptor.Encrypt(_signatureString);

            if (signature.CompareIgnoreCase(_signatureString) && CheckHelper.IsNumber(timestamp))
            {
                DateTime _timestampMillis =
                    UnixEpochHelper.DateTimeFromUnixTimestampMillis(timestamp.ToDoubleOrDefault(0f));
                double _minutes = DateTime.UtcNow.Subtract(_timestampMillis).TotalMinutes;

                if (_minutes > signatureExpiredMinutes)
                {
                    return CheckResult.Fail("签名时间戳失效");
                }
            }

            return CheckResult.Success();
        }

        #endregion Methods
    }
}