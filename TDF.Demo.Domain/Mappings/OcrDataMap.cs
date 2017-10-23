using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class OcrDataMap : EntityTypeConfiguration<OcrData>
    {
        public OcrDataMap()
        {
            ToTable("ocr_data");
        }
    }
}
