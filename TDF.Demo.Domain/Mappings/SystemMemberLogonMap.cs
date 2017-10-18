using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemMemberLogonMap : BaseEntityTypeConfiguration<SystemMemberLogon>
    {
        public SystemMemberLogonMap()
        {
            ToTable("System_Member_Logon");
            Property(x => x.Password).IsRequired().HasMaxLength(32);
            Property(x => x.Secretkey).IsRequired().HasMaxLength(32);
            HasRequired(x => x.SystemMember).WithRequiredDependent(x => x.SystemMemberLogon);
        }
    }
}
