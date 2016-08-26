using System.Web.Mvc;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet2.Utilities.WebForm.Core;

namespace YanZhiwei.DotNet.MVC.AdminPanel.UI.App_Start
{
    public class LoginAttribute : ActionFilterAttribute
    {
        public User CurrentUser = null;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string _userId = CookieManger.GetValue("UserID");
            //if(!string.IsNullOrEmpty(_userId))
            //{
            //    User _finded = ServiceContext.Current.AdminPanelService.GetUserById(_userId.ToInt32OrDefault(-1));
            //    if(_finded != null)
            //    {
            //        CurrentUser = _finded;
            //        filterContext.Controller.ViewData["Account"] = _finded;
            //        filterContext.Controller.ViewData["AccountName"] = _finded.AccountName;
            //        filterContext.Controller.ViewData["RealName"] = _finded.RealName;
            //        base.OnActionExecuting(filterContext);
            //    }
            //}
            //else
            //{
            //    filterContext.Result = new RedirectResult("/Login/Index");
            //}
        }
    }
}