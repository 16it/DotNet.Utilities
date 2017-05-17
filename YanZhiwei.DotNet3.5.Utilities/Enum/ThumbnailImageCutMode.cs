namespace YanZhiwei.DotNet3._5.Utilities.Enum
{
    /// <summary>
    /// 缩略图裁剪方式
    /// </summary>
    public enum ThumbnailImageCutMode
    {
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        W,

        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,

        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut,

        /// <summary>
        /// 不超出尺寸，比它小就不截了，不留白，大就缩小到最佳尺寸，主要为手机用
        /// </summary>
        Fit
    }
}