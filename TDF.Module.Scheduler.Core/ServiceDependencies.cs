using Autofac;
using TDF.Core.Ioc;
using TDF.Core.Reflection;
using TDF.Module.Scheduler.Core.Services;
using TDF.Module.Scheduler.Core.Services.Implementations;

namespace TDF.Module.Scheduler.Core
{
    public class ServiceDependencies : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<LogService>().As<ILogService>().SingleInstance();
            builder.RegisterType<ScheduledTaskService>().As<IScheduledTaskService>().InstancePerLifetimeScope();
        }

        public int Order => 2;
    }
}
