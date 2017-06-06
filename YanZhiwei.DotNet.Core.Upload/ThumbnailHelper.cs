using YanZhiwei.DotNet.Core.Config.Model;
using YanZhiwei.DotNet3._5.Utilities.Enum;
using YanZhiwei.DotNet3._5.Utilities.WebForm.Core;

namespace YanZhiwei.DotNet.Core.Upload
{
    /// <summary>
    /// 生成缩略图
    /// </summary>
    public class ThumbnailHelper
    {
        /// <summary>
        /// 即时生成缩略图
        /// </summary>
        /// <param name="originalImagePath">原始图片路径</param>
        /// <param name="thumbnailPath">水印图片路径</param>
        /// <param name="size">图片大小</param>
        public static void BuilderThumbnail(string originalImagePath, string thumbnailPath, ThumbnailSize size)
        {
            WebThumbnailImage.BuilderThumbnails(originalImagePath, thumbnailPath,
                                            size.Width,
                                            size.Height,
                                            size.Mode,
                                            size.AddWaterMarker,
                                            size.WaterMarkerPosition,
                                            size.WaterMarkerPath,
                                            size.Quality);
        }

        /// <summary>
        /// 即时生成缩略图
        /// </summary>
        /// <param name="originalImagePath">原始图片路径</param>
        /// <param name="thumbnailPath">水印图片路径</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="mode">缩略图模式</param>
        /// <param name="isaddwatermark">是否添加水印</param>
        /// <param name="quality">图片质量</param>
        public static void BuilderThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbnailImageCutMode mode, bool isaddwatermark, int quality)
        {
            ThumbnailSize _size = new ThumbnailSize()
            {
                Width = width,
                Height = height,
                Mode = mode,
                AddWaterMarker = isaddwatermark,
                Quality = quality
            };
            BuilderThumbnail(originalImagePath, thumbnailPath, _size);
        }

        /// <summary>
        /// 即时生成缩略图
        /// </summary>
        /// <param name="originalImagePath">原始图片路径</param>
        /// <param name="thumbnailPath">水印图片路径</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void BuilderThumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        {
            BuilderThumbnail(originalImagePath, thumbnailPath, width, height, ThumbnailImageCutMode.Cut, false, 88);
        }
    }
}