namespace YanZhiwei.DotNet.Core.Service.Events
{
    /// <summary>
    /// 实体类插入
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class EntityInserted<T> where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">实体类/param>
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// 操作插入的实体类
        /// </summary>
        public T Entity
        {
            get;
            private set;
        }
    }
}