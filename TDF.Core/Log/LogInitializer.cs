using System;
using System.IO;
using TDF.Core.Configuration;

namespace TDF.Core.Log
{
    public class LogInitializer : AbstractInitializer
    {
        /// <summary>
        /// Log4Net的配置文件地址
        /// </summary>
        public string ConfigPath { get; set; }

        private bool ConfigComplete { get; set; }

        public override void Initialize()
        {
            if (!ConfigComplete)
            {
                var configFile = new FileInfo(ConfigPath);
                log4net.Config.XmlConfigurator.Configure(configFile);
            }
            LogProvider.SetLogType("log4net");
            base.Initialize();
        }
    }

    public static class LogInitialzerExt
    {
        public static InitializerContext EnableLog4Net(this InitializerContext context, Action<LogInitializer> configure = null) {
            var logInitialzer = new LogInitializer();
            if (configure != null)
            {
                configure.Invoke(logInitialzer);
            }
            context.Initialzer.Add(logInitialzer);
            return context;
        }

        public static InitializerContext DisableLog4Net(this InitializerContext context)
        {
            LogProvider.SetLogType("EmptyLog");
            return context;
        }
    }
}
