namespace YanZhiwei.DotNet.Core.Cache.Event
{
    /// <summary>
    /// 实体类删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// 操作删除的实体类
        /// </summary>
        public T Entity
        {
            get;
            private set;
        }
    }
}