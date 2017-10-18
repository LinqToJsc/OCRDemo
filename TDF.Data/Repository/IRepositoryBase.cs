using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TDF.Core.Entity;

namespace TDF.Data.Repository
{
    public interface IRepositoryBase : IDisposable
    {
        IRepositoryBase BeginTrans(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);

        int Commit();

        int Insert<TEntity>(TEntity entity) where TEntity : class, IEntity;

        int Insert<TEntity>(List<TEntity> entitys) where TEntity : class, IEntity;

        int Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

        int Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity;

        TEntity FindEntity<TEntity>(object keyValue) where TEntity : class, IEntity;

        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntity;

        IQueryable<TEntity> IQueryable<TEntity>(bool track = false) where TEntity : class, IEntity;

        IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate, bool track = false)
            where TEntity : class, IEntity;
    }
}
