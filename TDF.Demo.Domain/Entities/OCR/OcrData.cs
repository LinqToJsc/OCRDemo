using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Demo.Domain.Entities
{
    public class OcrData: EntityBase
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string TotalAmount { get; set; }

        public string Company { get; set; }

        [Column(TypeName = "text")]
        public string ListData { get; set; }
    }
}
