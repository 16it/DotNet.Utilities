using System.Collections.Generic;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.CacheTests.Model;

namespace YanZhiwei.DotNet.Core.CacheTests.Service
{
    public class UserService : IUserService
    {
        private const string CacheKey = "UserServer";

        public void Delete(User item)
        {
            using (var dbContext = new AccountDbContext())
            {
                dbContext.Delete<User>(item);
            }
        }

        public IList<User> GetAll()
        {
            using (var dbContext = new AccountDbContext())
            {
                return dbContext.Users.ToCacheList(CacheKey);
            }
        }

        public User GetById(object id)
        {
            return CacheHelper.Get(string.Format("{0}_{1}", CacheKey, id), () =>
             {
                 using (var dbContext = new AccountDbContext())
                 {
                     return dbContext.Find<User>(id);
                 }
             });
        }

        public void Insert(User item)
        {
            using (var dbContext = new AccountDbContext())
            {
                dbContext.Insert<User>(item);
            }
        }

        public void Update(User item)
        {
            using (var dbContext = new AccountDbContext())
            {
                dbContext.Update<User>(item);
            }
        }
    }
}