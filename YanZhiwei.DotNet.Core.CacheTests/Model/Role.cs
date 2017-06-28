using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YanZhiwei.DotNet.Framework.Contract;

namespace YanZhiwei.DotNet.Core.CacheTests.Model
{
    [Table("Role")]
    public class Role : ModelBase
    {
        [Required(ErrorMessage = "角色名不能为空")]
        public string Name
        {
            get;
            set;
        }
        
        public string Info
        {
            get;
            set;
        }
        
        public virtual List<User> Users
        {
            get;
            set;
        }
        
        public string BusinessPermissionString
        {
            get;
            set;
        }
    }
}