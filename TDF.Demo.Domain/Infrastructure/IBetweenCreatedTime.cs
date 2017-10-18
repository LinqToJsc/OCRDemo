using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;

namespace TDF.Demo.Domain.Infrastructure
{
    public interface IBetweenCreatedTime : IEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedTime { get; set; }
    }
    public static class BetweenCreatedTimeExtensions
    {
        /// <summary>
        /// 时间区间查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="createdTimeRange">创建时间区间</param>
        /// <returns></returns>
        public static IQueryable<T> WhereBetweenTime<T>(this IQueryable<T> query, DateTime[] createdTimeRange) where T : class, IBetweenCreatedTime
        {
            if (createdTimeRange == null || createdTimeRange.Contains(DateTime.MinValue))
                return query;

            var beginTime = createdTimeRange[0];
            var endTime = createdTimeRange[1];

            if (beginTime < endTime)
            {
                endTime = endTime.AddDays(1);
                return query.Where(x => x.CreatedTime >= beginTime && x.CreatedTime <= endTime);
            }
            else if (beginTime > endTime)
            {
                beginTime = beginTime.AddDays(1);
                return query.Where(x => x.CreatedTime <= beginTime && x.CreatedTime >= endTime);
            }
            else if (beginTime == endTime || createdTimeRange.Length == 1)
            {//查询某天数据
                endTime = endTime.AddDays(1);
                return query.Where(x => x.CreatedTime >= beginTime && x.CreatedTime <= endTime);
            }

            return query;
        }
    }
}
