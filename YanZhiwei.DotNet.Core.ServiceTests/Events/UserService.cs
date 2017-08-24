using System.Collections.Generic;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.ServiceTests.Model;

namespace YanZhiwei.DotNet.Core.ServiceTests.Events
{
    public class UserService : IUserService
    {
        private IEventPublisher _eventPublisher;
        private const string CacheKey = "UserServer";

        public UserService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public UserService() {

        }
        public void Delete(User item)
        {
            _eventPublisher.EntityDeleted<User>(item);
        }

        public IList<User> GetAll()
        {
            return null;
        }

        public User GetById(object id)
        {
            return null;
        }

        public void Insert(User item)
        {
            _eventPublisher.EntityInserted<User>(item);
        }

        public void Update(User item)
        {
            _eventPublisher.EntityUpdated<User>(item);
        }
    }
}