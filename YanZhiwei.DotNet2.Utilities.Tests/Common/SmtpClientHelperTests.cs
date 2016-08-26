using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Operator;

namespace YanZhiwei.DotNet2.Utilities.DataOperator.Tests
{
    [TestClass()]
    public class SmtpClientHelperTests
    {
        [TestMethod()]
        public void SendTest()
        {
            SmtpServer _server = new SmtpServer("smtp.163.com", "18501600184@163.com", "******");
            SmtpClientOperator _client = new SmtpClientOperator(_server, "楚人游子", DateTime.Now.FormatDate(1) + "测试", DateTime.Now.FormatDate(1) + "单元测试", new string[] { "churenyouzi@outlook.com" });
            _client.Send();
        }
    }
}