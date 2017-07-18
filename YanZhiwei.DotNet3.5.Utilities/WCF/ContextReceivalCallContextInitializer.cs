using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using YanZhiwei.DotNet3._5.Utilities.CallContext;

namespace YanZhiwei.DotNet3._5.Utilities.WCF
{
   
    public class ContextReceivalCallContextInitializer : ICallContextInitializer
    {
        public bool IsBidirectional { get; set; }

        public ContextReceivalCallContextInitializer() : this(false)
        {
        }

        public ContextReceivalCallContextInitializer(bool isBidirectional)
        {
            this.IsBidirectional = isBidirectional;
        }
        /// <summary>
        /// 实现它来参与清理调用该操作的线程。
        /// </summary>
        /// <param name="correlationState">从 <see cref="M:System.ServiceModel.Dispatcher.ICallContextInitializer.BeforeInvoke(System.ServiceModel.InstanceContext,System.ServiceModel.IClientChannel,System.ServiceModel.Channels.Message)" /> 方法返回的关联对象。</param>
        public void AfterInvoke(object correlationState)
        {
            //果需要双向传递，则通过AfterInvoke方法将context保存到reply message的header中被送回client端。 
            if (!this.IsBidirectional)
            {
                return;
            }

            WCFCallContext context = correlationState as WCFCallContext;
            if (context == null)
            {
                return;
            }
            MessageHeader<WCFCallContext> contextHeader = new MessageHeader<WCFCallContext>(context);
            OperationContext.Current.OutgoingMessageHeaders.Add(contextHeader.GetUntypedHeader(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace));
            WCFCallContext.Current = null;
        }

        /// <summary>
        /// 实现它来参与初始化操作线程。
        /// </summary>
        /// <param name="instanceContext">操作的服务实例。</param>
        /// <param name="channel">客户端通道。</param>
        /// <param name="message">传入消息。</param>
        /// <returns>
        /// 作为 <see cref="M:System.ServiceModel.Dispatcher.ICallContextInitializer.AfterInvoke(System.Object)" /> 方法的参数传回的关联对象。
        /// </returns>
        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, System.ServiceModel.Channels.Message message)
        {
            //BeforeInvoke中通过local name和namespace提取context对应的message header，并设置当前的ApplicationContext
            WCFCallContext context = message.Headers.GetHeader<WCFCallContext>(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace);
            if (context == null) { return null; }
            WCFCallContext.Current = context;
            return WCFCallContext.Current;
        }
    }
}