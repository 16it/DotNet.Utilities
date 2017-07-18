using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace YanZhiwei.DotNet3._5.Utilities.WCF
{
    public class ContextPropagationBehavior : IEndpointBehavior
    {

        public bool IsBidirectional { get; set; }
        public ContextPropagationBehavior() : this(false) { }
        public ContextPropagationBehavior(bool isBidirectional)
        {
            this.IsBidirectional = isBidirectional;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ContextAttachingMessageInspector(this.IsBidirectional));
        }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
            {
                operation.CallContextInitializers.Add(new ContextReceivalCallContextInitializer(this.IsBidirectional));
            }
        }
        public void Validate(ServiceEndpoint endpoint) { }
    }
}
