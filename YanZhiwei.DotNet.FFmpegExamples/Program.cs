using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YanZhiwei.DotNet.FFmpeg.Utilities;
namespace YanZhiwei.DotNet.FFmpegExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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
