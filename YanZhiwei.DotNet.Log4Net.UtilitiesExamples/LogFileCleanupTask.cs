using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace YanZhiwei.DotNet.Log4Net.UtilitiesExamples
{
    public class LogFileCleanupTask
    {
        #region - Constructor -

        public LogFileCleanupTask()
        {
        }

        #endregion - Constructor -

        #region - Methods -

        /// <summary>
        /// Cleans up. Auto configures the cleanup based on the log4net configuration
        /// </summary>
        /// <param name="date">Anything prior will not be kept.</param>
        public void CleanUp(DateTime date)
        {
            string directory = string.Empty;
            string filePrefix = string.Empty;

            var repo = LogManager.GetAllRepositories().FirstOrDefault();
            if (repo == null) return;

            var appList = repo.GetAppenders().Where(x => x.GetType() == typeof(RollingFileAppender));
            if (appList != null)
            {
                foreach (var app in appList)
                {
                    var appender = app as RollingFileAppender;

                    CleanupLogs(appender, 5);
                    //directory = Path.GetDirectoryName(appender.File);
                    //filePrefix = Path.GetFileName(appender.File);

                    //   CleanUp(directory, filePrefix, date);
                }
            }
        }
        private static void CleanupLogs(RollingFileAppender appender, int maxAgeInDays)
        {
            if (!File.Exists(appender.File)) return;
            var datePatternBits = appender.DatePattern.Split(new char[] { '\'' }, StringSplitOptions.None);
            if (datePatternBits.Count() != 5 || datePatternBits[0].Length > 0 || datePatternBits[4].Length > 0)
                throw new ApplicationException(
                    string.Format(
                        "Log4Net RollingFileAppender ({0} DatePattern unexpected format. Expected \"\'xxx\'date\'eee\'\" {1},{2},{3}",
                        appender.Name, datePatternBits.Count(), datePatternBits[0].Length, datePatternBits[4].Length));
            List<string> logPatternsToKeep = new List<string>();
            for (var i = 0; i <= maxAgeInDays; i++)
                logPatternsToKeep.Add(DateTime.Now.AddDays(-i).ToString(appender.DatePattern));

            FileInfo fileInfo = new FileInfo(appender.File);

            var searchString = string.Format("{0}*{1}", datePatternBits[1], datePatternBits[3]);
            var folderFiles =
                fileInfo.Directory.GetFiles(searchString);
            var logFiles = folderFiles
                .Where(x => logPatternsToKeep.All(y => !x.Name.Contains(y) && x.Name != fileInfo.Name));

            foreach (var log in logFiles)
                if (File.Exists(log.FullName)) File.Delete(log.FullName);
        }
        /// <summary>
        /// Cleans up.
        /// </summary>
        /// <param name="logDirectory">The log directory.</param>
        /// <param name="logPrefix">The log prefix. Example: logfile dont include the file extension.</param>
        /// <param name="date">Anything prior will not be kept.</param>
        public void CleanUp(string logDirectory, string logPrefix, DateTime date)
        {


            var dirInfo = new DirectoryInfo(logDirectory);
            if (!dirInfo.Exists)
                return;

            var fileInfos = dirInfo.GetFiles("{0}*.*".Sub(logPrefix));
            if (fileInfos.Length == 0)
                return;

            foreach (var info in fileInfos)
            {
                if (info.LastWriteTime < date)
                {
                    info.Delete();
                }
            }
        }

        #endregion - Methods -
    }

    /// <summary>
    /// Extension helper methods for strings
    /// </summary>
    [DebuggerStepThrough, DebuggerNonUserCode]
    public static class StringExtensions
    {
        /// <summary>
        /// Formats a string using the <paramref name="format"/> and <paramref name="args"/>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// <returns>A string with the format placeholders replaced by the args.</returns>
        public static string Sub(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}