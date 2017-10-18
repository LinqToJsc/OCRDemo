using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class DataDictionaryTypeMap : BaseEntityTypeConfiguration<DataDictionaryType>
    {
        public DataDictionaryTypeMap()
        {
            ToTable("Data_DictionaryType");
            Property(x => x.Name).IsRequired().HasMaxLength(32);
            Property(x => x.Remark).HasMaxLength(200);
        }
    }
}
