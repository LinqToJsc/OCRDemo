using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using TDF.Core.Event;
using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Demo.AdminWeb
{
    /// <summary>
    /// 注册事件消费者接口
    /// </summary>
    public class EventDependencies : IDependencyRegistrar
    {
        public int Order => 3;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer).As(consumer.FindInterfaces((type, criteria) => {
                    return type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                }, typeof(IConsumer<>))).InstancePerLifetimeScope();
            }
        }
    }
}