namespace YanZhiwei.DotNet.Framework.Contract
{
    using System;
    
    
    /// <summary>
    /// 实体类基类
    /// </summary>
    public class ModelBase
    {
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModelBase()
        {
            CreateTime = DateTime.Now;
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get;
            set;
        }
        
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int ID
        {
            get;
            set;
        }
        
        #endregion Properties
    }
}