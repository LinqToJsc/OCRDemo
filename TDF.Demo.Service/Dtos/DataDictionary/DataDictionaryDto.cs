using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Service.Dtos.DataDictionary
{
    public class DataDictionaryDto: DataDictionaryBasicDto
    {
        public DateTime? ModifiedTime { get; set; }

        public Guid? CreatorId { get; set; }

        [MaxLength(32)]
        public string CreatorName { get; set; }

        [MaxLength(32)]
        public string ModifierName { get; set; }

        [MaxLength(2000)]
        public string Values { get; set; }

        public bool? DeleteMark { get; set; }

        public DateTime? DeleteTime { get; set; }
    }
}
