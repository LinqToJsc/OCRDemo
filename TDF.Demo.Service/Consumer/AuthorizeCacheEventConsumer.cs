using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Caching;
using TDF.Core.Event;
using TDF.Demo.Domain.Entities;
using TDF.Demo.Domain.Events;

namespace TDF.Demo.Service.Consumer
{
    public class AuthorizeCacheEventConsumer : IConsumer<EntityUpdated<SystemAction>>, IConsumer<EntityInserted<SystemAction>>, IConsumer<EntityDeleted<SystemAction>>
    {
        /// <summary>
        /// {0}-MemberId
        /// </summary>
        public const string AUTHORIZE_KEY = "authorize-{0}";

        public void Handler(EntityUpdated<SystemAction> eventEntity)
        {
            var memberIds =
                eventEntity.Entity.SystemActionRoles.SelectMany(
                    x => x.SystemRole.SystemMemberRoles.Select(y => y.SystemMemberId)).ToList();
            foreach (var memberId in memberIds)
            {
                CacheManager.Remove(string.Format(AUTHORIZE_KEY, memberId));
            }
        }

        public void Handler(EntityInserted<SystemAction> eventEntity)
        {
            var memberIds =
                eventEntity.Entity.SystemActionRoles.SelectMany(
                    x => x.SystemRole.SystemMemberRoles.Select(y => y.SystemMemberId)).ToList();
            foreach (var memberId in memberIds)
            {
                CacheManager.Remove(string.Format(AUTHORIZE_KEY, memberId));
            }
        }

        public void Handler(EntityDeleted<SystemAction> eventEntity)
        {
            var memberIds =
                eventEntity.Entity.SystemActionRoles.SelectMany(
                    x => x.SystemRole.SystemMemberRoles.Select(y => y.SystemMemberId)).ToList();
            foreach (var memberId in memberIds)
            {
                CacheManager.Remove(string.Format(AUTHORIZE_KEY, memberId));
            }
        }
    }
}
