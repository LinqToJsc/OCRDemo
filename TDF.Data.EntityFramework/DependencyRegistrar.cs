using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using TDF.Core.Ioc;
using TDF.Core.Reflection;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;

namespace TDF.Data.EntityFramework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<RepositoryBase>().As<IRepositoryBase>();
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>));
        }

        public int Order => 1;
    }
}
