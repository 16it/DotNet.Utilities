namespace YanZhiwei.DotNet.Core.Download
{
    using System;
    using System.Web;

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

        }

        #endregion Methods
    }
}