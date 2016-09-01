using YanZhiwei.DotNet.SharpZipLib.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using YanZhiwei.DotNet2.Utilities.Common;
using System.Diagnostics;

namespace YanZhiwei.DotNet.SharpZipLib.Utilities.Tests
{
    [TestClass()]
    public class SharpZipHelperTests
    {
        [TestMethod()]
        public void MakeZipFileTest()
        {
            string[] _zipFilepathList = new string[3];
            _zipFilepathList[0] = @"D:\ToExecelTest_20160901113654.xls";
            _zipFilepathList[1] = @"D:\ToExecelTest_20160901132839.xls";
            _zipFilepathList[2] = @"D:\ToExecelTest_20160901132842.xls";
            string _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 0);
            bool _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 1);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 2);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 3);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 4);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 5);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 6);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 7);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 8);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
            Thread.Sleep(1000);
            _zipedFilePath = string.Format(@"D:\ToExecelTest_{0}.zip", DateTime.Now.FormatDate(12));
            SharpZipHelper.MakeZipFile(_zipFilepathList, _zipedFilePath, 9);
            _actual = File.Exists(_zipedFilePath);
            Assert.IsTrue(_actual);
        }

        [TestMethod()]
        public void UnMakeZipFileTest()
        {
            string _unZipFilePath = @"D:\ToExecelTest_20160901134224.zip";
            SharpZipHelper.UnMakeZipFile(_unZipFilePath);
            bool _actual = File.Exists(@"D:\ToExecelTest_20160901134224\ToExecelTest_20160901113654.xls");
            Assert.IsTrue(_actual);
        }
    }
}