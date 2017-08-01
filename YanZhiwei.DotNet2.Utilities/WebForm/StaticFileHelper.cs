namespace YanZhiwei.DotNet2.Utilities.WebForm
{
    using System;
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    /// 静态文件帮助类
    /// </summary>
    public class StaticFileHelper
    {
        #region Methods

        /// <summary>
        /// 获取虚拟路径映射的服务器物理物理
        /// </summary>
        /// <param name="path">虚拟路径</param>
        /// <returns>物理路径</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }
            string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(_baseDirectory, path);
        }

        #endregion Methods
    }
}