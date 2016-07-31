using System;
using System.Globalization;
using System.Web.Mvc;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet.MVC.AdminPanel.UI.App_Start;

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