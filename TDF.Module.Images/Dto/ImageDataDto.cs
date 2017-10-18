using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Images.Enums;

namespace TDF.Module.Images.Dto
{
    public class ImageDataDto
    {
        public byte[] Bytes { get; set; }

        public ImageFormat Format { get; set; }
    }
}
