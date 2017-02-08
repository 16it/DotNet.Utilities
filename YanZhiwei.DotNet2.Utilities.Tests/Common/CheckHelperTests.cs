using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YanZhiwei.DotNet2.Utilities.Tests;

namespace YanZhiwei.DotNet2.Utilities.Common.Tests
{
    [TestClass()]
    public class CheckHelperTests
    {
        [TestMethod()]
        public void IsMacAddrTest()
        {
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-8E-01-35-86"));
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-BA-00-50-26"));
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-C7-01-BB-CE"));
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-C1-01-BE-AC"));
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-88-00-50-0E"));
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-C1-01-BF-AF"));
            Assert.IsTrue(CheckHelper.IsMacAddr("F4-82-00-C8-01-BB-3F"));
        }
        
        [TestMethod()]
        public void InRangeTest()
        {
            bool _actual = CheckHelper.InRange("2", 1, 5);
            Assert.IsTrue(_actual);
            DateTime _start = new DateTime(2009, 12, 9, 10, 0, 0); //10 o'clock
            DateTime _end = new DateTime(2009, 12, 10, 12, 0, 0); //12 o'clock
            DateTime _now = new DateTime(2009, 12, 10, 11, 0, 0);
            Assert.IsTrue(CheckHelper.InRange(_now, _start, _end, true));
            _now = new DateTime(2009, 12, 10, 12, 0, 0);
            Assert.IsFalse(CheckHelper.InRange(_now, _start, _end, false));
        }
        
        [TestMethod()]
        public void IsNumberTest()
        {
            bool _actual = CheckHelper.IsNumber("abc");
            Assert.IsFalse(_actual);
        }
        
        [TestMethod()]
        public void NotNullTest()
        {
            bool _actual = CheckHelper.NotNull("abc");
            Assert.IsTrue(_actual);
        }
        
        [TestMethod()]
        public void IsEmailTest()
        {
            bool _actual = CheckHelper.IsEmail("Yan.Zhiwei@hotmail.com");
            Assert.IsTrue(_actual);
        }
        
        [TestMethod()]
        public void IsURLTest()
        {
            bool _actual = CheckHelper.IsURL("www.cnblogs.com/yan-zhiwei");
            Assert.IsTrue(_actual);
            bool _actual_error = CheckHelper.IsURL("www.cnb");
            Assert.IsFalse(_actual_error);
        }
        
        [TestMethod()]
        public void IsFilePathTest()
        {
            bool _actual = CheckHelper.IsFilePath(@"C:\alipay\log.txt");
            Assert.IsTrue(_actual);
            bool _actual_error = CheckHelper.IsFilePath("alipay");
            Assert.IsFalse(_actual_error);
        }
        
        [TestMethod()]
        public void IsPoseCodeTest()
        {
            bool _actual = CheckHelper.IsPoseCode("412000");
            Assert.IsTrue(_actual);
            bool _actual_error = CheckHelper.IsPoseCode("alipay");
            Assert.IsFalse(_actual_error);
        }
        
        [TestMethod()]
        public void IsBase64Test()
        {
            bool _actual = CheckHelper.IsBase64("6KiA5b+X5Lyf");
            Assert.IsTrue(_actual);
            bool _actual_error = CheckHelper.IsBase64("yanzhiwei");
            Assert.IsFalse(_actual_error);
        }
        
        [TestMethod()]
        public void IsImageFormatTest()
        {
            byte[] _data = ImageHelper.ToBytes(TestResource.cat);
            bool _actual = CheckHelper.IsImageFormat(_data);
            Assert.IsTrue(_actual);
            byte[] _data_txt = TestResource.book;
            bool _actual_error = CheckHelper.IsImageFormat(_data_txt);
            Assert.IsFalse(_actual_error);
        }
        
        [TestMethod()]
        public void IsDateTest()
        {
            bool _actual = CheckHelper.IsDate("2014年12月12日");
            Assert.IsTrue(_actual);
        }
        
        [TestMethod()]
        public void IsValidPortTest()
        {
            bool _actual = CheckHelper.IsValidPort("8060");
            Assert.IsTrue(_actual);
        }
        
        [TestMethod()]
        public void IsLocalIp4Test()
        {
            bool _actual = CheckHelper.IsLocalIp4("192.168.1.149");
            Assert.IsTrue(_actual);
        }
        
        [TestMethod()]
        public void IsBinaryCodedDecimalTest()
        {
            Assert.IsFalse(CheckHelper.IsBinaryCodedDecimal(string.Empty));
            Assert.IsFalse(CheckHelper.IsBinaryCodedDecimal("012"));
            Assert.IsTrue(CheckHelper.IsBinaryCodedDecimal("01"));
            Assert.IsTrue(CheckHelper.IsBinaryCodedDecimal("3425"));
        }
        
        [TestMethod()]
        public void IsIp46AddressTest()
        {
            Assert.IsTrue(CheckHelper.IsIp46Address("192.168.1.1:8060"));
            Assert.IsTrue(CheckHelper.IsIp46Address("[2001:0DB8:02de::0e13]:9010"));
        }
    }
}