using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YanZhiwei.DotNet.Core.Model;
using YanZhiwei.DotNet.Core.Config;
namespace YanZhiwei.DotNet.Core.ConfigExamples
{
    class Program
    {
        static void Main(string[] args)
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
