using Castle.DynamicProxy;
using System;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.Core.Service;
using YanZhiwei.DotNet3._5.Utilities.Service;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace YanZhiwei.DotNet.Core.ServiceTests
{
    public class AdventureWorksServiceProxy : WcfServiceProxy, IServiceBase
    {
        protected override int MaxReceivedMessageSize => 2147483647;

        protected override TimeSpan Timeout => TimeSpan.FromMinutes(10);

        protected override string Url => "http://localhost:8092/AdventureWorksService/AdventureWorks";

        public T CreateService<T, F>()
            where T : class
            where F : IInterceptor, new()
        {
            var key = string.Format("{0}-{1}", typeof(T), Url);
            return CacheHelper.Get<T>(key, () =>
            {
                return base.CreateBasicHttpService<T>();
            });
        }

        public override void AddBehaviors(KeyedByTypeCollection<IEndpointBehavior> behaviors)
        {
            throw new NotImplementedException();
        }
    }
}