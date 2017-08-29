using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet.Core.Service.Events;
using YanZhiwei.DotNet.Core.ServiceTests.Model;

namespace YanZhiwei.DotNet.Core.ServiceTests.Events
{
    [TestClass()]
    public class ModelCacheEventConsumer : IConsumer<EntityInserted<User>>,
        IConsumer<EntityUpdated<User>>,
        IConsumer<EntityDeleted<User>>
    {
        public void HandleEvent(EntityInserted<User> eventMessage)
        {
            Assert.AreEqual("YanZhiwei1", eventMessage.Entity.LoginName);
        }

        public void HandleEvent(EntityUpdated<User> eventMessage)
        {
            Assert.AreEqual("YanZhiwei3", eventMessage.Entity.LoginName);
        }

        public void HandleEvent(EntityDeleted<User> eventMessage)
        {
            Assert.AreEqual("YanZhiwei2", eventMessage.Entity.LoginName);
        }
    }
}