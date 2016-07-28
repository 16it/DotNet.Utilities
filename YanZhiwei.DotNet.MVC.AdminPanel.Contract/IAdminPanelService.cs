using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;

namespace YanZhiwei.DotNet.MVC.AdminPanel.Contract
{
    public interface IAdminPanelService
    {
        bool UserLogin(User item);
    }
}