using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using YanZhiwei.DotNet3._5.Utilities.Enum;

namespace YanZhiwei.DotNet3._5.Utilities.WebForm.Core
{
    /// <summary>
    /// 生成缩略图
    /// </summary>
    public class WebThumbnailImage
    {
        /// <summary>
        ///  生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="isaddwatermark">是否添加水印</param>
        /// <param name="quality">图片品质</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode = "Cut", bool isaddwatermark = false, int quality = 75)
        {
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, isaddwatermark, ImagePosition.Default, null, quality);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        /// <param name="isaddwatermark">是否添加水印</param>
        /// <param name="quality">图片品质</param>
        /// <param name="imagePosition">水印位置</param>
        /// <param name="waterImage">水印图片名称</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, bool isaddwatermark, ImagePosition imagePosition, string waterImage = null, int quality = 75)
        {
            Image _originalImage = Image.FromFile(originalImagePath);
            int _towidth = width;
            int _toheight = height;
            int x = 0;
            int y = 0;
            int _ow = _originalImage.Width;
            int _oh = _originalImage.Height;

            switch(mode)
            {
                case "HW"://指定高宽缩放（可能变形）
                    break;

                case "W"://指定宽，高按比例
                    _toheight = _originalImage.Height * width / _originalImage.Width;
                    break;

                case "H"://指定高，宽按比例
                    _towidth = _originalImage.Width * height / _originalImage.Height;
                    break;

                case "Cut"://指定高宽裁减（不变形）
                    if(_originalImage.Width >= _towidth && _originalImage.Height >= _toheight)
                    {
                        if((double)_originalImage.Width / (double)_originalImage.Height > (double)_towidth / (double)_toheight)
                        {
                            _oh = _originalImage.Height;
                            _ow = _originalImage.Height * _towidth / _toheight;
                            y = 0;
                            x = (_originalImage.Width - _ow) / 2;
                        }
                        else
                        {
                            _ow = _originalImage.Width;
                            _oh = _originalImage.Width * height / _towidth;
                            x = 0;
                            y = (_originalImage.Height - _oh) / 2;
                        }
                    }
                    else
                    {
                        x = (_originalImage.Width - _towidth) / 2;
                        y = (_originalImage.Height - _toheight) / 2;
                        _ow = _towidth;
                        _oh = _toheight;
                    }

                    break;

                case "Fit"://不超出尺寸，比它小就不截了，不留白，大就缩小到最佳尺寸，主要为手机用
                    if(_originalImage.Width > _towidth && _originalImage.Height > _toheight)
                    {
                        if((double)_originalImage.Width / (double)_originalImage.Height > (double)_towidth / (double)_toheight)
                            _toheight = _originalImage.Height * width / _originalImage.Width;
                        else
                            _towidth = _originalImage.Width * height / _originalImage.Height;
                    }
                    else if(_originalImage.Width > _towidth)
                    {
                        _toheight = _originalImage.Height * width / _originalImage.Width;
                    }
                    else if(_originalImage.Height > _toheight)
                    {
                        _towidth = _originalImage.Width * height / _originalImage.Height;
                    }
                    else
                    {
                        _towidth = _originalImage.Width;
                        _toheight = _originalImage.Height;
                        _ow = _towidth;
                        _oh = _toheight;
                    }

                    break;

                default:
                    break;
            }

            //新建一个bmp图片
            Image _bitmap = new Bitmap(_towidth, _toheight);
            //新建一个画板
            Graphics _graphics = Graphics.FromImage(_bitmap);
            //设置高质量插值法
            _graphics.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            _graphics.SmoothingMode = SmoothingMode.HighQuality;
            _graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _graphics.CompositingQuality = CompositingQuality.HighQuality;
            //清空画布并以透明背景色填充
            _graphics.Clear(Color.White);
            //在指定位置并且按指定大小绘制原图片的指定部分
            _graphics.DrawImage(_originalImage, new Rectangle(0, 0, _towidth, _toheight),
                                new Rectangle(x, y, _ow, _oh),
                                GraphicsUnit.Pixel);

            //加图片水印
            if(isaddwatermark)
            {
                if(string.IsNullOrEmpty(waterImage))
                    waterImage = "watermarker.png";

                Image _copyImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, waterImage));
                int _xPosOfWm;
                int _yPosOfWm;
                int _wmHeight = _copyImage.Height;
                int _wmWidth = _copyImage.Width;
                int _phHeight = _toheight;
                int _phWidth = _towidth;

                switch(imagePosition)
                {
                    case ImagePosition.LeftBottom:
                        _xPosOfWm = 70;
                        _yPosOfWm = _phHeight - _wmHeight - 70;
                        break;

                    case ImagePosition.LeftTop:
                        _xPosOfWm = 70;
                        _yPosOfWm = 0 - 70;
                        break;

                    case ImagePosition.RightTop:
                        _xPosOfWm = _phWidth - _wmWidth - 70;
                        _yPosOfWm = 0 - 70;
                        break;

                    case ImagePosition.RigthBottom:
                        _xPosOfWm = _phWidth - _wmWidth - 70;
                        _yPosOfWm = _phHeight - _wmHeight - 70;
                        break;

                    default:
                        _xPosOfWm = 10;
                        _yPosOfWm = 0;
                        break;
                }

                _graphics.DrawImage(_copyImage, new Rectangle(_xPosOfWm, _yPosOfWm, _copyImage.Width, _copyImage.Height), 0, 0, _copyImage.Width, _copyImage.Height, GraphicsUnit.Pixel);
            }

            // 以下代码为保存图片时,设置压缩质量
            EncoderParameters _encoderParams = new EncoderParameters();
            long[] _qualityArray = new long[1];
            _qualityArray[0] = quality;
            EncoderParameter _encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, _qualityArray);
            _encoderParams.Param[0] = _encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo[] _arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo _jpegICI = null;

            for(int i = 0; i < _arrayICI.Length; i++)
            {
                if(_arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    _jpegICI = _arrayICI[i];
                    //设置JPEG编码
                    break;
                }
            }

            try
            {
                if(_jpegICI != null)
                {
                    _bitmap.Save(thumbnailPath, _jpegICI, _encoderParams);
                }
                else
                {
                    //以jpg格式保存缩略图
                    _bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _originalImage.Dispose();
                _bitmap.Dispose();
                _graphics.Dispose();
            }
        }
    }
}