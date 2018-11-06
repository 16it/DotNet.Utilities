namespace YanZhiwei.DotNet2.Utilities.Collection
{
    using Operator;

    /// <summary>
    /// 用于JSON传输的Json集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// 时间:2016/10/16 16:21
    /// 备注:
    public class JsonPagedList<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// 分页集合数据
        /// </summary>
        /// 时间:2016/10/16 16:20
        /// 备注:
        public readonly PagedList<T> PagedList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pagedList">PagedList</param>
        /// 时间:2016/10/16 16:20
        /// 备注:
        public JsonPagedList(PagedList<T> pagedList)
        {
            ValidateOperator.Begin().NotNull(pagedList, "分页数据集合");
            PagedList = pagedList;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int CurrentPageIndex => PagedList.CurrentPageIndex;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize => PagedList.PageSize;

        /// <summary>
        /// 记录总数
        /// </summary>
        public int TotalItemCount => PagedList.TotalItemCount;

        /// <summary>
        /// 页总数
        /// </summary>
        public int TotalPageCount => PagedList.TotalPageCount;

        #endregion Properties
    }
}