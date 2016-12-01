namespace YanZhiwei.DotNet.Krypton.Utilities
{
    using ComponentFactory.Krypton.Toolkit;
    
    /// <summary>
    ///  RichTextBox 辅助类
    /// </summary>
    /// 时间：2016/11/23 15:38
    /// 备注：
    public static class KryptonRichTextBoxHelper
    {
        #region Methods
        
        /// <summary>
        /// 设置RichTextBox的值，并且设置焦点最最后
        /// </summary>
        /// <param name="richText">KryptonRichTextBox</param>
        /// <param name="text">文本</param>
        /// 时间：2016/11/23 15:40
        /// 备注：
        public static void SetTextFocused(this KryptonRichTextBox richText, string text)
        {
            richText.AppendText(text);
            richText.SelectionStart = richText.Text.Length;
            richText.ScrollToCaret();
            richText.Focus();
        }
        
        #endregion Methods
    }
}