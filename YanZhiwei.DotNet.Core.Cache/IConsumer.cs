namespace YanZhiwei.DotNet.Core.Cache
{
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}