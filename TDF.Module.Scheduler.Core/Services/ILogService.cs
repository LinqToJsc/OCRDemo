using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Scheduler.Core.Entity.Enums;

namespace TDF.Module.Scheduler.Core.Services
{
    /// <summary>
    /// 任务日志服务
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        void LogError(Guid? taskId, string name, string description);

        /// <summary>
        /// 调度服务开始运行
        /// </summary>
        void LogServiceStarted();

        /// <summary>
        /// 调度服务暂停运行
        /// </summary>
        void LogServiceStopped();

        /// <summary>
        /// 任务开始运行
        /// </summary>
        /// <param name="taskId"></param>
        void LogTaskStarted(Guid taskId);

        /// <summary>
        /// 任务暂停运行
        /// </summary>
        /// <param name="taskId"></param>
        void LogTaskStopped(Guid taskId);

        /// <summary>
        /// 任务完成运行
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="version"></param>
        void LogTaskWorkCompleted(Guid taskId, string version);

        /// <summary>
        /// 没有找到任务的Dll
        /// </summary>
        /// <param name="dllPath"></param>
        void LogTaskNotStoppedButDllUpdated(string dllPath);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="level"></param>
        void Log(Guid? taskId, string name, string description, TaskLogLevel level);
    }
}
