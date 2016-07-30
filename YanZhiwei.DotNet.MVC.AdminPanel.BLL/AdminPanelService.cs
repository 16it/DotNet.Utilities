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
                string _password = "d44fcf2c65c1e3b75878cd4e2c913d77";// MD5EncryptHelper.ToRandomMD5(item.Password);
                return dbContext.Users.Where(c =>
                string.Compare(c.AccountName, item.AccountName, true) == 0
                && string.Compare(c.Password, _password, true) == 0).FirstOrDefault();
            }
        }
    }
}