using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TDF.Core.Ioc;
using TDF.Core.Operator;
using TDF.Core.Reflection;
using TDF.Web.Authentication.Services;
using TDF.Web.Authentication.Services.Implements;

namespace TDF.Web.Authentication
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<CacheOperatorProvider>().As<IOperatorProvider>().InstancePerLifetimeScope();
            builder.RegisterType<WebApiIdentityProvider>().As<IIdentityProvider>().InstancePerLifetimeScope();
        }

        public int Order => 1;
    }
}
