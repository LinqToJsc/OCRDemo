using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using TDF.Core;
using TDF.Core.Configuration;
using TDF.Core.Exceptions;
using TDF.Core.Log;

namespace TDF.Data.EntityFramework.DbContext
{
    public abstract class AbstractDbContext : System.Data.Entity.DbContext
    {
        protected AbstractDbContext(string nameOrConnectionstring)
            : base(nameOrConnectionstring)
        {

        }

        protected AbstractDbContext() : base(Configs.Instance.ConnectionString)
        {
            
        }

        /// <summary>
        /// 获得EF的映射程序集
        /// </summary>
        public abstract Assembly[] GetMappingAssemblies();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var assemblies = GetMappingAssemblies();
            if (assemblies == null)
            {
                throw new TDFException("Mapping未加载");
            }
            var typesToRegister = assemblies.ToList().SelectMany(x=>x.GetTypes())
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null 
                && (type.BaseType.IsInherit(typeof(EntityTypeConfiguration<>))||type.BaseType.IsInherit(typeof(StructuralTypeConfiguration<>))) 
                && !type.IsAbstract).ToList();
            if (!typesToRegister.Any())
            {
                LogFactory.GetLogger(GetType()).Warn("Mapping没找到");
            }
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
