namespace YanZhiwei.DotNet.HtmlAgilityPack.Utilities
{
    using global::HtmlAgilityPack;
    
    using DotNet2.Utilities.Operator;
    using DotNet3._5.Utilities.WebForm.Core;
    
    /// <summary>
    /// 基于HtmlAgilityPack的爬虫解析辅助类
    /// 结合火狐插件XPath Checker使用
    /// </summary>
    public class HtmlParse
    {
        #region Fields
        
        /// <summary>
        /// 需要解析的URL
        /// </summary>
        /// 时间：2016/10/26 17:20
        /// 备注：
        public readonly string HtmlAgilityURL = null;
        
        private readonly HtmlDocument htmldoc = new HtmlDocument();
        
        #endregion Fields
        
        #region Constructors
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">需要解析的URL</param>
        /// 时间：2016/10/26 17:06
        /// 备注：
        public HtmlParse(string url)
        {
            string _paramterName = "需要解析的URL";
            ValidateOperator.Begin().NotNullOrEmpty(url, _paramterName).IsURL(url, _paramterName);
            HtmlAgilityURL = url;
        }
        
        #endregion Constructors
        
        #region Methods
        
        /// <summary>
        /// 获取单个节点
        /// </summary>
        /// <param name="xPath">xPath语法</param>
        /// <returns>单个节点</returns>
        /// 时间：2016/10/26 17:17
        /// 备注：
        public HtmlNode GetNode(string xPath)
        {
            CheckedxPath(xPath);
            return htmldoc.DocumentNode.SelectSingleNode(xPath);
        }
        
        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="xPath">xPath语法</param>
        /// <returns>节点集合</returns>
        /// 时间：2016/10/26 17:17
        /// 备注：
        public HtmlNodeCollection GetNodes(string xPath)
        {
            CheckedxPath(xPath);
            return htmldoc.DocumentNode.SelectNodes(xPath);
        }
        
        /// <summary>
        /// 根据URL加载响应内容
        /// </summary>
        /// <returns>请求是否有返回</returns>
        /// 时间：2016/10/26 17:16
        /// 备注：
        public bool LoadHtmlDocument()
        {
            string _responeText = WebRequest.HttpGet(HtmlAgilityURL);
            bool _loadHmtlSuccessed = !string.IsNullOrEmpty(_responeText);
            
            if(_loadHmtlSuccessed)
            {
                htmldoc.Load(_responeText);
            }
            
            return _loadHmtlSuccessed;
        }
        
        private void CheckedxPath(string xPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(xPath, "xPath");
        }
        
        #endregion Methods
    }
}