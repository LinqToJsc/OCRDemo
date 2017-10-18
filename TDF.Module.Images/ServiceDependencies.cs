using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Module.Images
{
    public class ServiceDependencies : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerLifetimeScope();
        }

        public int Order => 2;
    }
}
