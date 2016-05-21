using System;
using YanZhiwei.DotNet.FFmpeg.Utilities;

namespace YanZhiwei.DotNet.FFmpegExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //rtsp://218.204.223.237:554/live/1/66251FC11353191F/e7ooqwcfbqjoo80j.sdp
                //http://www.haoweis.com:8800/hls/Media/Live/P0226GQ5V01700000000/1/2/live.m3u8?user=100868&password=100868
                //ffmpeg -i rtsp://218.204.223.237:554/live/1/66251FC11353191F/e7ooqwcfbqjoo80j.sdp -r 1/60 -f image2 d:\\images%05d.png -c copy -map 0 -f segment -segment_time 60 -segment_format mp4 d:\\out%03d.mp4
                FFmpegHelper _ffmperHelper = new FFmpegHelper(@"D:\ffmpeg\bin\ffmpeg.exe");
                using (_ffmperHelper.SaveVideoStream(@"http://www.haoweis.com:8800/hls/Media/Live/P0226GQ5V01700000000/1/2/live.m3u8?user=100868&password=100868", @"D:\dump2.flv"))
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}