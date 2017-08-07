using System.ComponentModel.DataAnnotations;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.Attribute
{
    ///<summary>
    /// 中文约束Attribute
    /// </summary>
    public class ChineseAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChineseAttribute()
            : base(RegexPattern.ChineseCheck)
        {
            ErrorMessage = "请输入中文";
        }
    }
}