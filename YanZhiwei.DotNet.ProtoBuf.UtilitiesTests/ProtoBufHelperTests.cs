using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace YanZhiwei.DotNet.ProtoBuf.Utilities.Tests
{
    [TestClass()]
    public class ProtoBufHelperTests
    {
        private static List<Person> personList = null;
        private static string serializeFilePath = @"D:\ProtoBufHelperTests.bin";
        
        [TestInitialize]
        public void Init()
        {
            personList = new List<Person>();
            
            for(int i = 0; i < 1000; i++)
            {
                personList.Add(new Person()
                {
                    Name = "churenyouzi",
                    Addr = "zhuzhou"
                });
            }
        }
        
        [TestMethod()]
        public void SerializeTest()
        {
            Assert.IsNotNull(ProtoBufHelper.Serialize(personList));
            ProtoBufHelper.Serialize(personList, serializeFilePath);
            bool _actual = File.Exists(serializeFilePath);
            Assert.IsTrue(_actual);
        }
        
        [TestCleanup]
        public void End()
        {
            if(File.Exists(serializeFilePath))
                File.Delete(serializeFilePath);
        }
    }
    
    [ProtoContract]
    public class Person
    {
        public string Name
        {
            get;
            set;
        }
        
        public string Addr
        {
            get;
            set;
        }
    }
}