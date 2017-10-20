using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;
using TDF.Module.Images.Entity;

namespace TDF.Demo.Domain.Mappings
{
    public class ImageMap : EntityTypeConfiguration<Image>
    {
        public ImageMap()
        {
            ToTable("Images");
        }
    }
}
