﻿using System.Collections.Generic;
using YanZhiwei.DotNet.Core.CacheTests.Model;

namespace YanZhiwei.DotNet.Core.CacheTests.Service
{
    public interface IUserService
    {
        User GetById(object id);

        void Insert(User item);

        void Update(User item);

        void Delete(User item);

        IList<User> GetAll();
    }
}