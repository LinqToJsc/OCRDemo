using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;
using TDF.Core.Event;

namespace TDF.Demo.Domain.Events
{
    public class EntityDeleted<T> : IEvent where T : IEntity
    {
        public T Entity { get; }

        public bool CancelBubble { get; set; }

        public int Order { get; set; }

        public bool Async { get; set; }

        public EntityDeleted(T entity)
        {
            Entity = entity;
            Order = 1;
            CancelBubble = false;
        }
    }
}
