namespace YanZhiwei.DotNet.Core.Cache
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage);
    }
}