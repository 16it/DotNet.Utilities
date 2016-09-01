namespace YanZhiwei.DotNet.SharpZipLib.Utilities
{
    using System.IO;

    using ICSharpCode.SharpZipLib.Zip;

    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.Operator;

    /// <summary>
    /// 文件压缩帮助类
    /// </summary>
    public class SharpZipHelper
    {
        #region Methods

        /// <summary>
        /// 多个文件压缩
        /// </summary>
        /// <param name="filepathListToZip">要压缩文件(绝对文件路径)</param>
        /// <param name="zipedfiledPath">压缩(绝对文件路径)</param>
        /// <param name="compressionLevel">压缩比,0~9，数值越大压缩率越高</param>
        public static void MakeZipFile(string[] filepathListToZip, string zipedfiledPath, int compressionLevel)
        {
            CheckedZipParmter(filepathListToZip, zipedfiledPath, compressionLevel);
            MakeZipFile(filepathListToZip, zipedfiledPath, compressionLevel, string.Empty, string.Empty);
        }

        /// <summary>
        /// 多个文件压缩
        /// </summary>
        /// <param name="filepathListToZip">要压缩文件(绝对文件路径)</param>
        /// <param name="zipedfiledPath">压缩(绝对文件路径)</param>
        /// <param name="compressionLevel">压缩比,0~9，数值越大压缩率越高</param>
        /// <param name="password">加密密码</param>
        /// <param name="comment">压缩文件描述</param>
        public static void MakeZipFile(string[] filepathListToZip, string zipedfiledPath, int compressionLevel,
            string password, string comment)
        {
            if(File.Exists(zipedfiledPath))
                File.Delete(zipedfiledPath);

            using(FileStream fileSream = File.Open(zipedfiledPath, FileMode.Create))
            {
                using(ZipOutputStream zipOutputStream = new ZipOutputStream(fileSream))
                {
                    zipOutputStream.SetLevel(compressionLevel);

                    if(!string.IsNullOrEmpty(password))
                        zipOutputStream.Password = password;

                    if(!string.IsNullOrEmpty(comment))
                        zipOutputStream.SetComment(comment);

                    foreach(string filename in filepathListToZip)
                    {
                        using(FileStream zipFilestream = File.OpenRead(filename))
                        {
                            byte[] _zipBuffer = new byte[zipFilestream.Length];
                            zipFilestream.Read(_zipBuffer, 0, _zipBuffer.Length);
                            ZipEntry _zipEntry = new ZipEntry(Path.GetFileName(filename));
                            _zipEntry.Size = zipFilestream.Length;
                            zipOutputStream.PutNextEntry(_zipEntry);
                            zipOutputStream.Write(_zipBuffer, 0, _zipBuffer.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 实现解压操作(密码为空,默认解压当压缩文件同级目录)
        /// </summary>
        /// <param name="unZipfilePath">要解压文件Zip</param>
        public static void UnMakeZipFile(string unZipfilePath)
        {
            UnMakeZipFile(unZipfilePath, string.Empty, string.Empty);
        }

        /// <summary>
        /// 实现解压操作
        /// </summary>
        /// <param name="unZipfilePath">要解压Zip文件路径</param>
        /// <param name="unZipDir">解压目的路径(物理路径)</param>
        /// <param name="password">解压密码</param>
        public static void UnMakeZipFile(string unZipfilePath, string unZipDir, string password)
        {
            ValidateOperator.Begin().NotNullOrEmpty(unZipfilePath, "要解压Zip文件路径").IsFilePath(unZipfilePath).CheckFileExists(unZipfilePath);
            using(FileStream unZipfileStream = File.OpenRead(unZipfilePath))
            {
                using(ZipInputStream zipInputStream = new ZipInputStream(unZipfileStream))
                {
                    if(!string.IsNullOrEmpty(password))
                        zipInputStream.Password = password;

                    ZipEntry _zipEntry;
                    unZipDir = string.IsNullOrEmpty(unZipDir) == true ? string.Format(@"{0}\", FileHelper.GetExceptEx(unZipfilePath)) : unZipDir;
                    FileHelper.CreateDirectory(unZipDir);
                    int _size = 1024;

                    while((_zipEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        string _filename = Path.GetFileName(_zipEntry.Name);

                        if(!string.IsNullOrEmpty(_filename))
                        {
                            using(FileStream _newstream = File.Create(unZipDir + "\\" + _filename))
                            {
                                while(true)
                                {
                                    byte[] _unZipBuffer = new byte[_size];
                                    _size = zipInputStream.Read(_unZipBuffer, 0, _unZipBuffer.Length);

                                    if(_size > 0)
                                    {
                                        _newstream.Write(_unZipBuffer, 0, _size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void CheckedZipParmter(string[] filepathListToZip, string zipedfiledPath, int compressionLevel)
        {
            ValidateOperator.Begin().NotNull(filepathListToZip, "需要压缩文件(绝对文件路径)").NotNull(zipedfiledPath, "生成压缩文件路径").IsFilePath(zipedfiledPath).InRange(compressionLevel, 0, 9, "文件压缩比");

            foreach(string zipFilepath in filepathListToZip)
            {
                ValidateOperator.Begin().NotNull(zipFilepath, "需要压缩文件").IsFilePath(zipFilepath).CheckFileExists(zipFilepath);
            }
        }

        #endregion Methods
    }
}