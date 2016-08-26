using System.Diagnostics;
using YanZhiwei.DotNet2.Utilities.DataOperator;

namespace YanZhiwei.DotNet.FFmpeg.Utilities
{
    /// <summary>
    /// FFmpeg 帮助类
    /// </summary>
    /// 时间：2016/5/21 0:02
    /// 备注：
    public class FFmpegHelper
    {
        /// <summary>
        /// ffmpeg.exe路径
        /// </summary>
        /// 时间：2016/5/21 0:05
        /// 备注：
        public string FFmpegPath = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ffmpegPath">ffmpeg.exe路径.</param>
        /// 时间：2016/5/21 0:06
        /// 备注：
        public FFmpegHelper(string ffmpegPath)
        {
            ValidateHelper.Begin().CheckFileExists(ffmpegPath, "ffmpeg.exe");
            FFmpegPath = ffmpegPath;
        }

        /// <summary>
        /// 保存视频流到本地
        /// </summary>
        /// <param name="videoStream">视频流路径</param>
        /// <param name="savepath">视频流保存本地路径</param>
        /// 时间：2016/5/21 0:09
        /// 备注：
        public Process SaveVideoStream(string videoStream, string savepath)
        {
            ValidateHelper.Begin().NotNullOrEmpty(videoStream, "视频流路径").NotNullOrEmpty(savepath, "视频流保存本地路径").IsFilePath(savepath, "视频流保存本地路径");
            Process _process = new Process();
            _process.StartInfo.FileName = FFmpegPath;
            _process.StartInfo.Arguments = string.Format(@"-i {0} -c copy {1}", videoStream, savepath);
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.CreateNoWindow = false;
            _process.Start();
            _process.WaitForExit();
            _process.StandardError.ReadToEnd();
            return _process;
        }
    }
}