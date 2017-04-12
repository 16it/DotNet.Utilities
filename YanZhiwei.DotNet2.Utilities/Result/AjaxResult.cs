namespace YanZhiwei.DotNet2.Utilities.Result
{
    using YanZhiwei.DotNet2.Utilities.Enum;

    /// <summary>
    /// 表示Ajax操作结果
    /// </summary>
    public sealed class AjaxResult<T> : BasicResult<T>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="data">返回数据</param>
        /// <param name="resultType">Ajax操作结果类型</param>
        public AjaxResult(string message, T data, AjaxResultType resultType)
            : base(message, data)
        {
            ResultType = resultType;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 获取 Ajax操作结果类型
        /// </summary>
        public AjaxResultType ResultType
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 错误异常类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Error(string content, T data)
        {
            AjaxResult<T> _errorAjaxResult = new AjaxResult<T>(content, data, AjaxResultType.Error);
            return _errorAjaxResult;
        }

        /// <summary>
        /// 错误异常类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Error(string content)
        {
            return Error(content, default(T));
        }

        /// <summary>
        /// 失败类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Fail(string content, T data)
        {
            AjaxResult<T> _failAjaxResult = new AjaxResult<T>(content, data, AjaxResultType.Fail);
            return _failAjaxResult;
        }

        /// <summary>
        /// 失败类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Fail(string content)
        {
            return Fail(content, default(T));
        }

        /// <summary>
        /// 通知类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Info(string content, T data)
        {
            AjaxResult<T> _infoAjaxResult = new AjaxResult<T>(content, data, AjaxResultType.Info);
            return _infoAjaxResult;
        }

        /// <summary>
        /// 通知类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Info(string content)
        {
            return Info(content, default(T));
        }

        /// <summary>
        /// 成功类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Success(string content, T data)
        {
            AjaxResult<T> _successAjaxResult = new AjaxResult<T>(content, data, AjaxResultType.Success);
            return _successAjaxResult;
        }

        /// <summary>
        /// 成功类型
        /// </summary>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Success(T data)
        {
            return Success(string.Empty, data);
        }

        /// <summary>
        /// 成功类型
        /// </summary>
        /// <returns></returns>
        public static AjaxResult<T> Success()
        {
            return Success(string.Empty, default(T));
        }

        /// <summary>
        /// 警告类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Warning(string content, T data)
        {
            AjaxResult<T> _warningAjaxResult = new AjaxResult<T>(content, data, AjaxResultType.Warning);
            return _warningAjaxResult;
        }

        /// <summary>
        /// 警告类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Warning(string content)
        {
            return Warning(content, default(T));
        }

        #endregion Methods
    }
}