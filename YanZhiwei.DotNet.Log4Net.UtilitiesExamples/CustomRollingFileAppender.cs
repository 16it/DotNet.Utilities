using log4net.Appender;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YanZhiwei.DotNet.Log4Net.UtilitiesExamples
{
    public class CustomRollingFileAppender : RollingFileAppender
    {
        private readonly static Type declaringType = typeof(CustomRollingFileAppender);

        #region Public Instance Properties

        /// <summary>
        /// Gets or sets the maximum number of backups.
        /// </summary>
        /// <value>
        /// The maximum number of backups.
        /// </value>
        public int MaxNumberOfBackups
        {
            get { return m_maxNumberOfBackups; }
            set { m_maxNumberOfBackups = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of days.
        /// </summary>
        /// <value>
        /// The maximum number of days.
        /// </value>
        public int MaxNumberOfDays
        {
            get { return m_maxNumberOfDays; }
            set { m_maxNumberOfDays = value; }
        }

        #endregion Public Instance Properties

        protected override void AdjustFileBeforeAppend()
        {
            LogLog.Debug(declaringType, declaringType.ToString());

            base.AdjustFileBeforeAppend();

            //Now clean out old files
            if (m_maxNumberOfBackups > 0 || m_maxNumberOfDays > 0)
            {
                LogLog.Debug(declaringType,
                                            string.Format("Removing older files Max Backups: [{0}] Age limit [{1}] ", m_maxNumberOfBackups, m_maxNumberOfDays));
                RemoveOldLogFiles();
            }
        }

        protected void RemoveOldLogFiles()
        {
            string defaultSearch = string.Empty;
            string defaultMatch = string.Empty;
            try
            {
                FileInfo fiBase = new FileInfo(base.File);
                defaultSearch = GetWildcardPatternForFile(base.File);

                LogLog.Debug(declaringType, string.Format("Delete files search string: [{0}]", defaultSearch));
                string[] files = Directory.GetFiles(fiBase.DirectoryName);
                //We need to load this into a list of FileInfos.
                List<FileInfo> fiList = new List<FileInfo>();
                //This will perform badly in large directories - TEST!
                foreach (var file in files)
                {
                    //see if this is old
                    fiList.Add(new FileInfo(file));
                }

                //Our delete list
                List<FileInfo> fiToDelete = new List<FileInfo>();

                if (this.m_maxNumberOfDays > 0)
                {
                    //Now, sort by date/time and loop through to add old files
                    foreach (var item in fiList.OrderBy(itm => itm.CreationTime))
                    {
                        if (item.CreationTime < DateTime.Now.AddDays(this.m_maxNumberOfDays * -1))
                        {
                            fiToDelete.Add(item);
                        }
                    }
                }

                // Now we can delete
                if (fiToDelete.Count > 0)
                {
                    DeleteFiles(fiToDelete.ToArray());
                }
            }
            catch (Exception removeEx)
            {
                ErrorHandler.Error(string.Format("Exception while removing max files [{0}] count [{1}]", defaultSearch, m_maxNumberOfBackups), removeEx, log4net.Core.ErrorCode.GenericFailure);
            }
        }

        protected void DeleteFiles(FileInfo[] files)
        {
            for (int i = 0; i < files.Count(); i++)
            {
                LogLog.Debug(declaringType, "Deleting file: [" + files[i].FullName + "]");
                System.IO.File.Delete(files[i].FullName);
            }
        }

        /// <summary>
        /// Generates a wildcard pattern that can be used to find all files
        /// that are similar to the base file name.
        /// </summary>
        /// <param name="baseFileName"></param>
        /// <returns></returns>
        private string GetWildcardPatternForFile(string baseFileName)
        {
            //This is a file i/o pattern, not a regex

            if (this.PreserveLogFileNameExtension)
            {
                return string.Format("{0}.*{1}", Path.GetFileNameWithoutExtension(baseFileName), Path.GetExtension(baseFileName));
            }
            else
            {
                return string.Format("{0}.*", baseFileName);
            }
        }

        #region Private Instance Fields

        /// <summary>
        /// Default to 0 indicating keep all files
        /// </summary>
        private int m_maxNumberOfBackups = 0;

        /// <summary>
        /// Default to 0 indicating keep all files
        /// </summary>
        private int m_maxNumberOfDays = 0;

        #endregion Private Instance Fields
    }
}