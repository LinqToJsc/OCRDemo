using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.DataDictionary
{
    public class DataDictionaryTypeBasicDto : IDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public string Code { get; set; }
    }
}
