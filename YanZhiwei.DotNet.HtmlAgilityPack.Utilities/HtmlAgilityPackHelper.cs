namespace YanZhiwei.DotNet.HtmlAgilityPack.Utilities
{
    using global::HtmlAgilityPack;
    
    /// <summary>
    /// HtmlAgilityPack辅助类
    /// </summary>
    /// 时间：2016/10/26 17:20
    /// 备注：
    public static class HtmlAgilityPackHelper
    {
        #region Methods
        
        /// <summary>
        /// 获取节点的属性值
        /// </summary>
        /// <param name="node">HtmlNode</param>
        /// <param name="attrName">属性名称</param>
        /// <returns>属性值</returns>
        public static string GetNodeAttr(this HtmlNode node, string attrName)
        {
            if(node == null || string.IsNullOrEmpty(attrName) || node.Attributes[attrName] == null)
            {
                return string.Empty;
            }
            
            return node.Attributes[attrName].Value;
        }
        
        /// <summary>
        /// 获取节点的InnerHtml
        /// </summary>
        /// <param name="node">HtmlNode</param>
        /// <returns>获取节点的InnerHtml</returns>
        public static string GetNodeInnerHtml(this HtmlNode node)
        {
            if(node == null)
            {
                return string.Empty;
            }
            
            return node.InnerHtml;
        }
        
        /// <summary>
        /// 获取节点的OuterHtml
        /// </summary>
        /// <param name="node">HtmlNode</param>
        /// <returns>OuterHtml</returns>
        public static string GetNodeOutHtml(this HtmlNode node)
        {
            if(node == null)
            {
                return string.Empty;
            }
            
            return node.OuterHtml;
        }
        
        /// <summary>
        /// 获取节点的InnerText的值
        /// </summary>
        /// <param name="node">HtmlNode</param>
        /// <returns>InnerText的值</returns>
        public static string GetNodeText(this HtmlNode node)
        {
            if(node == null)
            {
                return string.Empty;
            }
            
            return node.InnerText;
        }
        
        #endregion Methods
    }
}