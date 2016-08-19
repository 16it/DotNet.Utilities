namespace YanZhiwei.DotNet.Core.Service
{
    /// <summary>
    /// ServiceFactory
    /// </summary>
    public abstract class ServiceFactory
    {
        public abstract T CreateService<T>() where T : class;
    }
}