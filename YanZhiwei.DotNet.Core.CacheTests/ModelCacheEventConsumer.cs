using System;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.Cache.Event;
using YanZhiwei.DotNet.Core.CacheTests.Model;

namespace YanZhiwei.DotNet.Core.CacheTests
{
    public class ModelCacheEventConsumer :
        IConsumer<EntityInserted<User>>,
        IConsumer<EntityUpdated<User>>,
        IConsumer<EntityDeleted<User>>
    {
        public void HandleEvent(EntityInserted<User> eventMessage)
        {
            throw new NotImplementedException();
        }
        
        public void HandleEvent(EntityUpdated<User> eventMessage)
        {
            
            throw new NotImplementedException();
        }
        
        public void HandleEvent(EntityDeleted<User> eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}