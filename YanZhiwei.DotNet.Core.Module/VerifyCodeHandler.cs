namespace YanZhiwei.DotNet.Core.Module
{
    using System.Web;
    using System.Web.SessionState;

    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.Model;
    using YanZhiwei.DotNet2.Utilities.ValidateCode;

    /// <summary>
    /// 验证码处理
    /// </summary>
    /// <seealso cref="System.Web.IHttpHandler" />
    /// <seealso cref="System.Web.SessionState.IRequiresSessionState" />
    public abstract class VerifyCodeHandler : IHttpHandler, IRequiresSessionState
    {
        #region Properties

        /// <summary>
        /// IsReusable
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 生成验证码完成方法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="validateCode">生成的验证码</param>
        public abstract void OnValidateCodeCreated(HttpContext context, string validateCode);

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void ProcessRequest(HttpContext context)
        {
            string _validateCode = string.Empty;
            string _validateType = context.Request.Params["style"];
            ValidateCodeType _createCode = null;

            if (string.IsNullOrEmpty(_validateType))
            {
                _createCode = new ValidateCode_Style1();
            }
            else
            {
                if (_validateType.CompareIgnoreCase("type1"))
                    _createCode = new ValidateCode_Style1();
                else if (_validateType.CompareIgnoreCase("type2"))
                    _createCode = new ValidateCode_Style2();
                else if (_validateType.CompareIgnoreCase("type3"))
                    _createCode = new ValidateCode_Style3();
                else if (_validateType.CompareIgnoreCase("type4"))
                    _createCode = new ValidateCode_Style4();
                else if (_validateType.CompareIgnoreCase("type5"))
                    _createCode = new ValidateCode_Style5();
                else if (_validateType.CompareIgnoreCase("type6"))
                    _createCode = new ValidateCode_Style6();
                else if (_validateType.CompareIgnoreCase("type7"))
                    _createCode = new ValidateCode_Style7();
                else if (_validateType.CompareIgnoreCase("type8"))
                    _createCode = new ValidateCode_Style8();
                else if (_validateType.CompareIgnoreCase("type9"))
                    _createCode = new ValidateCode_Style9();
                else if (_validateType.CompareIgnoreCase("type10"))
                    _createCode = new ValidateCode_Style10();
                else if (_validateType.CompareIgnoreCase("type11"))
                    _createCode = new ValidateCode_Style11();
                else if (_validateType.CompareIgnoreCase("type12"))
                    _createCode = new ValidateCode_Style12();
                else if (_validateType.CompareIgnoreCase("type13"))
                    _createCode = new ValidateCode_Style13();
                else if (_validateType.CompareIgnoreCase("type14"))
                    _createCode = new ValidateCode_Style14();
                else
                    _createCode = new ValidateCode_Style1();
            }

            byte[] _buffer = _createCode.CreateImage(out _validateCode);
            OnValidateCodeCreated(context, _validateCode);
            //  context.Session["validateCode"] = _validateCode;
            context.Response.ClearContent();
            context.Response.ContentType = ResponseContentType.Gif;
            context.Response.BinaryWrite(_buffer);
        }

        #endregion Methods
    }
}