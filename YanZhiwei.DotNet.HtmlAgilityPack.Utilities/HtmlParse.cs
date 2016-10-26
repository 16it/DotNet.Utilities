namespace YanZhiwei.DotNet.HtmlAgilityPack.Utilities
{
    using DotNet2.Utilities.Operator;
    using DotNet3._5.Utilities.WebForm.Core;
    using global::HtmlAgilityPack;
    using System.Xml.XPath;
    
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
            try
            {
                CheckedxPath(xPath);
                return htmldoc.DocumentNode.SelectSingleNode(xPath);
            }
            catch(XPathException)
            {
                return null;
            }
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
            try
            {
                CheckedxPath(xPath);
                return htmldoc.DocumentNode.SelectNodes(xPath);
            }
            catch(XPathException)
            {
                return null;
            }
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
                htmldoc.LoadHtml(_responeText);
            }
            
            return _loadHmtlSuccessed;
        }
        
        private void CheckedxPath(string xPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(xPath, "xPath");
        }
        
        #endregion Methods
        
        #region Other
        
        //1.以/开头的是从根节点开始选取，以//开头的是模糊选取，而不考虑它们的位置
        //2.可以使用属性来定位要选取的节点或节点集合 比如//span[@class="time"] 就是选择文档中所有class="time"的span元素。
        //3.节点集合中的某一个使用[i] 的方式选取 比如 //span[@class="time"][1] 就是选择文档中所有class="time"的span元素中的第一个span。注意在这里选择节点的索引是从1开始的，而不是0
        //4.使用|  来做容错选择，比如一个网页中某个数据可能在<div class="a1"></div>中 也可能在<div class="a2"></div> 这时就可以用 //div[@class="a1"]|//div[@class="a2"] 作为XPath
        //5.XPath中需要用到的引号 可以使用单引号  因为C#中字符串需要用双引号，XPath中需要引号的使用单引号即可，这样不用转义了。
        
        #endregion Other
    }
}