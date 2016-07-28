using System.Web.Mvc;
using YanZhiwei.DotNet2.Utilities.ValidateCode;
using YanZhiwei.DotNet2.Utilities.WebForm.Core;

namespace YanZhiwei.DotNet.MVC.AdminPanel.UI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 生成登陆的验证码
        /// </summary>
        /// <returns>ActionResult</returns>
        /// 时间：2016/7/28 15:39
        /// 备注：
        public ActionResult GetValidatorGraphics()
        {
            string _vCodeValue = string.Empty;
            ValidateCodeType _validateCode = new ValidateCode_Style10();
            byte[] graphic = _validateCode.CreateImage(out _vCodeValue);
            CookieManger.Save("ValidatorCode", _vCodeValue);
            return File(graphic, @"image/jpeg");
        }
    }
}