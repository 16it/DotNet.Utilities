using System;
using System.Diagnostics;
using YanZhiwei.DotNet.Core.Service.Events;
using YanZhiwei.DotNet.Core.ServiceTests.Model;

namespace YanZhiwei.DotNet.Core.ServiceTests.Events
{
    public class ModelCacheEventConsumer : IConsumer<EntityInserted<User>>,
        IConsumer<EntityUpdated<User>>,
        IConsumer<EntityDeleted<User>>
    {
        public void HandleEvent(EntityInserted<User> eventMessage)
        {
            Debug.WriteLine("用户数据插入");
        }

        public void HandleEvent(EntityUpdated<User> eventMessage)
        {
            Debug.WriteLine("用户更新");
        }

        public void HandleEvent(EntityDeleted<User> eventMessage)
        {
            Debug.WriteLine("用户删除");
        }
    }
}