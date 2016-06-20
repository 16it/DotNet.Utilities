using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YanZhiwei.DotNet2.Utilities.Core.Tests
{
    [TestClass()]
    public class ByteArrayBuilderTests
    {
        [TestMethod()]
        public void ToArrayTest()
        {
            using (ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append((byte)0x68);
                builder.Append((byte)0x01);
                builder.Append((byte)0x01);
                builder.Append((byte)0x02);
                builder.Append(new byte[2] { 0x03, 0x04 });
                byte[] _data = builder.ToArray();
                byte[] _expected = new byte[6] { 0x68, 0x01, 0x01, 0x02, 0x03, 0x04 };
                CollectionAssert.AreEqual(_expected, _data);
            }
        }

        [TestMethod()]
        public void ToStringTest()
        {
            using (ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append((byte)0x68);
                builder.Append((byte)0x01);
                builder.Append((byte)0x01);
                builder.Append((byte)0x02);
                builder.Append(new byte[2] { 0x03, 0x04 });
                string _data = builder.ToString();
                string _expected = "68 01 01 02 03 04";
                Assert.AreEqual(_expected, _data);
            }
        }
    }
}