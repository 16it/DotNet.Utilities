using System.ComponentModel.DataAnnotations;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.Attribute
{
    ///<summary>
    /// 身份证格式特性
    /// </summary>
    public class IdCardAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IdCardAttribute() : base(RegexPattern.IdCardCheck)
        {
            ErrorMessage = "身份证格式不正确";
        }
    }
}