using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;

namespace TDF.Module.Scheduler.Core.Entity
{
    public class EntityBase : IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastUpdatedTime { get; set; }

        public EntityBase()
        {
            this.Id = Guid.Empty;
            this.CreatedTime = DateTime.Now;
        }

        public void NewId()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
