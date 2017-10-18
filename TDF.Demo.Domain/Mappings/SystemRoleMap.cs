using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemRoleMap : BaseEntityTypeConfiguration<SystemRole>
    {
        public SystemRoleMap()
        {
            ToTable("System_Role");
            Property(x => x.Desc).HasMaxLength(64);
            Property(x => x.Name).IsRequired().HasMaxLength(16);
        }
    }
}
