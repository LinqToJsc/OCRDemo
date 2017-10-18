using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.DataDictionary
{
    public class DataDictionaryBasicDto: IDto
    {
        [Required, MaxLength(20)]
        public string Name { get; set; }

        public Guid Id { get; set; }

        public DateTime? CreatedTime { get; set; }

        public Guid ParentId { get; set; }

        public int Sort { get; set; }

        public string Key { get; set; }

        public bool IsSystem { get; set; }

        public bool Disabled { get; set; }

        public string TypeName { get; set; }
    }
}
