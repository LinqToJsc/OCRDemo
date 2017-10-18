using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;
using TDF.Module.Images.Enums;

namespace TDF.Module.Images.Entity
{
    public class Image : IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreateTime { get; set; }

        [MaxLength(256)]
        public string Key { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        public ImageFormat Format { get; set; }

        public BusinessType BusinessType { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public long Length { get; set; }

        public byte[] Bytes { get; set; }

        public static string GenerateKey(BusinessType type, Guid id, ImageFormat format)
        {
            var yearMonth = DateTime.Now.ToString("yyyyMM");
            return string.Format("t{0}t{3}-{1}{2}", type, id.ToString("n"), format.GetExtension(), yearMonth);
        }
    }
}
