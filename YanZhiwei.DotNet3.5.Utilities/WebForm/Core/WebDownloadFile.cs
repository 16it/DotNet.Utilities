using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Operator;
using YanZhiwei.DotNet2.Utilities.Result;
using YanZhiwei.DotNet2.Utilities.Common;
namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Core
{
    /// <summary>
    /// 文件下载
    /// </summary>
    public class WebDownloadFile
    {
        //http协议从1.1开始支持获取文件的部分内容，这为并行下载以及断点续传提供了技术支持。
        //它通过在Header里两个参数实现的，客户端发请求时对应的是 Range ，服务器端响应时对应的是 Content-Range ；

        //Range 参数还支持多个区间，用逗号分隔，下面对另一个内容为”hello world”的文件”a.html”多区间请求，
        //这时response的 Content-Type 不再是原文件mime类型，而用一种 multipart/byteranges 类型表示

        /// <summary>
        ///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePhysicsPath">带文件名下载路径</param>
        /// <param name="limitSpeed">每秒允许下载的字节数</param>
        public static FileDownloadResult FileDownload(string fileName, string filePhysicsPath, ulong limitSpeed)
        {
            ValidateOperator.Begin().NotNullOrEmpty(fileName, "下载文件名").CheckFileExists(filePhysicsPath);
           
            try
            {
                using (FileStream fileStream = new FileStream(filePhysicsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader fileReader = new BinaryReader(fileStream))
                    {
                        HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                        HttpContext.Current.Response.Buffer = false;
                        long _fileLength = fileStream.Length,
                             _startIndex = 0;
                        int _pack = 10240; //10K bytes
                        int _sleep = (int)Math.Floor((double)((ulong)(1000 * _pack) / limitSpeed)) + 1; // int sleep = 200;   //每秒5次   即5*10K bytes每秒

                        if (HttpContext.Current.Request.Headers["Range"] != null)
                        {
                            HttpContext.Current.Response.StatusCode = 206;
                            string[] _buffer = HttpContext.Current.Request.Headers["Range"].Split(new char[] { '=', '-' });
                            _startIndex = Convert.ToInt64(_buffer[1]);
                        }

                        HttpContext.Current.Response.AddHeader("Content-Length", (_fileLength - _startIndex).ToString());

                        if (_startIndex != 0)
                        {
                            HttpContext.Current.Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", _startIndex, _fileLength - 1, _fileLength));
                        }

                        HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                        HttpContext.Current.Response.ContentType = ResponseContentType.Bin;
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                        fileReader.BaseStream.Seek(_startIndex, SeekOrigin.Begin);
                        int _maxCount = (int)Math.Floor((double)((_fileLength - _startIndex) / _pack)) + 1;

                        for (int i = 0; i < _maxCount; i++)
                        {
                            if (HttpContext.Current.Response.IsClientConnected)
                            {
                                HttpContext.Current.Response.BinaryWrite(fileReader.ReadBytes(_pack));
                                Thread.Sleep(_sleep);
                            }
                            else
                            {
                                i = _maxCount;
                            }
                        }
                    }
                }
                return FileDownloadResult.Success(fileName, filePhysicsPath);
            }
            catch (Exception ex)
            {
                return FileDownloadResult.Fail(fileName, filePhysicsPath, ex.Message);
            }
        }

        /// <summary>
        /// 分块下载
        /// </summary>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePhysicsPath">文件物理路径</param>
        /// <returns>下载是否成功</returns>
        public static void FileDownload(string fileName, string filePhysicsPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(fileName, "下载文件名").CheckFileExists(filePhysicsPath);
            string _filePath = filePhysicsPath;
            long _chunkSize = 204800;             //块大小
            byte[] _buffer = new byte[_chunkSize]; //200K的缓冲区
            long _dataToRead = 0;                 //已读的字节数
            fileName = string.IsNullOrEmpty(fileName) == true ? Path.GetFileName(_filePath) : fileName;

            using (FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _dataToRead = fileStream.Length;
                HttpContext.Current.Response.ContentType = ResponseContentType.Bin;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                HttpContext.Current.Response.AddHeader("Content-Length", _dataToRead.ToString());

                while (_dataToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int _length = fileStream.Read(_buffer, 0, Convert.ToInt32(_chunkSize));
                        HttpContext.Current.Response.OutputStream.Write(_buffer, 0, _length);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Clear();
                        _dataToRead -= _length;
                    }
                    else
                    {
                        _dataToRead = -1;
                    }
                }
            }
        }
    }
}