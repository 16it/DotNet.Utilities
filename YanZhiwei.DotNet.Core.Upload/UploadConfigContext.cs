using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using YanZhiwei.DotNet.Core.Config;
using YanZhiwei.DotNet.Core.Config.Model;
using YanZhiwei.DotNet2.Utilities.WebForm;

namespace YanZhiwei.DotNet.Core.Upload
{
    /// <summary>
    /// 文件上传配置参数
    /// </summary>
    public class UploadConfigContext
    {
        private static readonly object syncObject = new object();

        /// <summary>
        /// 文件上传配置
        /// </summary>
        public static UploadConfig UploadConfig = CachedConfigContext.Instance.UploadConfig;

        static UploadConfigContext()
        {
        }

        private static string uploadPath;

        /// <summary>
        /// 上传路径
        /// </summary>
        public static string UploadPath
        {
            get
            {
                if (uploadPath == null)
                {
                    lock (syncObject)
                    {
                        if (uploadPath == null)
                        {
                            uploadPath = CachedConfigContext.Instance.UploadConfig.UploadPath ?? string.Empty;

                            if (HttpContext.Current != null)
                            {
                                bool _isLocal = FetchHelper.ServerDomain.IndexOf("localhost", StringComparison.OrdinalIgnoreCase) < 0;

                                if (_isLocal || string.IsNullOrEmpty(UploadConfig.UploadPath) || !Directory.Exists(UploadConfig.UploadPath))
                                    uploadPath = HttpContext.Current.Server.MapPath("~/" + "Upload");
                            }
                        }
                    }
                }

                return uploadPath;
            }
        }

        private static Dictionary<string, ThumbnailSize> thumbnailConfigDic;

        /// <summary>
        /// 缩略图存放路径
        /// </summary>
        public static Dictionary<string, ThumbnailSize> ThumbnailConfigDic
        {
            get
            {
                if (thumbnailConfigDic == null)
                {
                    lock (syncObject)
                    {
                        if (thumbnailConfigDic == null)
                        {
                            thumbnailConfigDic = new Dictionary<string, ThumbnailSize>();

                            foreach (var folder in UploadConfig.UploadFolders)
                            {
                                foreach (var s in folder.ThumbnailSizes)
                                {
                                    string _key = string.Format("{0}_{1}_{2}", folder.Path, s.Width, s.Height).ToLower();

                                    if (!thumbnailConfigDic.ContainsKey(_key))
                                    {
                                        thumbnailConfigDic.Add(_key, s);
                                    }
                                }
                            }
                        }
                    }
                }

                return thumbnailConfigDic;
            }
        }
    }
}