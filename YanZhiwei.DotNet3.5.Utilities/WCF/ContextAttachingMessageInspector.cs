using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using YanZhiwei.DotNet3._5.Utilities.CallContext;

namespace YanZhiwei.DotNet3._5.Utilities.WCF
{
    /// <summary>
    /// WCF调用上下文 MessageInspector
    /// </summary>
    internal sealed class ContextAttachingMessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        //在client端和service端都有自己的MessageInspector，client端叫做ClientMessageInspector，
        //实现System.ServiceModel.Dispatcher.IClientMessageInspector interface，
        //而service端叫做 DispatchMessageInspector， 实现了System.ServiceModel.Dispatcher.IDispatchMessageInspector interface。
        //DispatchMessageInspector允许你在request message交付到具体的DispatchOperation付诸执行之前或者reply message返回client之前对incoming message/outgoing message进行检验、修改或者其他一些基于message的操作；

        //MessageInspector使用相当广范，比如：我们可以定义自己的MessageInspector实现Logging功能；
        //或者在Client端通过ClientMessageInspector添加一些与业务无关的context信息，并在service通过DispatchMessageInspector将其取出。

        //在Client端对 IClientMessageInspector 进行实现
        //在service端对IDispatchMessageInspector进行实现

        #region IClientMessageInspector                          
        /// <summary>
        /// 在收到回复消息之后将它传递回客户端应用程序之前，启用消息的检查或修改。
        /// </summary>
        /// <param name="reply">要转换为类型并交回给客户端应用程序的消息。</param>
        /// <param name="correlationState">关联状态数据。</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        /// <summary>
        /// 在将请求消息发送到服务之前，启用消息的检查或修改。
        /// </summary>
        /// <param name="request">要发送给服务的消息。</param>
        /// <param name="channel">客户端对象通道。</param>
        /// <returns>
        /// 作为 <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> 方法的 <paramref name="correlationState " />参数返回的对象。如果不使用相关状态，则为 null。最佳做法是将它设置为 <see cref="T:System.Guid" />，以确保没有两个相同的 <paramref name="correlationState" /> 对象。
        /// </returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var _contextHeader = new MessageHeader<WCFCallContext>(WCFCallContext.Current);
            request.Headers.Add(_contextHeader.GetUntypedHeader(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace));
            return null;
        }

        #endregion IClientMessageInspector



        #region IDispatchMessageInspector

        /// <summary>
        /// Afters the receive request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="instanceContext">The instance context.</param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var _context = request.Headers.GetHeader<WCFCallContext>(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace);
            WCFCallContext.Current = _context;
            return null;
        }

        /// <summary>
        /// Befores the send reply.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <param name="correlationState">State of the correlation.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion IDispatchMessageInspector
    }
}