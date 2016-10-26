namespace YanZhiwei.DotNet2.Utilities.Model
{
    using Enum;
    
    /// <summary>
    /// 表示Ajax操作结果
    /// </summary>
    public class AjaxResult
    {
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="type">Ajax操作结果类型</param>
        /// <param name="data">返回数据</param>
        public AjaxResult(string content, AjaxResultType type, object data)
        : this(content, data, type)
        {
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">消息内容</param>
        public AjaxResult(string content)
        : this(content, AjaxResultType.Info, null)
        {
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="data">返回数据</param>
        /// <param name="type">Ajax操作结果类型</param>
        public AjaxResult(string content, object data, AjaxResultType type)
        {
            Type = type.ToString();
            Content = content;
            Data = data;
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string Content
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public object Data
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 获取 Ajax操作结果类型
        /// </summary>
        public string Type
        {
            get;
            private set;
        }
        
        #endregion Properties
    }
}