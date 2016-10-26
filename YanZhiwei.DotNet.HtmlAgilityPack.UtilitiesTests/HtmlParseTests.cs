using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.HtmlAgilityPack.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace YanZhiwei.DotNet.HtmlAgilityPack.Utilities.Tests
{
    [TestClass()]
    public class HtmlParseTests
    {
        [TestMethod()]
        public void GetGetNodeTest()
        {
            HtmlParse _htmlParse = new HtmlParse("https://news.cnblogs.com/");
            _htmlParse.LoadHtmlDocument();
            HtmlNode _htmlNode = _htmlParse.GetNode("id('entry_555912')/x:div[2]/x:h2/x:a");
        }
    }
}