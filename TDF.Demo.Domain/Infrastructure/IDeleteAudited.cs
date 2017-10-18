using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;

namespace TDF.Demo.Domain.Infrastructure
{
    public interface IDeleteAudited : IEntity
    {
        bool Deleted { get; set; }

        DateTime? DeletedTime { get; set; }
    }

    public static class DeleteAuditedExtensions
    {
        public static IQueryable<T> WhereNotDelete<T>(this IQueryable<T> query) where T : class, IDeleteAudited
        {
            return query.Where(x => x.Deleted == false);
        }
    }
}
