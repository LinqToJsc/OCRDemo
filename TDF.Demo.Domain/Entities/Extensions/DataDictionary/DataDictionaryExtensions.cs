using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Domain.Entities.Extensions.DataDictionary
{
    public static class DataDictionaryExtensions
    {
        public static IQueryable<Entities.DataDictionary> WhereByEnabled(this IQueryable<Entities.DataDictionary> query, bool? enabled)
        {
            if (enabled == null)
            {
                return query;
            }
            return query.Where(x => x.Disabled != enabled.Value);
        }

        public static IQueryable<Entities.DataDictionary> WhereByTypeId(this IQueryable<Entities.DataDictionary> query, Guid? typeId)
        {
            if (typeId == null)
            {
                return query;
            }
            return query.Where(x => x.ParentId == typeId.Value);
        }

        public static IQueryable<Entities.DataDictionary> WhereByKeyword(this IQueryable<Entities.DataDictionary> query, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return query;
            }
            return query.Where(x => x.Name.Contains(keyword.Trim()));
        }
    }
}
