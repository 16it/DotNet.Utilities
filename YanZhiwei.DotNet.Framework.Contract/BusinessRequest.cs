namespace YanZhiwei.DotNet.Framework.Contract
{
    /// <summary>
    /// 业务请求对象
    /// </summary>
    /// 时间：2016/9/9 14:50
    /// 备注：
    public class BusinessRequest : ModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessRequest()
        {
            PageSize = 5;
        }

        /// <summary>
        /// Sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public int Top
        {
            set
            {
                this.PageSize = value;
                this.PageIndex = 1;
            }
        }
        
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }
        
        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
    }
}