using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Module.Scheduler.Core
{
    /// <summary>
    /// Dll文件监听器，用于确保将Dll从文件中加载到内存中
    /// </summary>
    public class DllWatcher : IDisposable
    {
        /// <summary>
        /// 延迟的任务集合
        /// </summary>
        private readonly List<DelayedTask> _delayedTasks;

        /// <summary>
        /// 系统文件监听器
        /// </summary>
        private readonly FileSystemWatcher _watcher;

        /// <summary>
        /// Dll文件改变事件
        /// </summary>
        public event EventHandler<GenericEventArgs<string>> OnDllChanged;

        /// <summary>
        /// 监听的文件夹
        /// </summary>
        /// <param name="watchingFolder"></param>
        public DllWatcher(string watchingFolder)
        {
            _delayedTasks = new List<DelayedTask>();
            _watcher = new FileSystemWatcher(watchingFolder, "*.dll")
            {
                EnableRaisingEvents = true
            };
            _watcher.Changed += FolderChanged;
        }

        /// <summary>
        /// 文件目录中文件更改事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FolderChanged(object sender, FileSystemEventArgs e)
        {
            lock (_delayedTasks)
            {
                //当dll文件在目录中被删除时也移除对应内存中的延迟任务
                var existing = _delayedTasks.FirstOrDefault(x => x.Key == e.FullPath);
                if (existing != null)
                {
                    existing.Cancel();
                    existing.Dispose();
                    _delayedTasks.Remove(existing);
                }
                //延迟1000毫秒来确保dll完全上传到服务器目录，然后再触发其事件
                var task = new DelayedTask(e.FullPath, 1000, () =>
                {
                    if (this.OnDllChanged != null)
                    {
                        //触发Dll更改完毕事件
                        this.OnDllChanged.Invoke(null, new GenericEventArgs<string>(e.FullPath));
                    }
                    lock (_delayedTasks)
                    {
                        //将执行完毕的延迟任务移除
                        _delayedTasks.Where(x => x.Executed).ToList().ForEach(x => x.Dispose());
                        _delayedTasks.RemoveAll(x => x.Executed);
                    }
                });
                _delayedTasks.Add(task);
            }
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
            _watcher.Dispose();
            lock (_delayedTasks)
            {
                foreach (var task in _delayedTasks)
                {
                    task.Dispose();
                }
                _delayedTasks.Clear();
            }
            _disposed = true;
        }

        ~DllWatcher()
        {
            Dispose(false);
        }

        #endregion
    }
}
