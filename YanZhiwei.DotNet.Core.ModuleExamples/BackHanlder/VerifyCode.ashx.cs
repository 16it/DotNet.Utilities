using System.Web;
using YanZhiwei.DotNet.Core.Module;

namespace YanZhiwei.DotNet.Core.ModuleExamples.BackHanlder
{
    /// <summary>
    /// VerifyCode 的摘要说明
    /// </summary>
    public class VerifyCode : VerifyCodeHandler
    {
        public override void OnValidateCodeCreated(HttpContext context, string validateCode)
        {
            context.Session["validateCode"] = validateCode;
        }
    }
}