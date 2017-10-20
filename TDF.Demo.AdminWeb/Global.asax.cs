using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using TDF.Core.Configuration;
using TDF.Core.Ioc;
using TDF.Core.Log;
using TDF.Core.Module;
using TDF.Core.Operator;
using TDF.Data.EntityFramework.DbContext;
using TDF.Data.EntityFramework.Initializers;
using TDF.Data.EntityFramework.Migrations;
using TDF.Demo.Domain.Entities;
using TDF.Demo.Service.Dtos.SystemManage;
using TDF.Demo.Service.Mapper;
using TDF.Web.Authentication.Services.Implements;
using TDF.Web.Infrastructure;
using TDF.Web.ModelBinder;

namespace TDF.Demo.AdminWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //加载额外请求域名白名单
            RefererUriWhiteListConfig.LoadExtraRefererUri();
            //注册我们自己定义的模型绑定规则
            ModelBinderProviders.BinderProviders.Add(new FileModelBinderProvider());
            InitializerContext.Instance.IocInitialize(c =>
            {
                //这里可以做依赖注入
                //c.Builder.RegisterType<Class>().As<Interface>.InstancePerLifetimeScope();
                //c.Builder.RegisterType<DefaultDbContext>().As<AbstractDbContext>().InstancePerLifetimeScope();
                c.Builder.RegisterType<MySqlDbContext>().As<AbstractDbContext>();
                c.Builder.RegisterType<CacheOperatorProvider>().As<IOperatorProvider>();
                c.Builder.RegisterControllers(Assembly.GetExecutingAssembly());
                c.Builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
                c.SetResolver = x =>
                {
                    GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(x);
                    DependencyResolver.SetResolver(new AutofacDependencyResolver(x));
                };

            }, new WebAppTypeFinder())
                .EnableLog4Net(c =>
                {
                    c.ConfigPath = HttpContext.Current.Server.MapPath("/Configs/log4net.config");
                })
                //根据对象名称自动映射如： Entity1 <=> Entity1Dto，DTO对象以dto结尾，并实现接口IDto
                .InitializeAutoMapper(new AutoMapperInitializer(Assembly.GetAssembly(typeof (SystemMember)),
                    Assembly.GetAssembly(typeof (SystemMemberDto))))
                .InitializeDatabase<MySqlDbContext>(c =>
                    //.InitializeDatabase<DefaultDbContext>(c =>
                {
                    c.ConnectionName = "TDFMySqlDatabase";
                    //c.DatabaseInitializer = new CollegeDropCreateDatabaseIfModelChanges<DefaultDbContext>();//第一次创建数据库用此方法
                    //c.DatabaseInitializer = new EmptyDatabaseInitializer<DefaultDbContext>();//正式环境用此方法
                    //数据自动迁移模式，数据库会根据领域模型变化而自动更新表，适合开发中使用的模式。
                    //c.DatabaseInitializer = new MigrateDatabaseToLatestVersion<DefaultDbContext, Repository.Migrations.Configuration>();
                    if (Configs.Instance.EnvironmentType == EnvironmentType.Dev ||
                        Configs.Instance.EnvironmentType == EnvironmentType.Test)
                    {
                        //数据自动迁移模式，数据库会根据领域模型变化而自动更新表，适合开发中使用的模式。
                        c.DatabaseInitializer = new MigrateDatabaseToLatestVersion<MySqlDbContext, MySqlConfiguration>();
                        //c.DatabaseInitializer = new MigrateDatabaseToLatestVersion<TDFDbContext, TDFDbConfiguration> ();
                    }
                    else
                    {
                        //正式环境用此方法
                        c.DatabaseInitializer = new CreateDatabaseIfNotExists<MySqlDbContext>();
                    }
                })
                .MouduleInitialize()
                .ExecuteInit();
        }
    }
}