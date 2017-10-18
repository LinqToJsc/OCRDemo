using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TDF.Core.Entity;
using TDF.Data.MongoDB.Repository;
using TDF.Data.Repository;

namespace TDF.Data.MongoDB.Extensions
{
    public static class RepositoryBaseExtensions
    {
        public static IMongoCollection<TEntity> GetCollection<TEntity>(this IRepositoryBase repository) where TEntity : class, IEntity, new()
        {
            return MongodbHelper.Default.GetCollection<TEntity>();
        }

        public static IMongoCollection<TEntity> GetCollection<TEntity>(this IRepositoryBase<TEntity> repository) where TEntity : class, IEntity, new()
        {
            return MongodbHelper.Default.GetCollection<TEntity>();
        }
    }
}
