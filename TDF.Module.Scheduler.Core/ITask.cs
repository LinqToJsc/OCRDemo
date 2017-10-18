using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Module.Scheduler.Core
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface ITask : IDisposable
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 执行间隔描述
        /// </summary>
        string IntervalDescription { get; }

        /// <summary>
        /// 版本
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 开始运行
        /// </summary>
        void Run();

        /// <summary>
        /// 停止运行
        /// </summary>
        void Stop();

        /// <summary>
        /// 是否在运行中
        /// </summary>
        /// <returns></returns>
        bool IsRunning();

        /// <summary>
        /// 任务是否正在忙碌中
        /// </summary>
        /// <returns></returns>
        bool IsBusy();

        /// <summary>
        /// 执行的任务
        /// </summary>
        void DoWorkNow();
    }
}
