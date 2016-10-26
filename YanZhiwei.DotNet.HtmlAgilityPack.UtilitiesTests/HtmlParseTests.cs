using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet.HtmlAgilityPack.Utilities.Tests
{
    [TestClass()]
    public class HtmlParseTests
    {
        [TestMethod()]
        public void GetNodeTest()
        {
            HtmlParse _htmlParse = new HtmlParse("https://news.cnblogs.com/");
            _htmlParse.LoadHtmlDocument();
            HtmlNode _htmlNode = _htmlParse.GetNode("//div[@class='news_block'][@id='entry_555912']");
            Assert.IsNotNull(_htmlNode);
        }
        
        [TestMethod()]
        public void GetNodesTest()
        {
            HtmlParse _htmlParse = new HtmlParse("https://news.cnblogs.com/");
            _htmlParse.LoadHtmlDocument();
            HtmlNodeCollection _htmlNode = _htmlParse.GetNodes("//div[@class='news_block']");
            Assert.IsNotNull(_htmlNode);
        }
    }
}