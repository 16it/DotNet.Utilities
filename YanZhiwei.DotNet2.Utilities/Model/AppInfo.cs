namespace YanZhiwei.DotNet2.Utilities.Model
{
    /// <summary>
    /// APP 信息
    /// </summary>
    public sealed class AppInfo
    {
        #region Properties

        /// <summary>
        /// 辨识ID
        /// </summary>
        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// APP密钥
        /// </summary>
        public string AppSecret
        {
            get;
            set;
        }

        /// <summary>
        /// sharedKey
        /// </summary>
        public string SharedKey
        {
            get; set;
        }

        #endregion Properties
    }
}