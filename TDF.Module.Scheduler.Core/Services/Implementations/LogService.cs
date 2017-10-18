using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Ioc;
using TDF.Core.Log;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;
using TDF.Module.Scheduler.Core.Entity;
using TDF.Module.Scheduler.Core.Entity.Enums;

namespace TDF.Module.Scheduler.Core.Services.Implementations
{
    public class LogService : ILogService
    {
        public void LogError(Guid? taskId, string name, string description)
        {
            Log(taskId, name, description, TaskLogLevel.UnknownError);
        }

        public void LogServiceStarted()
        {
            Log(null, "服务启动", null, TaskLogLevel.Info);
        }

        public void LogServiceStopped()
        {
            Log(null, "服务停止", null, TaskLogLevel.Warning);
        }

        public void LogTaskStarted(Guid taskId)
        {
            Log(taskId, "任务启动", null, TaskLogLevel.Info);
        }

        public void LogTaskStopped(Guid taskId)
        {
            Log(taskId, "任务停止", null, TaskLogLevel.Info);
        }

        public void LogTaskWorkCompleted(Guid taskId, string version)
        {
            Log(taskId, "任务执行完成", version, TaskLogLevel.Info);
        }

        public void LogTaskNotStoppedButDllUpdated(string dllPath)
        {
            Log(null, "DLL错误", dllPath + "-任务没有停止，但是DLL却更新了。更新的DLL不会执行。请先停止该任务，再重新启动该任务，新的DLL中的任务将会生效。", TaskLogLevel.UnknownError);
        }

        public void Log(Guid? taskId, string name, string description, TaskLogLevel level)
        {
            try
            {
                using (var repository = Ioc.Resolve<IRepositoryBase<TaskLog>>())
                {
                    repository.Insert(new TaskLog()
                    {
                        TaskId = taskId,
                        Name = name,
                        Description = description,
                        Level = level,
                        Id = Guid.NewGuid()
                    });
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger(GetType()).Error(ex);
            }
        }
    }
}
