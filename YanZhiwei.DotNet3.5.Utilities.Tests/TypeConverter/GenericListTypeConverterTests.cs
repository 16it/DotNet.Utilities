using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;

namespace YanZhiwei.DotNet3._5.Utilities.TypeConverter.Tests
{
    [TestClass()]
    public class GenericListTypeConverterTests
    {
        [TestMethod()]
        public void CanConvertFromTest()
        {
            var converterInt = TypeDescriptor.GetConverter(typeof(List<int>));
            Assert.AreEqual(converterInt.GetType(), typeof(GenericListTypeConverter<int>));

            var converterString = TypeDescriptor.GetConverter(typeof(List<string>));
            Assert.AreEqual(converterString.GetType(), typeof(GenericListTypeConverter<string>));
        }

        [TestMethod()]
        public void ConvertFromTest()
        {
            var itemsInt = "10,20,30,40,50";
            var converterInt = TypeDescriptor.GetConverter(typeof(List<int>));
            var _intConvertActual = converterInt.ConvertFrom(itemsInt) as IList<int>;
            Assert.IsNotNull(_intConvertActual);
            Assert.AreEqual(5, _intConvertActual.Count);

            var itemsString = "foo, bar, day";
            var converterString = TypeDescriptor.GetConverter(typeof(List<string>));
            var _stringConvertActual = converterString.ConvertFrom(itemsString) as List<string>;
            Assert.IsNotNull(_stringConvertActual);
            Assert.AreEqual(3, _stringConvertActual.Count);
        }

        [TestMethod()]
        public void ConvertToTest()
        {
            var itemsInt = new List<int> { 10, 20, 30, 40, 50 };
            var converterInt = TypeDescriptor.GetConverter(itemsInt.GetType());
            var _intConvertActual = converterInt.ConvertTo(itemsInt, typeof(string)) as string;
            Assert.IsNotNull(_intConvertActual);
            Assert.AreEqual(_intConvertActual, "10,20,30,40,50");

            var itemsString = new List<string> { "foo", "bar", "day" };
            var converterString = TypeDescriptor.GetConverter(itemsString.GetType());
            var _stringConvertActual = converterString.ConvertTo(itemsString, typeof(string)) as string;
            Assert.IsNotNull(_stringConvertActual);
            Assert.AreEqual(_stringConvertActual, "foo,bar,day");
        }

        [TestInitialize]
        public void Init()
        {
            TypeDescriptor.AddAttributes(typeof(List<int>),
              new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            TypeDescriptor.AddAttributes(typeof(List<string>),
                new TypeConverterAttribute(typeof(GenericListTypeConverter<string>)));
        }
    }
}