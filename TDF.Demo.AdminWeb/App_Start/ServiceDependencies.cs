using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using TDF.Core.Ioc;
using TDF.Core.Reflection;
using TDF.Demo.Service;
using TDF.Demo.Service.SystemManage;
using TDF.Demo.Service.SystemManage.Implemented;
using TDF.Web.Authentication.Services;

namespace TDF.Demo.AdminWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceDependencies : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            var assembly = Assembly.GetAssembly(typeof(ISystemMemberService));
            builder.AutoRegisterByNamespace(
                new[] { "TDF.Demo.Service", "TDF.Demo.Service.SystemManage", "TDF.Demo.Service.DataDictionary" }, assembly,
                r => r.InstancePerLifetimeScope());

            builder.RegisterType<RoleAuthorizeService>().As<IRoleAuthorizeService>();
        }

        public int Order => 2;
    }
}