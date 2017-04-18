namespace YanZhiwei.DotNet2.Utilities.Result
{
    /// <summary>
    /// 检查结果
    /// </summary>
    public sealed class CheckResult : BasicResult<string>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="state">检查状态</param>
        public CheckResult(string message, bool state)
            : base(message, null)
        {
            State = state;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 检查状态
        /// </summary>
        public bool State
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="message">检查附加错误信息</param>
        /// <returns>CheckResult</returns>
        public static CheckResult Fail(string message)
        {
            CheckResult _item = new CheckResult(message, false);
            return _item;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message">成功附加信息</param>
        /// <returns>CheckResult</returns>
        public static CheckResult Success(string message)
        {
            CheckResult _item = new CheckResult(message, true);
            return _item;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns>CheckResult</returns>
        public static CheckResult Success()
        {
            return Success(null);
        }

        #endregion Methods
    }
}