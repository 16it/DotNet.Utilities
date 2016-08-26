namespace YanZhiwei.DotNet2.Utilities.Exception
{
    using System;

    /// <summary>
    /// 框架异常，用于在后端抛出到前端做相应处理
    /// </summary>
    [Serializable]
    public class FrameworkException : Exception
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrameworkException()
        : this(string.Empty)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        public FrameworkException(string message)
        : this("error", message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="inner">内部异常</param>
        public FrameworkException(string message, Exception inner)
        : base(message, inner)
        {
        }

        /// <summary>
        /// 构造消息
        /// </summary>
        /// <param name="name">错误名称</param>
        /// <param name="message">错误消息</param>
        public FrameworkException(string name, string message)
        : base(message)
        {
            this.Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">枚举</param>
        public FrameworkException(string message, Enum errorCode)
        : this("error", message, errorCode)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">错误名称</param>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">枚举</param>
        public FrameworkException(string name, string message, Enum errorCode)
        : base(message)
        {
            this.Name = name;
            this.ErrorCode = errorCode;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 错误代码枚举
        /// </summary>
        public Enum ErrorCode
        {
            get;
            set;
        }

        /// <summary>
        /// 错误名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        #endregion Properties
    }
}