using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Driver;
using TDF.Core;
using TDF.Core.Entity;
using TDF.Data.Repository;

namespace TDF.Data.MongoDB.Repository
{
    public class RepositoryBase : IRepositoryBase
    {

        public IRepositoryBase BeginTrans(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            throw new NotImplementedException("MongoDB暂时不支持事务");
            return this;
        }

        public int Commit()
        {
            throw new NotImplementedException("MongoDB暂时不支持事务");
        }

        public int Insert<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            MongodbHelper.Default.Insert(entity);
            return 1;
        }

        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class, IEntity
        {
            MongodbHelper.Default.Insert(entitys);
            return 1;
        }

        public int Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            UpdateDefinition<TEntity> update = null;
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (update == null)
                    {
                        update = Builders<TEntity>.Update.Set(prop.Name, prop.GetValue(entity, null));
                    }
                    else
                    {
                        update.Set(prop.Name, prop.GetValue(entity, null));
                    }
                }
            }
            if (update != null)
            {
                MongodbHelper.Default.GetCollection<TEntity>()
                    .UpdateOne(new ExpressionFilterDefinition<TEntity>(x => x.Id == entity.Id),
                        update);
            }
            return 1;
        }

        public int Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            MongodbHelper.Default.Remove<TEntity>(x=>x.Id == entity.Id);
            return 1;
        }

        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
        {
            MongodbHelper.Default.Remove(predicate);
            return 1;
        }

        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class, IEntity
        {
            return MongodbHelper.Default.Find<TEntity>(x => x.Id == (Guid) keyValue).FirstOrDefault();
        }

        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity
        {
            return MongodbHelper.Default.Find(predicate).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="track">是否跟踪数据，如果为true那么查询出来的数据可以使用SaveChange()方法进行更新。</param>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable<TEntity>(bool track = false) where TEntity : class, IEntity
        {
            return MongodbHelper.Default.GetCollection<TEntity>().AsQueryable();
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
            return MongodbHelper.Default.GetCollection<TEntity>().AsQueryable().Where(predicate);
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
            _disposed = true;
        }

        ~RepositoryBase()
        {
            Dispose(false);
        }

        #endregion
    }
}
