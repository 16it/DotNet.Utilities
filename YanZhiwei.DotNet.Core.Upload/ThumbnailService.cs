using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using YanZhiwei.DotNet.Core.Model;

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
            //正则从文件路径里匹配出上传的文件夹目录.....
            Match _uploadfolder = Regex.Match(imagefilePath, @"^(.*)\\upload\\(.+)\\(day_\d+)\\(\d+)(\.[A-Za-z]+)$", RegexOptions.IgnoreCase);

            if(!_uploadfolder.Success)
                return;

            string _root = _uploadfolder.Groups[1].Value,
                   _folder = _uploadfolder.Groups[2].Value,
                   _subFolder = _uploadfolder.Groups[3].Value,
                   _fileName = _uploadfolder.Groups[4].Value,
                   _fileExt = _uploadfolder.Groups[5].Value;

            foreach(var pair in UploadConfigContext.ThumbnailConfigDic
                    .Where(t => t.Key.StartsWith(_folder.ToLower() + "_") && t.Value.Timming == timming))
            {
                ThumbnailSize _size = pair.Value;
                string _thumbnailFileFolder = string.Format("{0}\\upload\\{1}\\{2}\\thumb",
                                              _root, _folder, _subFolder);

                if(!Directory.Exists(_thumbnailFileFolder))
                    Directory.CreateDirectory(_thumbnailFileFolder);

                string _thumbnailFilePath = string.Format("{0}\\upload\\{1}\\{2}\\thumb\\{3}_{4}_{5}{6}",
                                            _root, _folder, _subFolder, _fileName, _size.Width, _size.Height, _fileExt);
                ThumbnailHelper.MakeThumbnail(imagefilePath, _thumbnailFilePath, _size);
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

            while(true)
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
            foreach(var group in UploadConfigContext.UploadConfig.UploadFolders)
            {
                string _folder = Path.Combine(UploadConfigContext.UploadPath, group.Path);

                if(!Directory.Exists(_folder))
                    continue;

                foreach(string dayFolder in Directory.GetDirectories(_folder))
                {
                    foreach(string filePath in Directory.GetFiles(dayFolder))
                    {
                        var m = Regex.Match(filePath, @"^(.+\\day_\d+)\\(\d+)(\.[A-Za-z]+)$", RegexOptions.IgnoreCase);

                        if(!m.Success)
                            continue;

                        var root = m.Groups[1].Value;
                        var fileName = m.Groups[2].Value;
                        var ext = m.Groups[3].Value;
                        var thumbnailFileFolder = Path.Combine(dayFolder, "Thumb");

                        if(!Directory.Exists(thumbnailFileFolder))
                            Directory.CreateDirectory(thumbnailFileFolder);

                        //删除配置里干掉的Size对应的缩略图
                        //先不启用，等配置添完了再启用
                        //foreach (var thumbFilePath in Directory.GetFiles(thumbnailFileFolder))
                        //{
                        //    if (!group.ThumbnailSizes.Exists(s =>
                        //        Regex.IsMatch(thumbFilePath, string.Format(@"\\\d+_{0}_{1}+\.[A-Za-z]+$", s.Width, s.Height))))
                        //        File.Delete(thumbFilePath);
                        //}

                        foreach(var size in group.ThumbnailSizes)
                        {
                            if(size.Timming != Timming.Lazy)
                                continue;

                            var thumbnailFilePath = string.Format("{0}\\thumb\\{1}_{2}_{3}{4}",
                                                                  root, fileName, size.Width, size.Height, ext);

                            if(File.Exists(thumbnailFilePath) && size.IsReplace)
                                File.Delete(thumbnailFilePath);

                            if(!File.Exists(thumbnailFilePath))
                                ThumbnailHelper.MakeThumbnail(filePath, thumbnailFilePath, size);
                        }
                    }
                }
            }
        }
    }
}