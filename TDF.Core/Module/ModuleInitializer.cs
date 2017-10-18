using System;
using System.Linq;
using TDF.Core.Configuration;
using TDF.Core.Reflection;

namespace TDF.Core.Module
{
    public class ModuleInitializer : AbstractInitializer
    {
        public override string ComponentName => "模块初始化工具";

        public override int Order => 4;

        public override void Initialize()
        {
            var drTypes = Ioc.Ioc.Resolve<ITypeFinder>().FindClassesOfType<IModuleInitializer>();
            var drInstancees = drTypes.Select(drType => (IModuleInitializer) Activator.CreateInstance(drType)).ToList();
            foreach (var module in drInstancees.OrderBy(x=>x.Order))
            {
                module.Initialize();
            }
        }
    }

    public static class IocInitializerExt
    {
        public static InitializerContext MouduleInitialize(this InitializerContext context)
        {
            var initializer = new ModuleInitializer();
            context.Initialzer.Add(initializer);
            return context;
        }
    }
}
