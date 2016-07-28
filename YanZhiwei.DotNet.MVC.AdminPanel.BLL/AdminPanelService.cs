using System.Linq;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet.MVC.AdminPanel.DAL;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet.MVC.AdminPanel.BLL
{
    public class AdminPanelService : IAdminPanelService
    {
        public bool UserLogin(User item)
        {
            using (var dbContext = new AdminPanelDbContext())
            {
                item.Password = MD5EncryptHelper.ToRandomMD5(item.Password).ToString();
                return dbContext.Users.Count(c => 
                string.Compare(c.AccountName, item.AccountName, true) == 0 
                && string.Compare(c.Password, item.Password, true) == 0) > 0;
            }
        }
    }
}