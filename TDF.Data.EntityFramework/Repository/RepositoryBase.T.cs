using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TDF.Core;
using TDF.Core.Entity;
using TDF.Core.Ioc;
using TDF.Data.EntityFramework.DbContext;
using TDF.Data.Repository;

namespace TDF.Data.EntityFramework.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity, new()
    {
        protected readonly AbstractDbContext DbContext = Ioc.Resolve<AbstractDbContext>();

        public RepositoryBase()
        {
        }

        public int Insert(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Added;
            return DbContext.SaveChanges();
        }

        public int Insert(List<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                DbContext.Entry(entity).State = EntityState.Added;
            }
            return DbContext.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            if (DbContext.Entry(entity).State != EntityState.Detached)
            {
                return DbContext.SaveChanges();
            }
            DbContext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (typeof(IEntity<>).IsAssignableGenericFrom(prop.PropertyType) ||//这里只对当前实体更新，不对实体内部更新
                        typeof(IEntity).IsAssignableGenericFrom(prop.PropertyType) ||
                        typeof(ICollection<>).IsAssignableGenericFrom(prop.PropertyType))
                    {
                        continue;
                    }
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        DbContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    DbContext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return DbContext.SaveChanges();
        }

        public int Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            return DbContext.SaveChanges();
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitys = DbContext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => DbContext.Entry(m).State = EntityState.Deleted);
            return DbContext.SaveChanges();
        }
        public TEntity FindEntity(object keyValue)
        {
            return DbContext.Set<TEntity>().Find(keyValue);
        }

        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> IQueryable()
        {
            return DbContext.Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="track">是否跟踪数据，如果为true那么查询出来的数据可以使用SaveChange()方法进行更新。</param>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable(bool track = false)
        {
            return track ? DbContext.Set<TEntity>().AsQueryable() : DbContext.Set<TEntity>().AsQueryable().AsNoTracking();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="track">是否跟踪数据，如果为true那么查询出来的数据可以使用SaveChange()方法进行更新。</param>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate, bool track = false)
        {
            return track ? DbContext.Set<TEntity>().Where(predicate) : DbContext.Set<TEntity>().Where(predicate).AsNoTracking();
        }

        #region implementation of IDisposable

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
               //释放非托管资源
            }
            DbContext.Dispose();
            _disposed = true;
        }

        ~RepositoryBase()
        {
            Dispose(false);
        }

        #endregion
    }
}