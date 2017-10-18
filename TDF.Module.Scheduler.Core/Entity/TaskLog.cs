using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Scheduler.Core.Entity.Enums;

namespace TDF.Module.Scheduler.Core.Entity
{
    public class TaskLog : EntityBase
    {
        public Guid? TaskId { get; set; }

        [Required, MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public TaskLogLevel Level { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        public virtual ScheduledTask Task { get; set; }
    }
}
