using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Domain.Entities
{
    public class OcrListData: EntityBase
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }

        public string Total { get; set; }


    }
}
