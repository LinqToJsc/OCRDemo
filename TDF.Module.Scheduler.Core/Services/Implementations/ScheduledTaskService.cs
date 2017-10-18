using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Ioc;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;
using TDF.Module.Scheduler.Core.Entity;

namespace TDF.Module.Scheduler.Core.Services.Implementations
{
   public class ScheduledTaskService : IScheduledTaskService
    {
        public ScheduledTask Get(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                return repository.FindEntity(x => x.Type == type);
            }
        }

        public List<ScheduledTask> GetAllEnabled()
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                return repository.IQueryable(x => x.IsEnabled ).ToList();
            }
        }

        public void MarkTaskStarted(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                task.StartedTime = DateTime.Now;
                repository.Update(task);
            }
        }

        public void MarkTaskStopped(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                if (task == null)
                {
                    return;
                }
                task.StoppedTime = DateTime.Now;
                task.IsBusy = false;
                task.IsEnabled = false;
                repository.Update(task);
            }
        }

        public void Save(ScheduledTask task)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var dbTask = repository.FindEntity(x => x.Type == task.Type);
                if (dbTask == null)
                {
                    dbTask = new ScheduledTask
                    {
                        Type = task.Type,
                        Description = task.Description,
                        DllExists = true,
                        Id = Guid.NewGuid(),
                        Name = task.Name,
                        CreatedTime = DateTime.Now,
                        DllFileName = task.DllFileName,
                        IntervalDescription = task.IntervalDescription,
                        IsBusy = false,
                        IsEnabled = true, //todo=>feijie 这里默认为启动吧，等以后完善之后有UI控制界面了再改未默认禁用
                        LastWorkedVersion = task.LastWorkedVersion,
                        RunImmediately = false,
                        Version = task.Version
                    };
                    repository.Insert(dbTask);
                }
                else
                {
                    dbTask.UpdateOnDllChanged(task);
                    repository.Update(dbTask);
                }
            }
        }

        public DateTime? GetLastWorkCompletedTime(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                return task.LastWorkCompletedTime;
            }
        }

        public void UpdateLastWorkStartedTime(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                task.LastWorkStartedTime = DateTime.Now;
                task.IsBusy = true;
                repository.Update(task);
            }
        }

        public void UpdateLastWorkCompletedTime(string type, string version)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                task.LastWorkCompletedTime = DateTime.Now;
                task.IsBusy = false;
                task.RunImmediately = false;
                task.LastWorkedVersion = version;
                repository.Update(task);
            }
        }

        public void LogDllNotExists(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                task.DllExists = false;
                repository.Update(task);
            }
        }

        public void UpdateImmediatelyRunStarted(string type)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ScheduledTask>>())
            {
                var task = repository.FindEntity(x => x.Type == type);
                task.RunImmediately = false;
                repository.Update(task);
            }
        }
    }
}
