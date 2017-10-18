using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemModuleRoleMap : BaseEntityTypeConfiguration<SystemModuleRole>
    {
        public SystemModuleRoleMap()
        {
            ToTable("System_Module_Role");
            HasRequired(x => x.SystemRole)
                .WithMany(x => x.SystemModuleRoles)
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(false);
            HasRequired(x => x.SystemModule)
                .WithMany(x => x.SystemModuleRoles)
                .HasForeignKey(x => x.ModuleId)
                .WillCascadeOnDelete(false);
        }
    }
}
