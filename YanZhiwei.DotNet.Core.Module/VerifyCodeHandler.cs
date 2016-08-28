using System.Web;
using System.Web.SessionState;
using YanZhiwei.DotNet2.Utilities.ValidateCode;

namespace YanZhiwei.DotNet.Core.Module
{
    /// <summary>
    /// 验证码处理
    /// </summary>
    /// <seealso cref="System.Web.IHttpHandler" />
    /// <seealso cref="System.Web.SessionState.IRequiresSessionState" />
    public class VerifyCodeHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            string _validateCode = string.Empty;
            ValidateCode_Style1 _code = new ValidateCode_Style1();
            byte[] _buffer = _code.CreateImage(out _validateCode);
            context.Session["validateCode"] = _validateCode;
            context.Response.ClearContent();
            context.Response.ContentType = "image/Gif";
            context.Response.BinaryWrite(_buffer);
        }
    }
}