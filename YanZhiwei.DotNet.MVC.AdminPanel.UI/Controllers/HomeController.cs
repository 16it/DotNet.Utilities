using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet.MVC.AdminPanel.UI.App_Start;
using YanZhiwei.DotNet.MVC.AdminPanel.UI.Models;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.MVC.AdminPanel.UI.Controllers
{
    [Login]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            User _curUser = ViewData["Account"] as User;
            if (_curUser == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.RealName = _curUser.RealName;
            ViewBag.TimeView = DateTime.Now.ToLongDateString();
            ViewBag.DayDate = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            return View();
        }

        public JsonResult GetTreeByEasyui(int id)
        {
            User _curUser = ViewData["Account"] as User;
            if (_curUser != null)
            {
                IEnumerable<UserMenu> _userMenuList = ServiceContext.Current.AdminPanelService.GetMenuByUserId(_curUser.ID);
                if (_userMenuList != null)
                {
                    _userMenuList = _userMenuList.Where(c => c.menuparentid == id).ToList();
                    List<SysModuleNavModel> _sysModulNavList = new List<SysModuleNavModel>();
                    foreach (UserMenu item in _userMenuList)
                    {
                        SysModuleNavModel _sysMenu = new SysModuleNavModel();
                        _sysMenu.id = item.menuid.ToStringOrDefault(string.Empty);
                        _sysMenu.text = item.menuname.ToStringOrDefault(string.Empty).Trim();
                        _sysMenu.attributes = item.linkaddress.ToStringOrDefault(string.Empty).Trim();
                        _sysMenu.iconCls = item.icon.Trim();
                        _sysMenu.state = item.menuparentid == 0 ? "closed" : "open";
                        _sysModulNavList.Add(_sysMenu);
                    }
                    return Json(_sysModulNavList);
                }
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}