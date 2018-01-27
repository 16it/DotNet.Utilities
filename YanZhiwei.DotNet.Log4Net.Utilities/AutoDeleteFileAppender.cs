namespace YanZhiwei.DotNet.Log4Net.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using log4net.Appender;
    using log4net.Core;

    /// <summary>
    /// 自动删除多少天日志 RollingFileAppender
    /// </summary>
    /// <seealso cref="log4net.Appender.RollingFileAppender" />
    public class AutoDeleteFileAppender : RollingFileAppender
    {
        #region Fields

        private static readonly Type declaringType = typeof(AutoDeleteFileAppender);

        private string baseDirectory = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 最多保留多少天日志
        /// </summary>
        public int MaxNumberOfDays
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Performs any required rolling before outputting the next event
        /// </summary>
        /// <remarks>
        /// Handles append time behavior for RollingFileAppender.  This checks
        /// if a roll over either by date (checked first) or time (checked second)
        /// is need and then appends to the file last.
        /// </remarks>
        protected override void AdjustFileBeforeAppend()
        {
            var _now = DateTime.Now;
            int _curHour = _now.Hour, _curMinu = _now.Minute, _curSec = _now.Second;

            if (_curHour == 23 && _curMinu == 59 && _curSec == 59)//每天执行一次
            {
                if (MaxNumberOfDays > 0 && Directory.Exists(baseDirectory))
                {
                    RemoveOldLogFiles();
                }
            }

            base.AdjustFileBeforeAppend();
        }

        /// <summary>
        /// Creates and opens the file for logging.  If <see cref="P:log4net.Appender.RollingFileAppender.StaticLogFileName" />
        /// is false then the fully qualified name is determined and used.
        /// </summary>
        /// <param name="fileName">the name of the file to open</param>
        /// <param name="append">true to append to existing file</param>
        /// <remarks>
        /// This method will ensure that the directory structure
        /// for the <paramref name="fileName" /> specified exists.
        /// </remarks>
        protected override void OpenFile(string fileName, bool append)
        {
            baseDirectory = Path.GetDirectoryName(fileName);
            string fileNameOnly = Path.GetFileName(fileName);

            base.OpenFile(fileName, append);
        }

        protected void RemoveOldLogFiles()
        {
            try
            {
                string[] _logFiles = Directory.GetFiles(baseDirectory, "*.log", SearchOption.AllDirectories);
                List<FileInfo> _fiLogList = new List<FileInfo>();
                foreach (var file in _logFiles)
                {
                    _fiLogList.Add(new FileInfo(file));
                }

                foreach (var item in _fiLogList)
                {
                    if (item.LastAccessTime < DateTime.Now.AddDays(-MaxNumberOfDays))
                    {
                        base.DeleteFile(item.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Error(string.Format("删除{0}天日志发生错误", MaxNumberOfDays), ex, ErrorCode.GenericFailure);
            }
        }

        #endregion Methods
    }
}