using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YanZhiwei.DotNet2.Utilities.WebForm.Core;

namespace YanZhiwei.DotNet2.Utilities.WebFormExamples
{
    public partial class CookieMangerDemo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            NameValueCollection _aa = new NameValueCollection();
            _aa.Add("bb", "cc");
            _aa.Add("cc", "dd");
            CookieManger.AddValue("name", _aa);
        }
    }
}