using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.DataDictionary
{
    public class DataDictionaryTypeDto: IDto
    {
        public Guid Id { get; set; }

        public DateTime? ModifiedTime { get; set; }

        public Guid? CreatorId { get; set; }

        [MaxLength(32)]
        public string CreatorName { get; set; }

        [MaxLength(32)]
        public string ModifierName { get; set; }

        public DateTime? CreatedTime { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Remark { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? DeletedTime { get; set; }
    }
}
