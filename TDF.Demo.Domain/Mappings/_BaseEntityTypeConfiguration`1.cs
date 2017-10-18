using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public abstract class BaseEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : SystemEntity
    {
        protected BaseEntityTypeConfiguration()
        {
            Property(x => x.CreatorName).IsOptional().HasMaxLength(50);
            Property(x => x.ModifierName).IsOptional().HasMaxLength(50);
        }
    }
}
