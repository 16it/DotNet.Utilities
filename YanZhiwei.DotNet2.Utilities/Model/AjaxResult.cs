namespace YanZhiwei.DotNet2.Utilities.Model
{
    /// <summary>
    /// 表示Ajax操作结果
    /// </summary>
    public class AjaxResult<T>
    {
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="data">返回数据</param>
        /// <param name="state">操作结果</param>
        private AjaxResult(string message, T data, bool state)
        {
            State = state;
            Message = message;
            Data = data;
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string Message
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public T Data
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 获取 Ajax操作结果类型
        /// </summary>
        public bool State
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 失败类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Fail(string content, T data)
        {
            AjaxResult<T> _infoAjaxResult = new AjaxResult<T>(content, data, false);
            return _infoAjaxResult;
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
        /// 成功类型
        /// </summary>
        /// <param name="content">消息</param>
        /// <param name="data">内容</param>
        /// <returns>AjaxResult</returns>
        public static AjaxResult<T> Success(string content, T data)
        {
            AjaxResult<T> _infoAjaxResult = new AjaxResult<T>(content, data, true);
            return _infoAjaxResult;
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
        
        #endregion Properties
    }
}