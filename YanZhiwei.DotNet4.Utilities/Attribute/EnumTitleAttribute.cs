namespace YanZhiwei.DotNet4.Utilities.Attribute
{
    /// <summary>
    /// 枚举标题特性
    /// </summary>
    /// 时间：2016/9/8 13:26
    /// 备注：
    public class EnumTitleAttribute : System.Attribute
    {
        #region Fields

        private bool isDisplay = true;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="synonyms">近义词</param>
        public EnumTitleAttribute(string title, params string[] synonyms)
        {
            Title = title;
            Synonyms = synonyms;
            Order = int.MaxValue;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 分类
        /// </summary>
        public int Category
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplay
        {
            get
            {
                return isDisplay;
            }
            set
            {
                isDisplay = value;
            }
        }

        /// <summary>
        /// 字母
        /// </summary>
        public string Letter
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// 近义词
        /// </summary>
        public string[] Synonyms
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        #endregion Properties
    }
}