using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Scheduler.Core.Entity;

namespace TDF.Module.Scheduler.Core.Mapping
{
    public class TaskLogMap : EntityTypeConfiguration<TaskLog>
    {
        public TaskLogMap()
        {
            HasOptional(x => x.Task)
                .WithMany(x => x.TaskLogs)
                .HasForeignKey(x => x.TaskId)
                .WillCascadeOnDelete();
        }
    }
}
