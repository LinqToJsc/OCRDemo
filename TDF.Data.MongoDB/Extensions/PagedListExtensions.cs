using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TDF.Core.Models;

namespace TDF.Data.MongoDB.Extensions
{
    public static class PagedListExtensions
    {
        public static PagedList<T> ToPageResult<T>(this IFindFluent<T, T> query, int pageIndex, int pageSize)
        {
            var pageResult = new PagedList<T>();
            var totalCount = query.Count();
            var totalPages = (int)totalCount / pageSize;
            pageResult.TotalPages = totalCount % pageSize == 0 ? totalPages : totalPages + 1;
            pageResult.Rows = query.Skip(pageIndex * pageSize).Limit(pageSize).ToList();
            pageResult.PageIndex = pageIndex;
            pageResult.TotalCount = (int)totalCount;
            return pageResult;
        }
    }
}
