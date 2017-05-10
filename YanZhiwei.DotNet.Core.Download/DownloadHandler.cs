namespace YanZhiwei.DotNet.Core.Download
{
    using System.Web;
    using YanZhiwei.DotNet2.Utilities.DesignPattern;
    using YanZhiwei.DotNet3._5.Utilities.WebForm.Core;

    public class DownloadHandler : IHttpHandler
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

        public void ProcessRequest(HttpContext context)
        {
            string _downloadFileName = context.Request["fileName"];
            string _filePath = HttpContext.Current.Server.MapPath("~/") + "files/" + _downloadFileName;
            string _downloadEncryptFileName = Singleton<DownloadFileHelper>.GetInstance().EncryptFileName(_downloadFileName);
            WebDownloadFile.FileDownload(_downloadEncryptFileName, _filePath, 1024);
        }

        #endregion Methods
    }
}