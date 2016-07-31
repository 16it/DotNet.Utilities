using System;
using System.Web.Mvc;
using YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.ValidateCode;
using YanZhiwei.DotNet2.Utilities.WebForm.Core;

namespace YanZhiwei.DotNet.MVC.AdminPanel.UI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            //
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(User item)
        {
            try
            {
                User _finded = ServiceContext.Current.AdminPanelService.UserLogin(item);
                if (_finded != null)
                {
                    if (!_finded.IsAble.Value)
                    {
                        return Content("用户已被禁用，请您联系管理员");
                    }
                    else
                    {
                        CookieManger.Save("UserID",_finded.ID.ToString());

                        return Content("OK");
                    }
                }
                else
                {
                    return Content("用户名称或者密码错误！");
                }
            }
            catch (Exception ex)
            {
                return Content("登录异常," + ex.Message);
            }
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