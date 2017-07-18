using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace YanZhiwei.DotNet3._5.Utilities.WCF
{
    /// <summary>
    /// WCF调用上下文 MessageInspector
    /// </summary>
    internal sealed class CallContextMessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        //在client端和service端都有自己的MessageInspector，client端叫做ClientMessageInspector，
        //实现System.ServiceModel.Dispatcher.IClientMessageInspector interface，
        //而service端叫做 DispatchMessageInspector， 实现了System.ServiceModel.Dispatcher.IDispatchMessageInspector interface。
        //DispatchMessageInspector允许你在request message交付到具体的DispatchOperation付诸执行之前或者reply message返回client之前对incoming message/outgoing message进行检验、修改或者其他一些基于message的操作；

        //MessageInspector使用相当广范，比如：我们可以定义自己的MessageInspector实现Logging功能；
        //或者在Client端通过ClientMessageInspector添加一些与业务无关的context信息，并在service通过DispatchMessageInspector将其取出。

        //在Client端对 IClientMessageInspector 进行实现
        //在service端对IDispatchMessageInspector进行实现

        /// <summary>
        /// Afters the receive reply.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <param name="correlationState">State of the correlation.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Afters the receive request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="instanceContext">The instance context.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Befores the send reply.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <param name="correlationState">State of the correlation.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Befores the send request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="channel">The channel.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            throw new NotImplementedException();
        }
    }
}