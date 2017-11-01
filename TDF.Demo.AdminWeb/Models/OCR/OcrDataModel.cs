using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDF.Demo.AdminWeb.Models.OCR
{
    public class OcrDataModel
    {
       public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string TotalAmount { get; set; }

        public string Company { get; set; }
        
        public string ListData { get; set; }
    }
}