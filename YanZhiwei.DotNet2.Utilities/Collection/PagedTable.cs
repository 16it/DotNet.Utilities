namespace YanZhiwei.DotNet2.Utilities.Collection
{
    using System;
    using System.Data;
    
    using Interfaces;
    
    /// <summary>
    /// 分页的DataTable
    /// </summary>
    /// 时间：2016/10/27 11:39
    /// 备注：
    public class PagedTable : IPagedList
    {
        #region Fields
        
        /// <summary>
        /// 已经分页的DataTable
        /// </summary>
        /// 时间：2016/10/27 11:33
        /// 备注：
        public readonly DataTable PagedDataTable = null;
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table">已分页的DataTable</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalItemCount">总数据行数</param>
        /// 时间：2016/10/27 11:39
        /// 备注：
        public PagedTable(DataTable table, int pageIndex, int pageSize, int totalItemCount)
        {
            PagedDataTable = table;
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }
        
        #endregion Constructors
        
        #region Properties
        
        /// <summary>
        /// 当前页索引
        /// </summary>
        /// 时间：2016/10/27 11:39
        /// 备注：
        public int CurrentPageIndex
        {
            get;
            set;
        }
        
        /// <summary>
        /// 分页大小
        /// </summary>
        /// 时间：2016/10/27 11:39
        /// 备注：
        public int PageSize
        {
            get;
            set;
        }
        
        /// <summary>
        /// 记录总数
        /// </summary>
        /// 时间：2016/10/27 11:39
        /// 备注：
        public int TotalItemCount
        {
            get;
            set;
        }
        
        /// <summary>
        /// 页总数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            }
        }
        
        #endregion Properties
    }
}