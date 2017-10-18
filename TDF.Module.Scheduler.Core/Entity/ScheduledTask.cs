using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Module.Scheduler.Core.Entity
{
    /// <summary>
    /// 计划任务
    /// </summary>
    public class ScheduledTask : EntityBase
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }
        
        /// <summary>
        /// 任务类型
        /// </summary>
        [Required, MaxLength(500)]
        public string Type { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [Required, MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 执行频率描述
        /// </summary>
        [Required, MaxLength(500)]
        public string IntervalDescription { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required, MaxLength(200)]
        public string Version { get; set; }

        /// <summary>
        /// 上次执行的版本
        /// </summary>
        [MaxLength(200)]
        public string LastWorkedVersion { get; set; }

        /// <summary>
        /// Dll文件名称
        /// </summary>
        [Required, MaxLength(200)]
        public string DllFileName { get; set; }

        /// <summary>
        /// 是否是启动的
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否是忙碌的
        /// </summary>
        public bool IsBusy { get; set; }
        
        /// <summary>
        /// Dll是否是存在的
        /// </summary>
        public bool DllExists { get; set; }

        /// <summary>
        /// 是否是立即允许的
        /// </summary>
        public bool RunImmediately { get; set; }

        public DateTime? StartedTime { get; set; }

        public DateTime? StoppedTime { get; set; }

        public DateTime? LastWorkStartedTime { get; set; }

        public DateTime? LastWorkCompletedTime { get; set; }

        public virtual ICollection<TaskLog> TaskLogs { get; set; }

        public void UpdateOnDllChanged(ScheduledTask task)
        {
            this.Name = task.Name;
            this.Description = task.Description;
            this.IntervalDescription = task.IntervalDescription;
            this.LastUpdatedTime = DateTime.Now;
            this.Version = task.Version;
            this.DllExists = true;
            this.DllFileName = task.DllFileName;
        }
    }
}
