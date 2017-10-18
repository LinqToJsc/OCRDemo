using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    public class RepositoryBase : IRepositoryBase
    {
        protected readonly AbstractDbContext DbContext = Ioc.Resolve<AbstractDbContext>();

        private DbTransaction DbTransaction { get; set; }

        public IRepositoryBase BeginTrans(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            var dbConnection = ((IObjectContextAdapter) DbContext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            DbTransaction = dbConnection.BeginTransaction(isolationLevel);
            return this;
        }

        public int Commit()
        {
            try
            {
                var returnValue = DbContext.SaveChanges();
                DbTransaction?.Commit();
                return returnValue;
            }
            catch (Exception)
            {
                DbTransaction?.Rollback();
                throw;
            }
        }

        public int Insert<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            DbContext.Entry(entity).State = EntityState.Added;
            return DbTransaction == null ? Commit() : 0;
        }

        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class, IEntity
        {
            foreach (var entity in entitys)
            {
                DbContext.Entry(entity).State = EntityState.Added;
            }
            return DbTransaction == null ? this.Commit() : 0;
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
        public int Update<TEntity>(TEntity entity) where TEntity : class, IEntity
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
            return DbTransaction == null ? this.Commit() : 0;
        }

        public int Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
            return DbTransaction == null ? this.Commit() : 0;
        }

        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
        {
            var entitys = DbContext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => DbContext.Entry(m).State = EntityState.Deleted);
            return DbTransaction == null ? this.Commit() : 0;
        }

        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class, IEntity
        {
            return DbContext.Set<TEntity>().Find(keyValue);
        }

        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
        {
            return DbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="track">是否跟踪数据，如果为true那么查询出来的数据可以使用SaveChange()方法进行更新。</param>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable<TEntity>(bool track = false) where TEntity : class, IEntity
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
        public IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate, bool track = false) where TEntity : class, IEntity
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
            DbTransaction?.Dispose();
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
