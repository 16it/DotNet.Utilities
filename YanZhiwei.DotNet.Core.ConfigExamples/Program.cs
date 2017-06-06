using System;
using YanZhiwei.DotNet.Core.Config;
using YanZhiwei.DotNet.Core.Config.Model;

namespace YanZhiwei.DotNet.Core.ConfigExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                DownloadConfig _config = new DownloadConfig();
                _config.FileNameEncryptorIvHexString = "01 02 03 04 05 06 07 08 09 0a 0a 0c 0d 01 02 08";
                _config.FileNameEncryptorKey = "DotnetDownloadConfig";
                _config.LimitDownloadSpeedKb = 100;
                _config.DownLoadMainDirectory = @"D:\Downloads";

                ConfigContext _configHelper = new ConfigContext();
                _configHelper.Save<DownloadConfig>(_config);
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