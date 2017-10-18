using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TDF.Core;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Core.Log;
using TDF.Module.Scheduler.Core.DateTimeExtensions;
using TDF.Module.Scheduler.Core.Services;
using Timer = System.Timers.Timer;

namespace TDF.Module.Scheduler.Core
{
    /// <summary>
    /// 任务抽象类，默认以Timer实现定时器功能
    /// </summary>
    public abstract class TaskBase : ITask
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// 任务时间片是否正在运行中
        /// </summary>
        private bool _busy = false;

        /// <summary>
        /// 上次任务执行完成时的时间
        /// </summary>
        public virtual DateTime? LastWorkedTime { get; private set; }

        /// <summary>
        /// 上次任务准备开始执行的时间
        /// </summary>
        public virtual DateTime? LastWorkingTime { get; private set; }

        /// <summary>
        /// 任务是否正在运行中
        /// </summary>
        private bool _isRunning = false;

        /// <summary>
        /// 任务Id
        /// </summary>
        private Guid? _taskId;

        /// <summary>
        /// 任务名称
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 描述
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// 执行间隔描述
        /// </summary>
        public abstract string IntervalDescription { get; }

        /// <summary>
        /// 定时器调度间隔，默认为1000毫秒
        /// </summary>
        public virtual int TimerInterval { get; } = 1000;

        /// <summary>
        /// 任务逻辑
        /// </summary>
        /// <param name="callback"></param>
        public abstract void DoWork(Action<string> callback);

        /// <summary>
        /// 调度平率
        /// </summary>
        public abstract TaskFrequency Frequency { get; }

        /// <summary>
        /// 调度开始的时间偏移量
        /// </summary>
        public abstract TimeSpan FrequencyTime { get; }

        /// <summary>
        /// 任务版本
        /// </summary>
        public abstract string Version { get; }

        protected TaskBase()
        {
            _timer = new Timer();
            _timer.Elapsed += TimerElapsed;
        }

        /// <summary>
        /// 运行任务
        /// </summary>
        public virtual void Run()
        {
            _timer.Interval = TimerInterval;
            _timer.Start();
            _isRunning = true;
            LastWorkingTime = SystemTime.Now();
            LastWorkedTime = SystemTime.Now();
            LogFactory.GetLogger(GetType()).Info(Name+":开始运行");
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        public virtual void Stop()
        {
            _timer.Stop();
            _isRunning = false;
        }
        
        /// <summary>
        /// 任务是否正在运行
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRunning()
        {
            return _isRunning;
        }

        /// <summary>
        /// 任务时间片是否正在运行
        /// </summary>
        /// <returns></returns>
        public virtual bool IsBusy()
        {
            return _busy;
        }

        /// <summary>
        /// 立即运行任务执行逻辑
        /// </summary>
        public void DoWorkNow()
        {
            if (_busy)
            {
                return;
            }
            try
            {
                _busy = true;
                this.UpdateLastWorkStartedTime();
                LastWorkingTime = SystemTime.Now();
                DoWork(message =>
                {
                    _busy = false;
                    LastWorkedTime = SystemTime.Now();
                    this.UpdateLastWorkCompletedTime(message);
                });
            }
            catch (Exception ex)
            {
                var log = Ioc.Resolve<ILogService>();
                log.LogError(TryGetTaskId(), "Task错误[" + this.Name + "]", ex.GetAllMessages());
                this.UpdateLastWorkCompletedTime(ex.GetAllMessages());
                _busy = false;
            }
        }

        /// <summary>
        /// 定时执行任务执行逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_busy)
            {
                return;
            }
            try
            {
                if (IsTimeToWork())//满足时间间隔逻辑后才执行
                {
                    _busy = true;
                    this.UpdateLastWorkStartedTime();
                    LastWorkingTime = SystemTime.Now();
                    DoWork(message =>
                    {
                        LastWorkedTime = SystemTime.Now();
                        this.UpdateLastWorkCompletedTime(message);
                        _busy = false;
                    });
                }
            }
            catch (Exception ex)
            {
                var log = Ioc.Resolve<ILogService>();
                log.LogError(TryGetTaskId(), "Task错误[" + this.Name + "]", ex.GetAllMessages());
                this.UpdateLastWorkCompletedTime(ex.GetAllMessages());
                _busy = false;
            }
        }

        /// <summary>
        /// 更新任务开始执行的时间
        /// </summary>
        protected virtual void UpdateLastWorkStartedTime()
        {
            var service = Ioc.Resolve<IScheduledTaskService>();
            service.UpdateLastWorkStartedTime(this.GetTypeFullName());
        }

        /// <summary>
        /// 更新任务完成的时间
        /// </summary>
        /// <param name="message"></param>
        protected virtual void UpdateLastWorkCompletedTime(string message)
        {
            var service = Ioc.Resolve<IScheduledTaskService>();
            var log = Ioc.Resolve<ILogService>();
            var type = this.GetTypeFullName();
            service.UpdateLastWorkCompletedTime(type, Version);
            log.LogTaskWorkCompleted(TryGetTaskId(), Version + ": " + message);
        }

        /// <summary>
        /// 当前时间是否可以执行任务逻辑
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTimeToWork()
        {
            switch (this.Frequency)
            {
                case TaskFrequency.Custom:
                    throw new Exception("请重写TaskBase.IsTimeToWork 方法.");
                case TaskFrequency.Minute:
                    return SystemTime.Now().IsMinuteTime(LastWorkedTime, FrequencyTime);
                case TaskFrequency.Hourly:
                    return SystemTime.Now().IsHourlyTime(LastWorkedTime, FrequencyTime);
                case TaskFrequency.Daily:
                    return SystemTime.Now().IsDailyTime(LastWorkedTime, FrequencyTime);
                case TaskFrequency.Weekly:
                    if (FrequencyDayOfWeek == null)
                    {
                        throw new Exception("请设置FrequencyDayOfWeek");
                    }
                    return SystemTime.Now().IsWeeklyTime(LastWorkedTime, FrequencyTime, FrequencyDayOfWeek.Value);
                case TaskFrequency.Monthly:
                    return SystemTime.Now().IsMonthlyTime(LastWorkedTime, FrequencyTime);
            }
            return false;
        }

        /// <summary>
        /// 任务的类型名称
        /// </summary>
        /// <returns></returns>
        protected string GetTypeFullName()
        {
            return this.GetType().FullName;
        }

        /// <summary>
        /// 每周的平率
        /// </summary>
        protected virtual DayOfWeek? FrequencyDayOfWeek => null;

        /// <summary>
        /// 获得任务Id
        /// </summary>
        /// <returns></returns>
        protected virtual Guid TryGetTaskId()
        {
            if (_taskId != null)
            {
                return _taskId.Value;
            }
            var service = Ioc.Resolve<IScheduledTaskService>();
            _taskId = service.Get(this.GetTypeFullName()).Id;
            return _taskId.Value;
        }

        #region implementation of IDisposable

        private bool _disposed = false;

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
            _disposed = true;
        }

        ~TaskBase()
        {
            Dispose(false);
        }

        #endregion
    }
}
