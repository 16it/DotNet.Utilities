using System;
using System.Linq;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet.MVC.AdminPanel.DAL;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.MVC.AdminPanel.BLL
{
    public class AdminPanelService : IAdminPanelService
    {
        public User UserLogin(User item)
        {
            using (var dbContext = new AdminPanelDbContext())
            {
                Guid _password = MD5EncryptHelper.ToRandomMD5(item.Password);
                return dbContext.Users.Where(c =>
                string.Compare(c.AccountName, item.AccountName, true) == 0
                && c.Password == _password.ToString()).FirstOrDefault();
            }
        }
    }
}