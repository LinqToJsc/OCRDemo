using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TDF.Core;
using TDF.Core.Configuration;
using TDF.Core.Entity;
using TDF.Core.Models;

namespace TDF.Demo.Service.Mapper
{
    /// <summary>
    /// AutoMapper初始化器
    /// </summary>
    public class AutoMapperInitializer : AbstractInitializer
    {
        /// <summary>
        /// 实体所在的程序集
        /// </summary>
        public Assembly EntityAssembly { get; set; }

        /// <summary>
        /// Dto所在的程序集
        /// </summary>
        public Assembly DtoAssembly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityAssembly">实体所在的程序集</param>
        /// <param name="dtoAssembly">Dto所在的程序集</param>
        public AutoMapperInitializer(Assembly entityAssembly, Assembly dtoAssembly)
        {
            EntityAssembly = entityAssembly;
            DtoAssembly = dtoAssembly;
        }

        /// <summary>
        /// 凡是对象以Dto结尾且实现了Dto接口的类，我们就将起自动Map
        /// </summary>
        public override void Initialize()
        {
            var entityTypes = EntityAssembly.GetTypes().Where(p => typeof(IEntity<>).IsAssignableGenericFrom(p) || typeof(IEntity).IsAssignableGenericFrom(p)).ToList();
            var dtoTypes =
                DtoAssembly.GetTypes().Where(p => typeof(IDto).IsAssignableGenericFrom(p) && p.Name.EndsWith("Dto")).ToList();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                foreach (var model in entityTypes)
                {
                    var dtoType = dtoTypes.FirstOrDefault(d => d.Name == model.Name + "Dto");
                    if (dtoType != null)
                    {
                        cfg.CreateMap(model, dtoType);
                        cfg.CreateMap(dtoType, model);

                    }
                }
            });
            base.Initialize();
        }
    }

    /// <summary>
    /// AutoMapper手动配置初始化器
    /// </summary>
    public class AutoMapperCustomInitializer : AbstractInitializer
    {
        public Action<IMapperConfigurationExpression> Config { get; set; }

        /// <summary>
        /// 凡是对象以Dto结尾且实现了Dto接口的类，我们就将起自动Map
        /// </summary>
        public override void Initialize()
        {
            AutoMapper.Mapper.Initialize(Config);
            base.Initialize();
        }
    }

    public static class AutoMapperInitializerExt
    {
        public static InitializerContext InitializeAutoMapper(this InitializerContext context, AutoMapperInitializer initializer, Action<AutoMapperInitializer> configure = null)
        {
            if (configure != null)
            {
                configure.Invoke(initializer);
            }
            context.Initialzer.Add(initializer);
            return context;
        }

        public static InitializerContext InitializeAutoMapper(this InitializerContext context, Action<IMapperConfigurationExpression> config)
        {
            context.Initialzer.Add(new AutoMapperCustomInitializer()
            {
                Config = config
            });
            return context;
        }
    }

}
