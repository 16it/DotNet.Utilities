namespace YanZhiwei.DotNet.Core.Download
{
    using System.Web;
    using YanZhiwei.DotNet2.Utilities.Result;
    using YanZhiwei.DotNet3._5.Utilities.WebForm.Core;
    
    /// <summary>
    /// 处理文件下载
    /// </summary>
    public abstract class DownloadHandler : IHttpHandler
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
        /// 处理下载结果抽象方法
        /// </summary>
        /// <param name="fileName">本地文件名称</param>
        /// <param name="filePath">下载文件路径</param>
        /// <param name="err">错误信息</param>
        /// <returns>响应字符串</returns>
        public abstract string GetResult(string fileName, string filePath, string err);
        
        /// <summary>
        ///下载文件完成抽象方法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">本地文件名称</param>
        /// <param name="filePath">下载文件路径</param>
        public abstract void OnDownloaded(HttpContext context, string fileName, string filePath);
        
        /// <summary>
        /// 处理文件下载入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string _downloadEncryptFileName = context.Request["fileName"];
            string _downloadFileName = DownloadFileHelper.Instance.DecryptFileName(_downloadEncryptFileName);
            string _filePath = DownloadConfigContext.DownLoadMainDirectory + _downloadFileName;//HttpContext.Current.Server.MapPath("~/") + "files/" + _downloadFileName;
            FileDownloadResult _result = WebDownloadFile.FileDownload(_downloadFileName, _filePath, DownloadConfigContext.LimitDownloadSpeedKb * 1024);
            
            if(_result.State)
            {
                OnDownloaded(context, _downloadFileName, _filePath);
            }
            
            context.Response.Write(GetResult(_downloadFileName, _filePath, _result.Message));
            context.Response.End();
        }
        
        #endregion Methods
    }
}