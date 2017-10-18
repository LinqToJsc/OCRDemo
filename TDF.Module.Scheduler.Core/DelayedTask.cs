using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TDF.Module.Scheduler.Core
{
    /// <summary>
    /// 延迟任务
    /// </summary>
    public class DelayedTask : IDisposable
    {
        private readonly Timer _timer;
        private readonly Action _action;

        /// <summary>
        /// 是否执行完毕
        /// </summary>
        public bool Executed { get; private set; } = false;

        public string Key { get; }

        public DelayedTask(string key, int deplay, Action action)
        {
            Key = key;
            _action = action;
            _timer = new Timer(deplay);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        /// <summary>
        /// 取消任务执行
        /// </summary>
        public void Cancel()
        {
            _timer.Stop();
        }

        /// <summary>
        /// 定时器时间片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            _action();
            Executed = true;
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
            _timer.Dispose();
            _disposed = true;
        }

        ~DelayedTask()
        {
            Dispose(false);
        }

        #endregion
    }
}
