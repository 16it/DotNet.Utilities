using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;

namespace YanZhiwei.DotNet.Core.SignalR.Infrastructure
{
    /// <summary>
    /// UnityHubActivator
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hubs.IHubActivator" />
    public sealed class UnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityHubActivator"/> class.
        /// </summary>
        /// <param name="container">IUnityContainer</param>
        public UnityHubActivator(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates the specified descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns></returns>
        public IHub Create(HubDescriptor descriptor)
        {
            var hubType = descriptor.HubType;
            return hubType != null ? _container.Resolve(hubType) as IHub : null;
        }
    }
}