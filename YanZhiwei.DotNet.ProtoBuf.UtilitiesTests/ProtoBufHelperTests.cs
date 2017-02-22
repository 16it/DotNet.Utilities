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
        private static string serializeFilePath2 = @"D:\ProtoBufHelperTests2.bin";
        
        [TestInitialize]
        public void Init()
        {
            personList = new List<Person>();
            
            for(int i = 0; i < 1000; i++)
            {
                personList.Add(new Person()
                {
                    Id = 12345,
                    Name = "Fred",
                    Address = new Address
                    {
                        Line1 = "Flat 1",
                        Line2 = "The Meadows"
                    }
                });
            }
        }
        
        [TestMethod()]
        public void SerializeTest()
        {
            Assert.IsNotNull(ProtoBufHelper.Serialize(personList));
            ProtoBufHelper.Serialize(personList, serializeFilePath);
            bool _actual = File.Exists(serializeFilePath);
            ProtoBufHelper.Serialize<Person>(personList[0], serializeFilePath2);
            _actual = File.Exists(serializeFilePath);
            Assert.IsTrue(_actual);
            PersonSample _personSample = new PersonSample();
            _personSample.Address = "zhuzhou";
            _personSample.Name = "yanzhiwei";
            byte[] _buffer = ProtoBufHelper.Serialize(_personSample);
            _actual = _buffer != null && _buffer.Length > 0;
            Assert.IsTrue(_actual);
        }
        
        [TestCleanup]
        public void End()
        {
            if(File.Exists(serializeFilePath))
                File.Delete(serializeFilePath);
                
            if(File.Exists(serializeFilePath2))
                File.Delete(serializeFilePath2);
        }
        
        [TestMethod()]
        public void DeserializeTest()
        {
            byte[] _buffer = ProtoBufHelper.Serialize(personList);
            List<Person> _actual = ProtoBufHelper.Deserialize<List<Person>>(_buffer);
            CollectionAssert.AreNotEqual(_actual, personList);
            _buffer = ProtoBufHelper.Serialize(personList[0]);
            Person _actualPerson = ProtoBufHelper.Deserialize<Person>(_buffer);
            Assert.IsNotNull(_actualPerson);
            ProtoBufHelper.Serialize(personList, serializeFilePath);
            _actual = ProtoBufHelper.Deserialize<List<Person>>(serializeFilePath);
            CollectionAssert.AreNotEqual(_actual, personList);
        }
    }
    
    [ProtoContract]
    internal class PersonSample
    {
        [ProtoMember(1)]
        public string Name
        {
            get;
            set;
        }
        
        [ProtoMember(2)]
        public string Address
        {
            get;
            set;
        }
    }
    
    [ProtoContract]
    internal class Person
    {
        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }
        
        [ProtoMember(2)]
        public string Name
        {
            get;
            set;
        }
        
        [ProtoMember(3)]
        public Address Address
        {
            get;
            set;
        }
    }
    
    [ProtoContract]
    internal class Address
    {
        [ProtoMember(1)]
        public string Line1
        {
            get;
            set;
        }
        
        [ProtoMember(2)]
        public string Line2
        {
            get;
            set;
        }
    }
}