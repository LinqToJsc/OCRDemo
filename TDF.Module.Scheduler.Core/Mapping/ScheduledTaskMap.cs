using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Scheduler.Core.Entity;

namespace TDF.Module.Scheduler.Core.Mapping
{
    public class ScheduledTaskMap : EntityTypeConfiguration<ScheduledTask>
    {
        public ScheduledTaskMap()
        {
            
        }
    }
}
