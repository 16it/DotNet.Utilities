using System.Diagnostics;
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
        public ModelCacheEventConsumer() {

        }

        public void HandleEvent(EntityInserted<User> eventMessage)
        {
            Debug.WriteLine("EntityInserted<User>");
        }

        public void HandleEvent(EntityUpdated<User> eventMessage)
        {
            Debug.WriteLine("EntityUpdated<User>");
        }

        public void HandleEvent(EntityDeleted<User> eventMessage)
        {
            Debug.WriteLine("EntityDeleted<User>");
        }
    }
}