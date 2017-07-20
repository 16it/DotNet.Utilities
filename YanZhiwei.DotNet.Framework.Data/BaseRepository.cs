using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using YanZhiwei.DotNet.Framework.Contract;
using YanZhiwei.DotNet2.Utilities.Collection;

namespace YanZhiwei.DotNet.Framework.Data
{
    public class BaseRepository<F> : IDataRepository<F>
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected DbSet<ModelBase<F>> dbSet;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            dbSet = UnitOfWork.Db.Set<ModelBase<F>>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体类</param>
        /// 时间：2016-01-13 13:31
        /// 备注：
        public void Delete<T>(T entity) where T : ModelBase<F>
        {
            if (UnitOfWork.Db.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            UnitOfWork.Db.SaveChanges();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keyValues">删除依据的键</param>
        /// <returns>
        /// 实体类
        /// </returns>
        /// 时间：2016-01-13 13:32
        /// 备注：
        public T Find<T>(params object[] keyValues) where T : ModelBase<F>
        {
            // return UnitOfWork.Db.Set<T>().Find(keyValues);
            return dbSet.Find(keyValues);
        }

        public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : ModelBase<F>
        {

            if (conditions == null)
                return dbSet.ToList<T>();
            else
                return dbSet.Where(conditions).ToList<T>();
        }

        public PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex) where T : ModelBase<F>
        {
            throw new NotImplementedException();
        }

        public T Insert<T>(T entity) where T : ModelBase<F>
        {
            throw new NotImplementedException();
        }

        public T Update<T>(T entity) where T : ModelBase<F>
        {
            throw new NotImplementedException();
        }
    }
}