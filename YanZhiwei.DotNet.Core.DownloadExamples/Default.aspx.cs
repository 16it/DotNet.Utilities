using System;
using YanZhiwei.DotNet.Core.Download;

namespace YanZhiwei.DotNet.Core.DownloadExamples
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = DownloadFileHelper.Instance.EncryptFileName("AnkhSvn-2.5.12582.msi");
            link.NavigateUrl = "~/download.aspx?fileName=" + url;
        }
    }
}