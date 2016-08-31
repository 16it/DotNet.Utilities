namespace YanZhiwei.DotNet.IrisSkin2.Utilities
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    using Sunisoft.IrisSkin;

    /// <summary>
    /// Iris 帮助类
    /// </summary>
    public class IrisHelper
    {
        #region Fields

        private const int DisableTag = 9999;

        #endregion Fields

        #region Methods

        /// <summary>
        /// 高亮控件颜色『需要控件有tag属性』
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="control">控件</param>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="fontColor">字体颜色</param>
        public static void ChangControlColor<T>(T control, Color color1, Color color2, Color fontColor)
        where T : Control
        {
            Bitmap _bmp = new Bitmap(control.Width, control.Height);
            using(Graphics _g = Graphics.FromImage(_bmp))
            {
                Rectangle _r = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
                using(LinearGradientBrush br = new LinearGradientBrush(
                    _r,
                    color1,
                    color2,
                    LinearGradientMode.Vertical))
                {
                    _g.FillRectangle(br, _r);
                }
            }
            control.BackgroundImage = _bmp;
            control.ForeColor = fontColor;
            control.Tag = DisableTag;
        }

        /// <summary>
        /// 高亮按钮控件
        /// </summary>
        /// <typeparam name="T">诸如panl</typeparam>
        /// <param name="control">诸如panl</param>
        /// <param name="button">需要高亮的按钮</param>
        /// <param name="color1">color1</param>
        /// <param name="color2">color2</param>
        /// <param name="fontColor">字体颜色</param>
        public static void HighlightButtonColor<T>(T control, Button button, Color color1, Color color2, Color fontColor)
        where T : Control
        {
            foreach(Control ct in control.Controls)
            {
                if(ct is Button)
                {
                    Button _button = (Button)ct;

                    if(_button.Name == button.Name)
                        ChangControlColor<Button>(button, color1, color2, fontColor);
                    else
                        RestoreButtonColor(_button);
                }
            }
        }

        /// <summary>
        /// 还原按钮默认主题
        /// </summary>
        /// <param name="button">Button</param>
        public static void RestoreButtonColor(Button button)
        {
            button.UseVisualStyleBackColor = true;
            button.Tag = null;
        }

        /// <summary>
        /// 设置程序主题
        /// </summary>
        /// <param name="skin">SkinEngine</param>
        /// <param name="bytes">byte数组</param>
        public static void SetupTheme(SkinEngine skin, byte[] bytes)
        {
            using(MemoryStream memoryStream = new MemoryStream(bytes))
            {
                skin.SkinStream = memoryStream;
            }
        }

        #endregion Methods
    }
}