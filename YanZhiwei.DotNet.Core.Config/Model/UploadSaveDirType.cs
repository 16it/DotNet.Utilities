namespace YanZhiwei.DotNet.Core.Config.Model
{
    /// <summary>
    /// 文件上传目录存储规则
    /// </summary>
    public enum UploadSaveDirType
    {
        /// <summary>
        /// 按天
        /// </summary>
        Day = 1,

        /// <summary>
        /// 按月份
        /// </summary>
        Month = 2,

        /// <summary>
        /// 按扩展名
        /// </summary>
        Ext = 3
    }
}