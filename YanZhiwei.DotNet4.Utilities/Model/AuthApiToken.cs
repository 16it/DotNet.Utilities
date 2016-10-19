using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YanZhiwei.DotNet4.Utilities.Model
{
    public class AuthApiToken
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public int StaffId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 用户名对应签名Token
        /// </summary>
        public Guid SignToken
        {
            get;
            set;
        }
        
        
        /// <summary>
        /// Token过期时间
        /// </summary>
        public DateTime ExpireTime
        {
            get;
            set;
        }
    }
}
