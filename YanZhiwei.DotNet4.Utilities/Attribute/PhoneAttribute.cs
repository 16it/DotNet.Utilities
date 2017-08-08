using System.ComponentModel.DataAnnotations;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.Attribute
{
    ///<summary>
    /// 手机号码格式特性
    /// </summary>
    public class PhoneNumAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PhoneNumAttribute() : base(RegexPattern.MobilePhone)
        {
            ErrorMessage = "手机号码不正确";
        }
    }
}