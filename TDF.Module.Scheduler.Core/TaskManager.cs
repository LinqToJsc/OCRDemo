using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using TDF.Core.Configuration;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Module.Scheduler.Core.Entity;
using TDF.Module.Scheduler.Core.Services;

namespace TDF.Module.Scheduler.Core
{
    /// <summary>
    /// 任务管理器
    /// </summary>
    public class TaskManager : IDisposable
    {
        /// <summary>
        /// 任务管理器实例
        /// </summary>
        public static TaskManager Instance { get; }

        static TaskManager()
        {
            Instance = new TaskManager();
        }

        /// <summary>
        /// TaskDll目录
        /// </summary>
        private readonly string _folder;

        /// <summary>
        /// 定时器
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// 定时器任务是否正在执行中
        /// </summary>
        private bool _isTimerBusy;

        /// <summary>
        /// 任务集合
        /// </summary>
        private readonly List<ITask> _tasks;

        /// <summary>
        /// Dll监听器
        /// </summary>
        private readonly DllWatcher _watcher;

        private readonly ILogService _log = Ioc.Resolve<ILogService>();
        private readonly IScheduledTaskService _service = Ioc.Resolve<IScheduledTaskService>();

        public TaskManager()
        {
            _tasks = new List<ITask>();
            _timer = new Timer(60000);//1分钟执行一次任务Dll同步
            _timer.Elapsed += TimerElapsed;
            _folder = Configs.Instance.GetValue("TaskDllFolder");
            if (string.IsNullOrEmpty(_folder))
            {
                _folder = AppDomain.CurrentDomain.BaseDirectory;
                //throw new TDFException("请在Appsetting中配置TaskDllFolder节点");
            }
            _watcher = new DllWatcher(_folder);
            _watcher.OnDllChanged += OnDllChanged;
        }

        /// <summary>
        /// 当有新任务Dll放入任务文件夹中时进行处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDllChanged(object sender, GenericEventArgs<string> e)
        {
            var tasks = LoadDllTasks(e.Data);
            if (tasks.Count > 0)
            {
                //将Dll中的任务配置信息同步到数据库中去
                UpdateDatabaseScheduleTasks(tasks, Path.GetFileName(e.Data));
                if (_tasks.Any(t => tasks.Any(x => x.GetType().FullName == t.GetType().FullName)))
                {
                    _log.LogTaskNotStoppedButDllUpdated(e.Data);
                }
            }
        }

        /// <summary>
        /// 开始运行任务管理器
        /// </summary>
        public void Run()
        {
            _isTimerBusy = false;
            _timer.Start();
            InitDlls();
        }

        /// <summary>
        /// 初始化Dll到数据库中去
        /// </summary>
        private void InitDlls()
        {
            var files = Directory.GetFiles(_folder, "*.dll");
            foreach (var file in files)
            {
                var tasks = LoadDllTasks(file);
                UpdateDatabaseScheduleTasks(tasks, Path.GetFileName(file));
            }
        }

        /// <summary>
        /// 将Dll中的任务配置信息同步到数据库中去
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="dllName"></param>
        private void UpdateDatabaseScheduleTasks(IEnumerable<ITask> tasks, string dllName)
        {
            foreach (var task in tasks)
            {
                var scheduledTask = new ScheduledTask()
                {
                    Name = task.Name,
                    Type = task.GetType().FullName,
                    Description = task.Description,
                    IntervalDescription = task.IntervalDescription,
                    Version = task.Version,
                    DllFileName = dllName
                };
                _service.Save(scheduledTask);
            }
        }

        /// <summary>
        /// 将所有Dll反射为ITask
        /// </summary>
        /// <returns></returns>
        private List<ITask> GetAllTasks()
        {
            var tasks = new List<ITask>();
            var files = Directory.GetFiles(_folder, "*.dll");
            foreach (var file in files)
            {
                tasks.AddRange(LoadDllTasks(file));
            }
            return tasks;
        }

        /// <summary>
        /// 将指定的Dll反射为ITask
        /// </summary>
        /// <param name="dllPath"></param>
        /// <returns></returns>
        private List<ITask> LoadDllTasks(string dllPath)
        {
            if (!File.Exists(dllPath))
            {
                return new List<ITask>();
            }
            var assembly = Assembly.Load(File.ReadAllBytes(dllPath));
            var types = assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(ITask))).ToList();
            return types.Select(type => Activator.CreateInstance(type) as ITask).ToList();
        }

        /// <summary>
        /// 定时执行的逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isTimerBusy)
            {
                return;
            }
            _isTimerBusy = true;
            try
            {
                //同步内存中的List<ITask>根据Dll，并根据数据库的配置来控制任务
                ControlTasksAccordingToDatabase();
            }
            catch (Exception ex)
            {
                _log.LogError(null, "TaskManager_TimerElapsed", ex.GetAllMessages());
            }
            _isTimerBusy = false;
        }

        /// <summary>
        /// 同步内存中的List<ITask/>根据Dll，并根据数据库的配置来控制任务
        /// </summary>
        private void ControlTasksAccordingToDatabase()
        {
            var scheduledTasks = _service.GetAllEnabled();
            //找到数据中已经删除的任务集合
            var tobeRemovedTasks = _tasks.Where(x => scheduledTasks.All(t => t.Type != x.GetType().FullName)).ToList();
            //移除内存中的任务集合
            RemveTaskInMemory(tobeRemovedTasks);
            //找到内存中不存在但数据库中存在的任务集合
            var tobeAddedTasks = scheduledTasks.Where(x => _tasks.All(t => t.GetType().FullName != x.Type)).ToList();
            //添加到任务集合中去
            AddTaskInMemory(tobeAddedTasks);
            //将内存中未运行的任务运行起来
            RunAllTasks();
            //找到数据库中标记为需要立即执行的任务
            var tobeRunImmediatelyTasks =
                _tasks.Where(x => scheduledTasks.All(t => t.RunImmediately && t.Type == x.GetType().FullName)).ToList();
            //立即执行
            RunWorkInTasksImmediately(tobeRunImmediatelyTasks);
        }

        /// <summary>
        /// 移除内存中的任务集合
        /// </summary>
        private void RemveTaskInMemory(List<ITask> tobeRemovedTasks)
        {
            foreach (var tobeRemoved in tobeRemovedTasks)
            {
                if (tobeRemoved.IsBusy())
                {
                    continue;
                }
                tobeRemoved.Stop();
                tobeRemoved.Dispose();
                _log.LogTaskStopped(_service.Get(tobeRemoved.GetType().FullName).Id);
                _service.MarkTaskStopped(tobeRemoved.GetType().FullName);
                _tasks.Remove(tobeRemoved);
            }
        }

        /// <summary>
        /// 将数据库中存在但内存中不存在的任务加载到内存任务列表中去
        /// </summary>
        private void AddTaskInMemory(List<ScheduledTask> tobeAddedTasks)
        {
            if (tobeAddedTasks.Count > 0)
            {
                var allTasks = GetAllTasks();
                foreach (var tobeAdded in tobeAddedTasks)
                {
                    var task = allTasks.FirstOrDefault(x => x.GetType().FullName == tobeAdded.Type);
                    if (task == null)
                    {
                        _service.LogDllNotExists(tobeAdded.Type);
                        continue;
                    }
                    _tasks.Add(task);
                    task.Run();
                    _log.LogTaskStarted(tobeAdded.Id);
                    _service.MarkTaskStarted(tobeAdded.Type);
                }
            }
        }

        /// <summary>
        /// 运行所有的任务
        /// </summary>
        private void RunAllTasks()
        {
            foreach (var task in _tasks.Where(task => !task.IsRunning()))
            {
                task.Run();
                _log.LogTaskStarted(_service.Get(task.GetType().FullName).Id);
                _service.MarkTaskStarted(task.GetType().FullName);
            }
        }

        /// <summary>
        /// 立即运行任务中的执行逻辑
        /// </summary>
        private void RunWorkInTasksImmediately(List<ITask> tobeRunImmediatelyTasks)
        {
            foreach (var task in tobeRunImmediatelyTasks.Where(task => task != null && task.IsRunning() && !task.IsBusy()))
            {
                _service.UpdateImmediatelyRunStarted(task.GetType().FullName);
                task.DoWorkNow();
            }
        }

        #region implementation of IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {

            }
            _timer.Stop();
            _timer.Dispose();
            _watcher.Dispose();
            foreach (var task in _tasks)
            {
                task.Stop();
                task.Dispose();
            }
            _tasks.Clear();
            _disposed = true;
        }

        ~TaskManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
