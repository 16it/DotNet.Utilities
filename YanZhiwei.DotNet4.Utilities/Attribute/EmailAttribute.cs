using System.ComponentModel.DataAnnotations;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.Attribute
{
    ///<summary>
    /// 邮箱验证格式特性
    /// </summary>
    public class EmailAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EmailAttribute() : base(RegexPattern.EmailCheck)
        {
            ErrorMessage = "邮箱格式不正确";
        }
    }
}