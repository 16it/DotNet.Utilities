using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using YanZhiwei.DotNet3._5.UtilitiesTests.Model;

namespace YanZhiwei.DotNet3._5.Utilities.Common.Tests
{
    [TestClass()]
    public class RecursiveSelectHelperTests
    {
        [TestMethod()]
        public void RecursiveSelectTest()
        {
            NodeData[] nodes = new NodeData[]
            {
                new NodeData
                {
                    Text = "A",
                    Children = new NodeData[]
                    {
                        new NodeData { Text = "C" },
                        new NodeData { Text = "D" },
                    }
                },
                new NodeData
                {
                    Text = "B",
                    Children = new NodeData[]
                    {
                        new NodeData
                        {
                            Text = "E",
                            Children = new NodeData[]
                            {
                                new NodeData { Text = "F" },
                            }
                        }
                    }
                }
            };
            var _finded = nodes.RecursiveSelect(node => node.Children).Where(c => c.Text == "E").FirstOrDefault();

            Assert.AreEqual("E", _finded.Text);
        }
    }

   
}