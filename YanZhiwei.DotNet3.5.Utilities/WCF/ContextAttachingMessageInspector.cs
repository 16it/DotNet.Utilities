using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using YanZhiwei.DotNet3._5.Utilities.CallContext;

namespace YanZhiwei.DotNet3._5.Utilities.WCF
{
    /// <summary>
    /// 在client端通过ClientMessageInspector将context信息存储到request message header中
    /// client端和service端，可以通过MessageInspector对request message或者reply message (incoming message或者outgoings message)进行检验。
    /// MessageInspector可以对MessageHeader进行自由的添加、修改和删除。
    /// 在service端的MessageInspector被称为DispatchMessageInspector
    /// 在client端被称为ClientMessageInspector。
    /// </summary>
    /// <seealso cref="System.ServiceModel.Dispatcher.IClientMessageInspector" />
    public class ContextAttachingMessageInspector : IClientMessageInspector
    {
        /// <summary>
        ///是否支持双向传递
        /// </summary>
        public bool IsBidirectional
        {
            get;
            set;
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public ContextAttachingMessageInspector() : this(false) { }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isBidirectional">是否支持双向传递</param>
        public ContextAttachingMessageInspector(bool isBidirectional)
        {
            this.IsBidirectional = IsBidirectional;
        }
        
        /// <summary>
        /// 在收到回复消息之后将它传递回客户端应用程序之前，启用消息的检查或修改。
        /// </summary>
        /// <param name="reply">要转换为类型并交回给客户端应用程序的消息。</param>
        /// <param name="correlationState">关联状态数据。</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if(IsBidirectional)
            {
                return;
            }
            
            if(reply.Headers.FindHeader(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace) < 0)
            {
                return;
            }

            WCFCallContext context = reply.Headers.GetHeader<WCFCallContext>(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace);
            
            if(context == null)
            {
                return;
            }

            WCFCallContext.Current = context;
        }
        
        /// <summary>
        /// 在将请求消息发送到服务之前，启用消息的检查或修改。
        /// </summary>
        /// <param name="request">要发送给服务的消息。</param>
        /// <param name="channel">客户端对象通道。</param>
        /// <returns>
        /// 作为 <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> 方法的 <paramref name="correlationState " />参数返回的对象。如果不使用相关状态，则为 null。最佳做法是将它设置为 <see cref="T:System.Guid" />，以确保没有两个相同的 <paramref name="correlationState" /> 对象。
        /// </returns>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            MessageHeader<WCFCallContext> contextHeader = new MessageHeader<WCFCallContext>(WCFCallContext.Current);
            request.Headers.Add(contextHeader.GetUntypedHeader(WCFCallContext.ContextHeaderLocalName, WCFCallContext.ContextHeaderNamespace));
            return null;
        }
    }
}