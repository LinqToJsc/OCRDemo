using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Domain.Mappings
{
    public class OcrDataMap : EntityTypeConfiguration<ocr_data>
    {
        public OcrDataMap()
        {
            ToTable("ocr_data");
            Property(x => x.Company).HasMaxLength(100);
            Property(x => x.DeliveryAddress).HasMaxLength(100);
            //Property(x => x.CreatedTime);

            //Property(x => x.Id);
            Property(x => x.InvoiceDate).HasMaxLength(100);
            Property(x => x.InvoiceNumber).HasMaxLength(100);
            Property(x => x.ListData).HasMaxLength(4000); 
            Property(x => x.TotalAmount).HasMaxLength(100);
        }
    }
}
