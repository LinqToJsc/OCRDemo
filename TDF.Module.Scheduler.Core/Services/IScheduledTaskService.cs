using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Scheduler.Core.Entity;

namespace TDF.Module.Scheduler.Core.Services
{
    /// <summary>
    /// 计划任务服务
    /// </summary>
    public interface IScheduledTaskService
    {
        /// <summary>
        /// 获取调度任务
        /// </summary>
        /// <param name="type">任务类型名称GetType().FullName</param>
        /// <returns></returns>
        ScheduledTask Get(string type);

        /// <summary>
        /// 获取所有启动的调度任务
        /// </summary>
        /// <returns></returns>
        List<ScheduledTask> GetAllEnabled();

        /// <summary>
        /// 标记任务为开始
        /// </summary>
        /// <param name="type"></param>
        void MarkTaskStarted(string type);

        /// <summary>
        /// 标记任务为暂停
        /// </summary>
        /// <param name="type"></param>
        void MarkTaskStopped(string type);

        /// <summary>
        /// 保存任务AddOrUpdate
        /// </summary>
        /// <param name="task"></param>
        void Save(ScheduledTask task);

        /// <summary>
        /// 获取任务上次执行完成的时间
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DateTime? GetLastWorkCompletedTime(string type);

        /// <summary>
        /// 更新任务开始执行的时间
        /// </summary>
        /// <param name="type"></param>
        void UpdateLastWorkStartedTime(string type);

        /// <summary>
        /// 更新任务完成
        /// </summary>
        /// <param name="type"></param>
        /// <param name="version"></param>
        void UpdateLastWorkCompletedTime(string type, string version);

        /// <summary>
        /// 标记任务的Dll不存在
        /// </summary>
        /// <param name="type"></param>
        void LogDllNotExists(string type);

        /// <summary>
        /// 标记任务正在立即执行中
        /// </summary>
        /// <param name="type"></param>
        void UpdateImmediatelyRunStarted(string type);
    }
}
