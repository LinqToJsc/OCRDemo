
using System;
using System.Linq;
using TDF.Core.Models;

namespace TDF.Demo.Service.Dtos.OcrData
{
    /// <summary>
    /// 预演 OCR 的采购单
    /// </summary>
    public class OcrDataDto : IDto
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string TotalAmount { get; set; }
        public string Company { get; set; }
        public string ListData { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    public static class OcrDataDtoExtensions
    {
        public static IQueryable<OcrDataDto> ToOcrDataDto(this IQueryable<Domain.Entities.ocr_data> query)
        {
            return query.Select(x => new OcrDataDto
            {
                Id = x.Id,
                Company = x.Company,
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                DeliveryAddress = x.DeliveryAddress,
                TotalAmount = x.TotalAmount,
                CreatedTime = x.CreatedTime,
                ListData = x.ListData
            });
        }
    }
}
