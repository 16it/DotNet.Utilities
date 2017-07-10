namespace YanZhiwei.DotNet.Core.Cache.Event
{

    /// <summary>
    /// 实体类更新
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class EntityUpdated<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// 操作更新的实体类
        /// </summary>
        public T Entity
        {
            get;
            private set;
        }
    }

}