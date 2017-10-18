using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TDF.Core.Entity;
using TDF.Core.Event;
using TDF.Core.Ioc;
using TDF.Core.Operator;
using TDF.Demo.Domain.Events;
using TDF.Demo.Domain.Infrastructure;

namespace TDF.Demo.Domain.Entities.Extensions
{
    public static class EntityExtensions
    {
        public static void Create<T>(this T entity) where T : ICreationAudited
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedTime = DateTime.Now;
            Ioc.Resolve<IEventPublisher>().Publish(new EntityInserted<T>(entity));
        }

        public static void Modify<T>(this T entity) where T : IModificationAudited
        {
            entity.ModifiedTime = DateTime.Now;
            Ioc.Resolve<IEventPublisher>().Publish(new EntityUpdated<T>(entity));
        }

        public static void Remove<T>(this T entity) where T : IDeleteAudited
        {
            entity.DeletedTime = DateTime.Now;
            entity.Deleted = true;
            Ioc.Resolve<IEventPublisher>().Publish(new EntityDeleted<T>(entity));
        }

        public static List<TResult> ToDtos<T, TResult>(this List<T> list) where T : IEntity
        {
            return list.Select(x => Mapper.Map<TResult>(x)).ToList();
        }

        public static void CreateByOperator<T>(this T entity) where T : SystemEntity
        {
            var oper = Ioc.Resolve<IOperatorProvider>().GetCurrent();
            if (oper != null)
            {
                entity.CreatorId = oper.Id;
                entity.CreatorName = oper.UserName;
            }
            entity.Create();
        }

        public static void ModifyByOperator<T>(this T entity) where T : SystemEntity
        {
            var oper = Ioc.Resolve<IOperatorProvider>().GetCurrent();
            if (oper != null)
            {
                entity.ModifierId = oper.Id;
                entity.ModifierName = oper.UserName;
            }
            entity.Modify();
        }

        public static void RemoveByOperator<T>(this T entity) where T : SystemEntity
        {
            var oper = Ioc.Resolve<IOperatorProvider>().GetCurrent();
            if (oper != null)
            {
                entity.ModifierId = oper.Id;
                entity.ModifierName = oper.UserName;
            }
            entity.Remove();
        }

        public static bool IsNullOrDeleted<T>(this T entity) where T : IDeleteAudited
        {
            return entity == null || (entity.DeletedTime != null && entity.Deleted);
        }

    }
}
