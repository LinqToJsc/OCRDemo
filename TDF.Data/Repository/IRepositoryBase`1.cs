using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TDF.Core.Entity;

namespace TDF.Data.Repository
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class, IEntity, new()
    {
        int Insert(TEntity entity);

        int Insert(List<TEntity> entitys);

        int Update(TEntity entity);

        int Delete(TEntity entity);


        int Delete(Expression<Func<TEntity, bool>> predicate);

        TEntity FindEntity(object keyValue);

        TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> IQueryable(bool track = false);

        IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate, bool track = false);

    }
}
