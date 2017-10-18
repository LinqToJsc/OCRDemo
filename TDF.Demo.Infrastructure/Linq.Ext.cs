using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Json;

namespace System.Linq
{
    public static class LinqExtensions
    {
        /// <summary>
        /// 根据数组属性进行分组排序
        /// </summary>
        /// <typeparam name="T1">元数据类型</typeparam>
        /// <typeparam name="T2">元数据中需要排序的数组中的对象类型</typeparam>
        /// <typeparam name="T3">返回的结果类型</typeparam>
        /// <param name="list">元数据集合</param>
        /// <param name="keySelector">对元数据集合中哪一个数组属性做为分组Key</param>
        /// <param name="predicate">过滤出需要排序的字段集合</param>
        /// <param name="selector">返回的分组数据集合</param>
        /// <returns></returns>
        public static List<T3> GroupByArray<T1, T2, T3>(this IEnumerable<T1> list, Func<T1, IEnumerable<T2>> keySelector,
            Func<T2, bool> predicate, Func<IEnumerable<T2>, IGrouping<string, KeyValuePair<string, T1>>, T3> selector)
            where T1 : class, new()
            where T2 : class, new()
        {
            var kvs = new List<KeyValuePair<string, T1>>();
            foreach (var item in list)
            {
                var groupPropertys = keySelector.Invoke(item).Where(predicate).ToList();
                kvs.Add(new KeyValuePair<string, T1>(groupPropertys.ToJson(), item));
            }
            return
                kvs.GroupBy(x => x.Key)
                    .Select(x => new { Key = x.Key.ToJson<List<T2>>(), Selector = x })
                    .Select(x => selector.Invoke(x.Key, x.Selector))
                    .ToList();
        }
    }
}
