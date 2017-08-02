using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;

namespace YanZhiwei.DotNet3._5.Utilities.TypeConverter.Tests
{
    [TestClass()]
    public class GenericDictionaryTypeConverterTests
    {
        [TestMethod()]
        public void CanConvertFromTest()
        {
            var converterInt = TypeDescriptor.GetConverter(typeof(Dictionary<int, string>));
            Assert.AreEqual(converterInt.GetType(), typeof(GenericDictionaryTypeConverter<int, string>));
        }

        [TestMethod()]
        public void ConvertFromTest()
        {
            var itemsInt = "10,aa;20,bb;30,cc;40,dd;50,ff";
            var converterInt = TypeDescriptor.GetConverter(typeof(Dictionary<int, string>));
            var _intConvertActual = converterInt.ConvertFrom(itemsInt) as Dictionary<int, string>;
            Assert.IsNotNull(_intConvertActual);
            Assert.AreEqual(5, _intConvertActual.Count);
        }

        [TestMethod()]
        public void ConvertToTest()
        {
            var itemsInt = new Dictionary<int, string>();
            itemsInt.Add(10, "aa");
            itemsInt.Add(20, "bb");
            itemsInt.Add(30, "cc");
            itemsInt.Add(40, "dd");
            itemsInt.Add(50, "ff");

            var converterInt = TypeDescriptor.GetConverter(itemsInt.GetType());
            var _intConvertActual = converterInt.ConvertTo(itemsInt, typeof(string)) as string;
            Assert.IsNotNull(_intConvertActual);
            Assert.AreEqual(_intConvertActual, "10, aa;20, bb;30, cc;40, dd;50, ff");
        }

        [TestInitialize]
        public void Init()
        {
            TypeDescriptor.AddAttributes(typeof(Dictionary<int, string>),
              new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<int, string>)));
        }
    }
}