using System;
using YanZhiwei.DotNet.Core.Download;

namespace YanZhiwei.DotNet.Core.DownloadExamples
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = DownloadFileHelper.Instance.EncryptFileName("DesignPattern.chm");
            link.NavigateUrl = "~/download.aspx?fileName=" + url;
        }
    }
}