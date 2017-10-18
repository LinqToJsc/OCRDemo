using Autofac;
using Autofac.Builder;
using System;
using System.Linq;
using System.Reflection;

namespace TDF.Core.Ioc
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 自动注册指定命名空间下的类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="namespances"></param>
        /// <param name="assembly"></param>
        /// <param name="action"></param>
        public static void AutoRegisterByNamespace(this ContainerBuilder builder, string[] namespances, Assembly assembly, Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> action = null)
        {
            var types = assembly.GetTypes().Where(x => namespances.Contains(x.Namespace));
            foreach (var type in types)
            {
                var interfaceType = type.GetInterface("I" + type.Name);
                if (interfaceType == null)
                {
                    continue;
                }
                var reg = builder.RegisterType(type).As(interfaceType);
                action?.Invoke(reg);
            }
        }
    }
}
