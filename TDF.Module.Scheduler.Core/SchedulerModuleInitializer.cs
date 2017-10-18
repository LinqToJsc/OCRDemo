using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Configuration;
using TDF.Core.Log;

namespace TDF.Module.Scheduler.Core
{
    public class SchedulerModuleInitializer : IModuleInitializer
    {
        public int Order { get; } = 1;
        public string ComponentName { get; } = "任务调度器";
        public void Initialize()
        {
            LogFactory.GetLogger().Info("任务调度器模块已加载");
        }
    }
}
