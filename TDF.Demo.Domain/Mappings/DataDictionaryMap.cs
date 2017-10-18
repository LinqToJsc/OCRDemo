using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class DataDictionaryMap : BaseEntityTypeConfiguration<DataDictionary>
    {
        public DataDictionaryMap()
        {
            ToTable("Data_Dictionary");

            Property(x => x.Key).IsRequired().HasMaxLength(32);
            Property(x => x.Values).IsRequired().HasMaxLength(4000);

            HasRequired(x => x.DataDictionaryType)
                .WithMany(x => x.DataDictionarys)
                .HasForeignKey(x=> x.ParentId)
                .WillCascadeOnDelete(false);
        }
    }
}
