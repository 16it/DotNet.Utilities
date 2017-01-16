using YanZhiwei.DotNet.MyXls.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using YanZhiwei.DotNet2.Utilities.Common;
using System.Data;

namespace YanZhiwei.DotNet.MyXls.Utilities.Tests
{
    [TestClass()]
    public class MyxlsExcelTests
    {
        private List<Person> personList = null;
        
        [TestInitialize]
        public void Init()
        {
            personList = new List<Person>();
            
            for(int i = 0; i < 255; i++)
            {
                personList.Add(new Person()
                {
                    Address = RandomHelper.NextString(10, false),
                    Age = (short)RandomHelper.NextNumber(0, 100),
                    Name = RandomHelper.NextString(4, false)
                });
            }
        }
        
        [TestMethod()]
        public void ToExecelTest()
        {
            string _excelpath = string.Format(@"D:\ToExecelTest_{0}.xls", DateTime.Now.FormatDate(12));
            
            if(File.Exists(_excelpath))
                File.Delete(_excelpath);
                
            MyxlsExcel.ToExecel<Person>(personList, _excelpath, "PersonInfo");
            bool _actual = File.Exists(_excelpath);
            Assert.IsTrue(_actual);
        }
        
        [TestMethod()]
        public void ToDataTableTest()
        {
            DataTable _actual = MyxlsExcel.ToDataTable(@"D:\为制作DB而临时产生的文件_愚园路246弄.xls", 0);
            Assert.IsNotNull(_actual);
        }
    }
    
    public class Person
    {
        [DisplayName("姓名")]
        public string Name
        {
            get;
            set;
        }
        
        [DisplayName("年龄")]
        public short Age
        {
            get;
            set;
        }
        
        [DisplayName("住址")]
        public string Address
        {
            get;
            set;
        }
    }
}