using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemModuleMap : BaseEntityTypeConfiguration<SystemModule>
    {
        public SystemModuleMap()
        {
            ToTable("System_Module");
            Property(x => x.Desc).HasMaxLength(64);
            Property(x => x.Name).IsRequired().HasMaxLength(32);
            Property(x => x.Code).IsRequired().HasMaxLength(32);
            Property(x => x.IconClass).HasMaxLength(32);
        }
    }
}
