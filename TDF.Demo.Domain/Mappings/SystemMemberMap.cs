using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemMemberMap : BaseEntityTypeConfiguration<SystemMember>
    {
        public SystemMemberMap()
        {
            ToTable("System_Member");
            //Account 唯一键
            Property(x => x.Account).IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute() { IsUnique = true })).HasMaxLength(32);
            Property(x => x.Email).HasMaxLength(150);
            Property(x => x.MobilePhone).HasMaxLength(32);
            Property(x => x.QQ).HasMaxLength(32);
            Property(x => x.RealName).HasMaxLength(100);

        }
    }
}
