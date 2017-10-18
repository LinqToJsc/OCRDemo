using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class SystemActionMap : BaseEntityTypeConfiguration<SystemAction>
    {
        public SystemActionMap()
        {
            ToTable("System_Action");
            Property(x => x.Code).IsRequired().HasMaxLength(32);
            Property(x => x.Desc).HasMaxLength(128);
            Property(x => x.Name).IsRequired().HasMaxLength(32);
            Property(x => x.Url).IsRequired().HasMaxLength(1024);
            HasRequired(x => x.SystemModule).WithMany(x => x.SystemActions)
                .HasForeignKey(x => x.ModuleId
                ).WillCascadeOnDelete(false);
        }
    }
}
