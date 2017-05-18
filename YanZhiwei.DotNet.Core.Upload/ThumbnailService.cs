using System;
using System.IO;
using System.Linq;
using System.Threading;
using YanZhiwei.DotNet.Core.Model;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Model;

namespace YanZhiwei.DotNet.Core.Upload
{
    /// <summary>
    /// 缩略图生成服务
    /// </summary>
    public class ThumbnailService
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imagefilePath">图片路径</param>
        /// <param name="timming">生成方式</param>
        public static void HandleThumbnail(string imagefilePath, Timming timming)
        {
            FileProperties _fileInfo = FileHelper.GetFileInfo(imagefilePath, @"^(.*)\\upload\\(.+)\\(day_\d+)\\(\d+)(\.[A-Za-z]+)$");

            if (_fileInfo == null) return;

            string _root = _fileInfo.Root,
                   _folder = _fileInfo.Folder,
                   _subFolder = _fileInfo.SubFolder,
                   _fileName = _fileInfo.FileName,
                   _fileExt = _fileInfo.FileNameExt;

            foreach (var pair in UploadConfigContext.ThumbnailConfigDic
                    .Where(t => t.Key.StartsWith(_fileInfo.Folder.ToLower() + "_", StringComparison.OrdinalIgnoreCase) && t.Value.Timming == timming))
            {
                ThumbnailSize _size = pair.Value;
                string _thumbnailFileFolder = string.Format("{0}\\upload\\{1}\\{2}\\thumb",
                                              _root, _folder, _subFolder);

                if (!Directory.Exists(_thumbnailFileFolder))
                    Directory.CreateDirectory(_thumbnailFileFolder);

                string _thumbnailFilePath = string.Format("{0}\\upload\\{1}\\{2}\\thumb\\{3}_{4}_{5}{6}",
                                            _root, _folder, _subFolder, _fileName, _size.Width, _size.Height, _fileExt);
                ThumbnailHelper.BuilderThumbnail(imagefilePath, _thumbnailFilePath, _size);
            }
        }

        /// <summary>
        /// 即时生成图片缩略图
        /// </summary>
        /// <param name="imagefilePath">图片路径</param>
        public static void HandleImmediateThumbnail(string imagefilePath)
        {
            HandleThumbnail(imagefilePath, Timming.Immediate);
        }

        /// <summary>
        /// 延迟生成图片缩略图
        /// </summary>
        /// <param name="intervalMunites">延迟分钟</param>
        public static void HandlerLazyThumbnail(int intervalMunites)
        {
            FileSystemWatcher _watcher = new FileSystemWatcher(UploadConfigContext.UploadPath);
            _watcher.IncludeSubdirectories = true;
            _watcher.Created += (s, e) =>
            {
                HandleThumbnail(e.FullPath, Timming.Lazy);
            };
            _watcher.EnableRaisingEvents = true;

            while (true)
            {
                HandlerLazyThumbnail();
                GC.Collect();
                Console.WriteLine("等待 {0} 分钟再重新扫描...........", intervalMunites);
                Thread.Sleep(intervalMunites * 60 * 1000);
            }
        }

        /// <summary>
        /// 延迟生成图片缩略图
        /// </summary>
        public static void HandlerLazyThumbnail()
        {
            foreach (var group in UploadConfigContext.UploadConfig.UploadFolders)
            {
                string _folder = Path.Combine(UploadConfigContext.UploadPath, group.Path);

                if (!Directory.Exists(_folder))
                    continue;

                foreach (string dayFolder in Directory.GetDirectories(_folder))
                {
                    foreach (string filePath in Directory.GetFiles(dayFolder))
                    {
                        FileProperties _fileInfo = FileHelper.GetFileInfo(filePath, @"^(.+\\day_\d+)\\(\d+)(\.[A-Za-z]+)$");

                        if (_fileInfo == null) continue;

                        string _root = _fileInfo.Root,
                               _fileName = _fileInfo.FileName,
                               _fileExt = _fileInfo.FileNameExt,
                               _thumbnailFileFolder = Path.Combine(dayFolder, "Thumb");

                        if (!Directory.Exists(_thumbnailFileFolder))
                            Directory.CreateDirectory(_thumbnailFileFolder);

                        foreach (var size in group.ThumbnailSizes)
                        {
                            if (size.Timming != Timming.Lazy)
                                continue;

                            string _thumbnailFilePath = string.Format("{0}\\thumb\\{1}_{2}_{3}{4}",
                                                        _root, _fileName, size.Width, size.Height, _fileExt);

                            if (File.Exists(_thumbnailFilePath) && size.IsReplace)
                                File.Delete(_thumbnailFilePath);

                            if (!File.Exists(_thumbnailFilePath))
                                ThumbnailHelper.BuilderThumbnail(filePath, _thumbnailFilePath, size);
                        }
                    }
                }
            }
        }
    }
}