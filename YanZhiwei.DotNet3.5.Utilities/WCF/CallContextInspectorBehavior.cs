using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace YanZhiwei.DotNet3._5.Utilities.WCF
{
    /// <summary>
    /// WCF调用上下文 InspectorBehavior
    /// </summary>
    /// <seealso cref="System.ServiceModel.Configuration.BehaviorExtensionElement" />
    /// <seealso cref="System.ServiceModel.Description.IEndpointBehavior" />
    public sealed class CallContextInspectorBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        /// <summary>
        /// Gets the type of the behavior.
        /// </summary>
        /// <value>
        /// The type of the behavior.
        /// </value>
        public override Type BehaviorType
        {
            get { return typeof(CallContextInspectorBehavior); }
        }

        /// <summary>
        /// Creates the behavior.
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new CallContextInspectorBehavior();
        }

        #region IEndpointBehavior

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="bindingParameters">The binding parameters.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Applies the client behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new CallContextMessageInspector());
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CallContextMessageInspector());
        }

        /// <summary>
        /// Validates the specified endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion IEndpointBehavior
    }
}