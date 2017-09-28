using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using YanZhiwei.DotNet.AutoMapper.UtilitiesTests;

namespace YanZhiwei.DotNet.AutoMapper.Utilities.Tests
{
    [TestClass()]
    public class MapperHelperTests
    {
        [TestMethod()]
        public void MapToTest()
        {
            User _user = new User();
            _user.Id = 1;
            _user.Name = "chruenyouzi";
            _user.PassWord = "123";
            var _userDto = AutoMapperHelper.MapTo<User, UserDto>(_user);
            Assert.AreEqual(_userDto.Name, "chruenyouzi");
            Assert.AreEqual(_userDto.PassWord, "123");
            _userDto = AutoMapperHelper.MapTo<User, UserDto>(_user);
            Assert.AreEqual(_userDto.Name, "chruenyouzi");
            Assert.AreEqual(_userDto.PassWord, "123");
            _userDto = AutoMapperHelper.MapTo<User, UserDto, UserProfile>(_user);
            Assert.AreEqual(_userDto.Name, "chruenyouzi");
            Assert.AreEqual(_userDto.PassWord, "123");
            _userDto = AutoMapperHelper.MapTo<User, UserDto, UserProfile>(_user);
            Assert.AreEqual(_userDto.Name, "chruenyouzi");
            Assert.AreEqual(_userDto.PassWord, "123");
        }
    }
    
    public class User
    {
        [Key]
        public long Id
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }
        
        [DataType(DataType.Password)]
        public string PassWord
        {
            get;
            set;
        }
    }
    
    public class UserDto
    {
        public string Name
        {
            get;
            set;
        }
        
        public string PassWord
        {
            get;
            set;
        }
    }
}