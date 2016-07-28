using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YanZhiwei.DotNet.Framework.Contract;

namespace YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model
{
    [Serializable]
    [Table("tbUser")]
    public class User : ModelBase
    {
        [MaxLength(50)]
        [Required(ErrorMessage = "Account Name is required")]
        [Description("帐户名")]
        public string AccountName { get; set; } // nvarchar(50), not null

        [MaxLength(50)]
        [Required(ErrorMessage = "Password is required")]
        [Description("帐户密码（32位MD5加密）")]
        public string Password { get; set; } // nvarchar(50), not null

        [MaxLength(50)]
        [Required(ErrorMessage = "Real Name is required")]
        public string RealName { get; set; } // nvarchar(50), not null

        [MaxLength(50)]
        [Description("联系人手机号码")]
        public string MobilePhone { get; set; } // nvarchar(50), null

        [MaxLength(50)]
        [Description("联系的邮箱")]
        public string Email { get; set; } // nvarchar(50), null

        public bool? IsAble { get; set; } // bit, null

        public bool? IfChangePwd { get; set; } // bit, null

        [MaxLength]
        [Description("介绍描述")]
        public string Description { get; set; } // nvarchar(max), null

        [MaxLength(50)]
        public string CreateBy { get; set; } // nvarchar(50), null

        [MaxLength(50)]
        public string UpdateBy { get; set; } // nvarchar(50), null

        [Description("修改时间")]
        public DateTime? UpdateTime { get; set; } // datetime, null
    }
}