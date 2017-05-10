namespace YanZhiwei.DotNet.Core.Download
{
    using System.Web;
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

        /// <summary>
        /// 处理文件下载入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string _downloadEncryptFileName = context.Request["fileName"];
            string _downloadFileName = DownloadFileHelper.Instance.DecryptFileName(_downloadEncryptFileName);
            string _filePath = HttpContext.Current.Server.MapPath("~/") + "files/" + _downloadFileName;
            WebDownloadFile.FileDownload(_downloadFileName, _filePath, 10240);
        }

        #endregion Methods
    }
}