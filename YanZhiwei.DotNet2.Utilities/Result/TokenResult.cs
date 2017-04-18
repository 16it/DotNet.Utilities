namespace YanZhiwei.DotNet2.Utilities.Result
{
    /// <summary>
    /// 令牌结果
    /// </summary>
    /// 时间：2016/10/20 14:08
    /// 备注：
    public class TokenResult
    {
        #region Properties

        /// <summary>
        /// 令牌
        /// </summary>
        public string Access_token
        {
            get;
            set;
        }

        /// <summary>
        /// 签名有效时间【分钟】
        /// </summary>
        public int Expires_in
        {
            get;
            set;
        }

        #endregion Properties
    }
}