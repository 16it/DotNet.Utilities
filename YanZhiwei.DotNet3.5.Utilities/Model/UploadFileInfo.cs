namespace YanZhiwei.DotNet3._5.Utilities.Model
{
    /// <summary>
    /// 上传返回信息
    /// </summary>
    public sealed class UploadFileInfo
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 网站路径
        /// </summary>
        public string WebFilePath { get; set; }

        /// <summary>
        /// 获取文件名
        /// </summary>
        public string FileName { get; set; }
    }
}