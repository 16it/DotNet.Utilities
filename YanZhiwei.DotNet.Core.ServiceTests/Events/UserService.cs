using System;
using YanZhiwei.DotNet.Core.Service.Events;
using YanZhiwei.DotNet.Core.ServiceTests.Model;

namespace YanZhiwei.DotNet.Core.ServiceTests.Events
{
    public class UserService : IUserService
    {
        private IEventPublisher _eventPublisher;
        //private const string CacheKey = "UserServer";

        public UserService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public string DeleteUser(string Name)
        {
            _eventPublisher.EntityDeleted<User>(CreateTestUser(Name));
            return Name;
        }

        public string GetUserName(string Name)
        {
            return Name;
        }

        public string InsertUser(string Name)
        {
            _eventPublisher.EntityInserted<User>(CreateTestUser(Name));
            return Name;
        }

        private User CreateTestUser(string name)
        {
            User _newUser = new User();
            _newUser.CreateTime = DateTime.Now;
            _newUser.Email = "churenyouzi@Outlook.com";
            _newUser.ID = 1;
            _newUser.IsActive = true;
            _newUser.LoginName = name;
            _newUser.Mobile = "17602110123";
            _newUser.Password = "123456";
            return _newUser;
        }

        public string UpdateUser(string Name)
        {
            _eventPublisher.EntityUpdated<User>(CreateTestUser(Name));
            return Name;
        }
    }
}