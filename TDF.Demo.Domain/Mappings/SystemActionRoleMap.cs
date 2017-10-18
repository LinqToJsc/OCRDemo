using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemActionRoleMap : BaseEntityTypeConfiguration<SystemActionRole>
    {
        public SystemActionRoleMap()
        {
            ToTable("System_Action_Role");
            HasRequired(x => x.SystemAction)
                .WithMany(x => x.SystemActionRoles)
                .HasForeignKey(x => x.ActionId)
                .WillCascadeOnDelete(false);
            HasRequired(x => x.SystemRole)
                .WithMany(x => x.SystemActionRoles)
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(false);
        }
    }
}
