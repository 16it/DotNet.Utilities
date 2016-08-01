using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet.MVC.AdminPanel.DAL;

namespace YanZhiwei.DotNet.MVC.AdminPanel.BLL
{
    public class AdminPanelService : IAdminPanelService
    {
        public IEnumerable<UserMenu> GetMenuByUserId(int id)
        {
            using (var dbContext = new AdminPanelDbContext())
            {
                string _sql = @"SELECT DISTINCT
                                (m.Name) menuname,
                                m.Id menuid,
                                m.Icon icon,
                                u.Id userid,
                                u.AccountName username,
                                m.ParentId menuparentid,
                                m.Sort menusort,
                                m.LinkAddress linkaddress
                                FROM tbUser u
                                JOIN tbUserRole ur ON u.Id = ur.UserId
                                JOIN tbRoleMenuButton rmb ON ur.RoleId = rmb.RoleId
                                JOIN tbMenu m ON rmb.MenuId = m.Id
                                WHERE u.Id = @Id
                                ORDER BY m.ParentId,
                                m.Sort; ";
                DbParameter[] _paramter = new DbParameter[1];
                _paramter[0] = new SqlParameter("@Id", id);
                return dbContext.SqlQuery<UserMenu>(_sql, _paramter).ToList();
            }
        }

        public User GetUserById(int id)
        {
            using (var dbContext = new AdminPanelDbContext())
            {
                return dbContext.Users.Where(c => c.ID == id).FirstOrDefault();
            }
        }

        public User UserLogin(User item)
        {
            using (var dbContext = new AdminPanelDbContext())
            {
                string _password = "21232F297A57A5A743894A0E4A801F";// MD5EncryptHelper.ToRandomMD5(item.Password);
                return dbContext.Users.Where(c =>
                string.Compare(c.AccountName, item.AccountName, true) == 0
                && string.Compare(c.Password, _password, true) == 0).FirstOrDefault();
            }
        }
    }
}