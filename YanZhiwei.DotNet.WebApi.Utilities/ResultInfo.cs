namespace YanZhiwei.DotNet.WebApi.Utilities
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// 服务基础返回模型
    /// </summary>
    [DataContract(Name = "xml")]
    public class ResultInfo
    {
        #region Properties
        
        /// <summary>
        /// 执行服务时返回消息
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool State
        {
            get;
            set;
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>失败
        /// </summary>
        /// <param name="message">定义失败信息</param>
        /// <returns>ResultInfo</returns>
        public static ResultInfo Fail(string message)
        {
            return new ResultInfo
            {
                State = false,
                Message = message
            };
        }
        
        /// <summary>成功
        /// </summary>
        /// <param name="msg">可定义成功信息</param>
        /// <returns></returns>
        public static ResultInfo Success(string msg)
        {
            return new ResultInfo
            {
                State = true,
                Message = msg
            };
        }
        
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static ResultInfo Success()
        {
            return Success(string.Empty);
        }
        
        #endregion Methods
    }
    
    /// <summary>服务返回基础模型
    /// </summary>
    [DataContract(Name = "xml")]
    public class ResultInfo<T> : ResultInfo
    {
        #region Constructors
        
        
        private ResultInfo()
        {
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>数据
        /// </summary>
        [DataMember]
        public T Data
        {
            get;
            set;
        }
        
        #endregion Properties
        
        #region Methods
        
        /// <summary>失败
        /// </summary>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static new ResultInfo<T> Fail(string msg)
        {
            return new ResultInfo<T>
            {
                State = false,
                Message = msg
            };
        }
        
        /// <summary>失败
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static ResultInfo<T> Fail(T data, string msg)
        {
            return new ResultInfo<T>
            {
                State = false,
                Message = msg,
                Data = data
            };
        }
        
        /// <summary>成功
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="msg">可定义成功信息</param>
        /// <returns></returns>
        public static ResultInfo<T> Success(T data, string msg)
        {
            return new ResultInfo<T>
            {
                State = true,
                Message = msg,
                Data = data
            };
        }
        
        /// <summary>成功
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <returns></returns>
        public static ResultInfo<T> Success(T data)
        {
            return Success(data, string.Empty);
        }
        
        #endregion Methods
    }
}