using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TDF.Core.Exceptions;
using TDF.Core.Models.Enum;

namespace TDF.Core
{
    public static partial class Ext
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string property, OrderBy orderby)
        {
            if (orderby == Models.Enum.OrderBy.Desc)
            {
                return OrderByDescending(query, property);
            }
            return OrderBy(query, property);

        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string property)
        {
            var type = typeof (T);
            var para = Expression.Parameter(type);
            var memberInfo = type.GetMember(property)[0];
            var member = Expression.MakeMemberAccess(para, memberInfo);
            var propertyType = type.GetProperty(property).PropertyType;

            if (propertyType.IsEnum)
            {
                var asExpr = Expression.Convert(member, typeof (int));
                return query.OrderBy(Expression.Lambda<Func<T, int>>(asExpr, para));
            }
            if (propertyType == typeof (string))
            {
                return query.OrderBy(Expression.Lambda<Func<T, string>>(member, para));
            }
            if (propertyType == typeof (DateTime))
            {
                return query.OrderBy(Expression.Lambda<Func<T, DateTime>>(member, para));
            }
            if (propertyType == typeof (DateTime?))
            {
                return query.OrderBy(Expression.Lambda<Func<T, DateTime?>>(member, para));
            }

            if (propertyType == typeof (int))
            {
                return query.OrderBy(Expression.Lambda<Func<T, int>>(member, para));
            }
            if (propertyType == typeof (int?))
            {
                return query.OrderBy(Expression.Lambda<Func<T, int?>>(member, para));
            }

            if (propertyType == typeof (decimal))
            {
                return query.OrderBy(Expression.Lambda<Func<T, decimal>>(member, para));
            }
            if (propertyType == typeof (decimal?))
            {
                return query.OrderBy(Expression.Lambda<Func<T, decimal?>>(member, para));
            }

            if (propertyType == typeof (bool))
            {
                return query.OrderBy(Expression.Lambda<Func<T, bool>>(member, para));
            }
            if (propertyType == typeof (bool?))
            {
                return query.OrderBy(Expression.Lambda<Func<T, bool?>>(member, para));
            }

            if (propertyType == typeof (double))
            {
                return query.OrderBy(Expression.Lambda<Func<T, double>>(member, para));
            }
            if (propertyType == typeof (double?))
            {
                return query.OrderBy(Expression.Lambda<Func<T, double?>>(member, para));
            }

            if (propertyType == typeof (float))
            {
                return query.OrderBy(Expression.Lambda<Func<T, float>>(member, para));
            }
            if (propertyType == typeof (float?))
            {
                return query.OrderBy(Expression.Lambda<Func<T, float?>>(member, para));
            }

            throw new KnownException("不支持的数据类型：" + propertyType);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string property)
        {
            var type = typeof (T);
            var para = Expression.Parameter(type);
            var memberInfo = type.GetMember(property)[0];
            var member = Expression.MakeMemberAccess(para, memberInfo);
            var propertyType = type.GetProperty(property).PropertyType;

            if (propertyType.IsEnum)
            {
                var asExpr = Expression.Convert(member, typeof (int));
                return query.OrderByDescending(Expression.Lambda<Func<T, int>>(asExpr, para));
            }
            if (propertyType == typeof (string))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, string>>(member, para));
            }
            if (propertyType == typeof (DateTime))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, DateTime>>(member, para));
            }
            if (propertyType == typeof (DateTime?))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, DateTime?>>(member, para));
            }

            if (propertyType == typeof (int))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, int>>(member, para));
            }
            if (propertyType == typeof (int?))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, int?>>(member, para));
            }

            if (propertyType == typeof (decimal))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, decimal>>(member, para));
            }
            if (propertyType == typeof (decimal?))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, decimal?>>(member, para));
            }

            if (propertyType == typeof (bool))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, bool>>(member, para));
            }
            if (propertyType == typeof (bool?))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, bool?>>(member, para));
            }

            if (propertyType == typeof (double))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, double>>(member, para));
            }
            if (propertyType == typeof (double?))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, double?>>(member, para));
            }

            if (propertyType == typeof (float))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, float>>(member, para));
            }
            if (propertyType == typeof (float?))
            {
                return query.OrderByDescending(Expression.Lambda<Func<T, float?>>(member, para));
            }

            throw new KnownException("不支持的数据类型：" + propertyType);
        }

        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
        public static Expression<T> ToLambda<T>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<T>(body, parameters);
        }
        public static Expression<Func<T, bool>> True<T>() { return param => true; }
        public static Expression<Func<T, bool>> False<T>() { return param => false; }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        private class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;
            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }
            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
