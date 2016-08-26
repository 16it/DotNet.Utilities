using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.Service;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract;

namespace YanZhiwei.DotNet.MVC.AdminPanel
{
    public class ServiceContext
    {
        public static ServiceContext Current
        {
            get
            {
                return CacheHelper.GetItem<ServiceContext>("ServiceContext", () => new ServiceContext());
            }
        }

        //public IAdminPanelService AdminPanelService
        //{
        //    get
        //    {
        //        ServiceHelper _helper = new ServiceHelper();
        //        return _helper.CreateService<IAdminPanelService>();
        //    }
        //}
    }
}