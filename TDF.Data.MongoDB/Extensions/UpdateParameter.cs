using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace TDF.Data.MongoDB.Extensions
{
    public static class UpdateParameter
    {
        public static UpdateDefinition<T> Create<T>(params KeyValuePair<Expression<Func<T, object>>, object>[]  updates)
        {
            UpdateDefinition<T> updateDefinination = null;
            var update = Builders<T>.Update;
            updates.ToList().ForEach(x =>
            {
                updateDefinination = updateDefinination == null
                    ? update.Set(x.Key, x.Value)
                    : updateDefinination.Set(x.Key, x.Value);
            });
            return updateDefinination;
        }

        //public static UpdateDefinition<T> Set<T>(this UpdateDefinition<T> updateDefinition, Expression<Func<T, object>> key, object value)
        //{
        //    updateDefinition.Set(key,value).set
        //    return updateDefinination;
        //}

        public static UpdateDefinition<T> Create<T>(Expression<Func<T, object>> key, object value)
        {
            var update = Builders<T>.Update;
            var updateDefinination = update.Set(key, value);
            return updateDefinination;
        }
    }
}
