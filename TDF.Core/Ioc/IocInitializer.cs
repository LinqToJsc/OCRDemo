using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using TDF.Core.Caching;
using TDF.Core.Configuration;
using TDF.Core.Event;
using TDF.Core.Reflection;

namespace TDF.Core.Ioc
{
    public class IocInitializer : AbstractInitializer
    {
        public ContainerBuilder Builder { get; set; }

        public Action<IContainer> SetResolver { get; set; }

        public override void Initialize()
        {
            var container = Builder.Build();
            if (SetResolver != null)
            {
                SetResolver.Invoke(container);
            }
            Ioc.ContainerManager = new ContainerManager(container);
            base.Initialize();
        }
    }

    public static class IocInitializerExt
    {
        public static InitializerContext IocInitialize(this InitializerContext context,Action<IocInitializer> configure = null, ITypeFinder typeFinder = null)
        {
            var initializer = new IocInitializer();
            initializer.Builder = new ContainerBuilder();
            initializer.Builder.RegisterType<Configs>().As<ISettings>().SingleInstance();
            initializer.Builder.RegisterType<Configs>().SingleInstance();
            initializer.Builder.RegisterType<MemoryCacheManager>().As<ICacheProvider>().SingleInstance();
            if (typeFinder == null)
            {
                typeFinder = new AppDomainTypeFinder();
            }
            initializer.Builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            initializer.Builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            var drInstancees = typeFinder.CreateInstanceOnFoundType<IDependencyRegistrar>();
            drInstancees = drInstancees.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstancees)
            {
                dependencyRegistrar.Register(initializer.Builder, typeFinder);
            }
            if (configure != null)
            {
                configure.Invoke(initializer);
            }
            context.Initialzer.Add(initializer);
            return context;
        }
    }
}
