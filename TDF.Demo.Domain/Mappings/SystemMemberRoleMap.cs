using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemMemberRoleMap : BaseEntityTypeConfiguration<SystemMemberRole>
    {
        public SystemMemberRoleMap()
        {
            ToTable("System_Member_Role");
            HasRequired(x => x.SystemMember)
                .WithMany(x => x.SystemMemberRoles)
                .HasForeignKey(x => x.SystemMemberId)
                .WillCascadeOnDelete(false);
            HasRequired(x => x.SystemRole)
                .WithMany(x => x.SystemMemberRoles)
                .HasForeignKey(x => x.SystemRoleId)
                .WillCascadeOnDelete(false);
        }
    }
}
