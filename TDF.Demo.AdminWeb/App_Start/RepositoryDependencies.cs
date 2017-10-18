using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using TDF.Core.Ioc;
using TDF.Core.Reflection;

namespace TDF.Demo.AdminWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryDependencies : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {

        }

        public int Order => 1;
    }
}